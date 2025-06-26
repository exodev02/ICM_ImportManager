using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ICM_ImportManager.Models;

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

        public async Task UploadToApiAsync(ImportModel model)
        {
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(_apiUrl, content);
        }
    }
}