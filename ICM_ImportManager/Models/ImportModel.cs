using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICM_ImportManager.Models
{
    public class ImportModel
    {
        public string Name { get; set; }
        public string ImportType { get; set; }
        public bool HasHeader { get; set; }
        public string Table { get; set; }
        public ColumnMatchings ColumnMatchings { get; set; }
        public string DateFormat { get; set; }
        public object[] ListSubitems { get; set; }
        public SubitemMap SubitemMap { get; set; }
        public bool AddMember { get; set; }
        public bool UpdateExistingRows { get; set; }
        public string TableType { get; set; }
        public bool IsLocal { get; set; }
        public bool UseIncrementalImport { get; set; }
        public string Culture { get; set; }
        public bool TableEffectiveDated { get; set; }
        public bool IsOdbcTextDriver { get; set; }
        public Version Version { get; set; }
        public bool FileOverwrite { get; set; }
        public long ImportId { get; set; }
        public string Delimiter { get; set; }
        public string Query { get; set; }
        public long QueryTimeout { get; set; }
        public bool UseAdvanced { get; set; }
        public string Model { get; set; }
        public long ImportMethod { get; set; }
        public bool UserSelected { get; set; }
        public bool Sandbox { get; set; }
        public long CodePage { get; set; }
        public bool IgnoreFirst { get; set; }
        public bool IgnoreLast { get; set; }
        public long RecordLength { get; set; }
        public bool UploadStage { get; set; }
        public bool PredictStage { get; set; }
        public bool DownloadStage { get; set; }
        public string SymonImportType { get; set; }
        public bool RefreshAllPipeDatasources { get; set; }
    }

    public partial class ColumnMatchings
    {
        public string[] Columns { get; set; }
        public string[] Matched { get; set; }
    }

    public partial class SubitemMap
    {
    }

    public partial class Version
    {
        public long RowVersion { get; set; }
    }
}
 