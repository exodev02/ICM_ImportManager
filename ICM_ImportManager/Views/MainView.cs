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
                await controller.UploadToApiAsync(model);
                Console.WriteLine($"Subido: {model.Name}");
                //Console.WriteLine($"Cargado: {model.Name}, {model.Version.RowVersion = 0}, {model.ImportId}, {model.DateFormat}");
            }

            Console.WriteLine("Proceso finalizado.");
        }
    }
}