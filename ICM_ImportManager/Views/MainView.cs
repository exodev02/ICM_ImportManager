using ICM_ImportManager.Controllers;
using ICM_ImportManager.Models;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace ICM_ImportManager.Views
{
    public class MainView
    {
        public static async Task Main(string[] args)
        {
            //string folderPath = ConfigurationManager.AppSettings["JsonFolderPath"];
            string apiUrl = ConfigurationManager.AppSettings["ApiURL"];

            var controller = new ImportController(apiUrl);
            //var imports = controller.ReadJsonFiles(folderPath);
            Console.WriteLine("[INFO] Obteniendo importaciones de tipo SQL desde ICM...");
            var imports = await controller.GetAllImports();
            Console.WriteLine("[INFO] Obteniendo los querys para cada importacion desde ICM...");
            var queryList = await controller.GetImportQuerys();

            Console.WriteLine("[INFO] Procesando importaciones");
            var combinedImports = from import in imports
                                  join query in queryList
                                  //on import.ImportId equals query.ImportID // los id pueden cambiar
                                  on import.Name equals query.Name
                                  select new ImportModel
                                  {
                                      Name = import.Name,
                                      ImportType = import.ImportType,
                                      HasHeader = import.HasHeader,
                                      Table = import.Table,
                                      ColumnMatchings = import.ColumnMatchings,
                                      DateFormat = import.DateFormat,
                                      ListSubitems = import.ListSubitems,
                                      SubitemMap = import.SubitemMap,
                                      AddMember = import.AddMember,
                                      UpdateExistingRows = import.UpdateExistingRows,
                                      TableType = import.TableType,
                                      IsLocal = import.IsLocal,
                                      UseIncrementalImport = import.UseIncrementalImport,
                                      Culture = import.Culture,
                                      TableEffectiveDated = import.TableEffectiveDated,
                                      IsOdbcTextDriver = import.IsOdbcTextDriver,
                                      Version = import.Version,
                                      FileOverwrite = import.FileOverwrite,
                                      ImportId = import.ImportId,
                                      Delimiter = import.Delimiter,
                                      Query = query.Query,
                                      QueryTimeout = import.QueryTimeout,
                                      UseAdvanced = import.UseAdvanced,
                                      Model = import.Model,
                                      ImportMethod = import.ImportMethod,
                                      UserSelected = import.UserSelected,
                                      Sandbox = import.Sandbox,
                                      CodePage = import.CodePage,
                                      IgnoreFirst = import.IgnoreFirst,
                                      IgnoreLast = import.IgnoreLast,
                                      RecordLength = import.RecordLength,
                                      UploadStage = import.UploadStage,
                                      PredictStage = import.PredictStage,
                                      DownloadStage = import.DownloadStage,
                                      SymonImportType = import.SymonImportType,
                                      RefreshAllPipeDatasources = import.RefreshAllPipeDatasources,
                                  };

            foreach (var item in combinedImports)
            {
                Console.WriteLine($"[INFO] Updated ► {item.Name} \n {item.Query}");
                //await controller.UploadImportsAsync(model);
                //Console.WriteLine($"Uploaded ► {model.Name}");
                //await controller.UpdateImportsAsync(model);
                //Console.WriteLine($"Updated ► {model.Name}");
            }

            Console.WriteLine("[INFO] Proceso finalizado!");
        }
    }
}