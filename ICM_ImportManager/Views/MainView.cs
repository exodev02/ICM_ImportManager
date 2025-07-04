using ICM_ImportManager.Controllers;
using ICM_ImportManager.Models;
using System;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICM_ImportManager.Views
{
    public class MainView
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("ICM IMPORT MANAGER v0.1");
            Console.WriteLine("Que operacion va a realizar? 0 = Crear | 1 = Actualizar");
            int opc = Convert.ToInt32(Console.ReadLine());

            if (opc == 0) // CREAR
            {
                int respuesta = 0;

                do
                {
                    string name = "";
                    string query = "";
                    string table = "";

                    

                    Console.Write("Nombre ► ");
                    Console.ReadLine();
                    Console.Write("Query ► ");
                    Console.ReadLine();
                    Console.Write("Tabla ► ");
                    Console.ReadLine();

                    var import = new ImportModel
                    {
                        Name = name,
                        ImportType = "DBImport",
                        HasHeader = true,
                        Table = table,
                        ColumnMatchings = new ColumnMatchings
                        {
                            Columns = [],
                            Matched = [],
                        },
                        DateFormat = "MonthFirst",
                        ListSubitems = [],
                        SubitemMap = { },
                        AddMember = false,
                        UpdateExistingRows = false,
                        TableType = "Custom",
                        IsLocal = false,
                        UseIncrementalImport = false,
                        Culture = "en-US",
                        TableEffectiveDated = false,
                        IsOdbcTextDriver = false,
                        Version = { RowVersion = 0 },
                        FileOverwrite = false,
                        ImportId = 0,
                        Delimiter = "\u0000",
                        Query = query,
                        QueryTimeout = 300,
                        UseAdvanced = false,
                        Model = ConfigurationManager.AppSettings["Model"],
                        ImportMethod = 0,
                        UserSelected = false,
                        Sandbox = false,
                        CodePage = 0,
                        IgnoreFirst = false,
                        IgnoreLast = false,
                        RecordLength = 0,
                        UploadStage = false,
                        PredictStage = false,
                        DownloadStage = false,
                        SymonImportType = "None",
                        RefreshAllPipeDatasources = false,
                    };

                    Console.WriteLine("Desea crear otra importacion? 0 = NO | 1 = SI");
                    respuesta = Convert.ToInt32(Console.ReadLine());
                } while (respuesta != 0);


            } else // ACTUALIZA
            {
                Console.WriteLine("Seleccior metodo de actualizacion. 0 = Mediante el API de ICM | 1 = ");
            }
            //string folderPath = ConfigurationManager.AppSettings["JsonFolderPath"];
            string apiUrl = ConfigurationManager.AppSettings["ApiURL"];
            string pattern = @"(?i)\s*from\s+""[^""]+""\s*";

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

            foreach (var import in combinedImports)
            {
                string query = import.Query;

                Match match = Regex.Match(query, pattern);

                Console.WriteLine($"\n[INFO] Import ► {import.Name}");

                if (match.Success)
                    Console.WriteLine("\t> Sintax ► PostgreSQL");
                else
                    Console.WriteLine("\t> Syntax ► SQL Server");

                    Console.WriteLine($"\t> Query ► {import.Query}");
                //await controller.UploadImportsAsync(import);
                //Console.WriteLine($"Uploaded ► {import.Name}");
                //await controller.UpdateImportsAsync(import);
                //Console.WriteLine($"Updated ► {import.Name}");
            }

            Console.WriteLine("[INFO] Proceso finalizado!");
        }
    }
}