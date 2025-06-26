using ICM_ImportManager.Models;
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
                {
                    model.DateFormat = "MonthFirst";
                    model.Version.RowVersion = 0;
                    model.ImportId = 0;
                    models.Add(model);
                }
                    
            }

            return models;
        }

        public async Task UploadToApiAsync(ImportModel model)
        {
            string endpoint = _apiUrl + "/api/v1/imports";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Model", ConfigurationManager.AppSettings["Model"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["authToken"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(endpoint, content);
        }
    }
}