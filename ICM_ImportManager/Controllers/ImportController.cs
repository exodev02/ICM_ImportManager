using ICM_ImportManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICM_ImportManager.Controllers
{
    public class ImportController
    {
        private readonly string _apiUrl;

        public ImportController(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public List<ImportModel> ReadJsonFiles(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.json");
            var models = new List<ImportModel>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var model = JsonSerializer.Deserialize<ImportModel>(json);
                if (model != null)
                    models.Add(model);
            }

            return models;
        }

        public async Task UploadImportsAsync(ImportModel model)
        {
            string endpoint = $"{_apiUrl}/api/v1/imports/";

            model.DateFormat = "MonthFirst";
            model.Version.RowVersion = 0;
            model.ImportId = 0;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Model", ConfigurationManager.AppSettings["Model"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["authToken"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(endpoint, content);
        }

        public async Task UpdateImportsAsync(ImportModel model)
        {
            string endpoint = $"{_apiUrl}/api/v1/imports/{model.ImportId}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Model", ConfigurationManager.AppSettings["Model"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["authToken"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync(endpoint, content);
        }

        public async Task<List<ImportQuery>> GetImportQuerys()
        {
            string endpoint = $"{_apiUrl}/api/v1/rpc/querytool";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Model", ConfigurationManager.AppSettings["Model"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["authToken"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = new
            {
                queryString = "SELECT DISTINCT \"ImportID\" ,\"Query\" FROM \"ImportDB\"",
                offset = 0,
                limit = 900
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                
                var apiResponse = JsonSerializer.Deserialize<ImportQueryApiResponse>(json, options);

                var importQuerys = new List<ImportQuery>();
                foreach (var row in apiResponse.Data)
                {
                    if (row.Count >= 2)
                    {
                        var importID = row[0].GetInt32();
                        var query = row[1].GetString();

                        importQuerys.Add(new ImportQuery
                        {
                            ImportID = importID,
                            Query = query
                        });
                    }
                    else
                    {
                        Console.WriteLine("Advertencia: Fila de datos inesperada sin ImportID o Query.");
                    }
                }
                return importQuerys;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP: {ex.StatusCode} — {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error de Deserialización JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error General: {ex.Message}");
            }

            return new List<ImportQuery>(); // Si algo salió mal, retorna una lista vacía
        }

        public async Task<List<ImportModel>> GetAllImports()
        {
            string endpoint = $"{_apiUrl}/api/v1/imports";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Model", ConfigurationManager.AppSettings["Model"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["authToken"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var imports = JsonSerializer.Deserialize<List<ImportModel>>(json);

            //foreach (var import in imports)
            //{
            //    if (import.ImportType != "DBImport")
            //        imports.Remove(import);
            //}

            return imports ?? new List<ImportModel>();
        }
    }
}