using System;
using System.Configuration;
using System.Threading.Tasks;
using ICM_ImportManager.Controllers;

namespace ICM_ImportManager.Views
{
    public class MainView
    {
        public static async Task Main(string[] args)
        {
            string folderPath = ConfigurationManager.AppSettings["JsonFolderPath"];
            string apiUrl = ConfigurationManager.AppSettings["APIURL"];

            var controller = new ImportController(apiUrl);
            var models = controller.ReadJsonFiles(folderPath);

            foreach (var model in models)
            {
                //await controller.UploadImportsAsync(model);
                //Console.WriteLine($"Subido ► {model.Name}");
                await controller.UpdateImportsAsync(model);
                Console.WriteLine($"Actualizado ► {model.Name}");
            }

            Console.WriteLine("Proceso finalizado.");
        }
    }
}