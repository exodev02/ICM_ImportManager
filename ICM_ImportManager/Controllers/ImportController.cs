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



                var importQueryModel = JsonSerializer.Deserialize<ImportQueryModel>(json, options);

                // Mapeo de los datos a una lista de ImportQuery
                var importQuerys = new List<ImportQuery>();


                foreach (var row in importQueryModel.Data)
                {
                    var importID = row[0].Integer ?? 0;
                    var query = row[1].String;

                    importQuerys.Add(new ImportQuery
                    {
                        ImportID = (int)importID,
                        Query = query
                    });
                }

                //var responseString = await response.Content.ReadAsStringAsync();
                //var result = JsonSerializer.Deserialize<ImportQueryModel>(responseString);

                //// Si quieres mapear manualmente a objetos fuertemente tipados:
                //var queries = result.Data.Select(row => new ImportQueryModel
                //{
                //    ImportID = Convert.ToInt32(row[0]),
                //    Query = row[1].ToString()
                //}).ToList();

                //return queries;
                return importQuerys;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{ex.StatusCode} — {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return new List<ImportQuery>(); // Si algo salio mal retorna una lista vacia
        }
    }
}