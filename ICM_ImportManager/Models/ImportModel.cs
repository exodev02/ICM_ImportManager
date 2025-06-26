using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace ICM_ImportManager.Models
{
    public partial class ImportModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("importType")]
        public string ImportType { get; set; }

        [JsonPropertyName("hasHeader")]
        public bool HasHeader { get; set; }

        [JsonPropertyName("table")]
        public string Table { get; set; }

        [JsonPropertyName("columnMatchings")]
        public ColumnMatchings ColumnMatchings { get; set; }

        [JsonPropertyName("dateFormat")]
        public string DateFormat { get; set; }

        [JsonPropertyName("listSubitems")]
        public object[] ListSubitems { get; set; }

        [JsonPropertyName("subitemMap")]
        public SubitemMap SubitemMap { get; set; }

        [JsonPropertyName("addMember")]
        public bool AddMember { get; set; }

        [JsonPropertyName("updateExistingRows")]
        public bool UpdateExistingRows { get; set; }

        [JsonPropertyName("tableType")]
        public string TableType { get; set; }

        [JsonPropertyName("isLocal")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("useIncrementalImport")]
        public bool UseIncrementalImport { get; set; }

        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("tableEffectiveDated")]
        public bool TableEffectiveDated { get; set; }

        [JsonPropertyName("isODBCTextDriver")]
        public bool IsOdbcTextDriver { get; set; }

        [JsonPropertyName("version")]
        public Version Version { get; set; }

        [JsonPropertyName("fileOverwrite")]
        public bool FileOverwrite { get; set; }

        [JsonPropertyName("importId")]
        public long ImportId { get; set; }

        [JsonPropertyName("delimiter")]
        public string Delimiter { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("queryTimeout")]
        public long QueryTimeout { get; set; }

        [JsonPropertyName("useAdvanced")]
        public bool UseAdvanced { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("importMethod")]
        public long ImportMethod { get; set; }

        [JsonPropertyName("userSelected")]
        public bool UserSelected { get; set; }

        [JsonPropertyName("sandbox")]
        public bool Sandbox { get; set; }

        [JsonPropertyName("codePage")]
        public long CodePage { get; set; }

        [JsonPropertyName("ignoreFirst")]
        public bool IgnoreFirst { get; set; }

        [JsonPropertyName("ignoreLast")]
        public bool IgnoreLast { get; set; }

        [JsonPropertyName("recordLength")]
        public long RecordLength { get; set; }

        [JsonPropertyName("uploadStage")]
        public bool UploadStage { get; set; }

        [JsonPropertyName("predictStage")]
        public bool PredictStage { get; set; }

        [JsonPropertyName("downloadStage")]
        public bool DownloadStage { get; set; }

        [JsonPropertyName("symonImportType")]
        public string SymonImportType { get; set; }

        [JsonPropertyName("refreshAllPipeDatasources")]
        public bool RefreshAllPipeDatasources { get; set; }
    }

    public partial class ColumnMatchings
    {
        [JsonPropertyName("columns")]
        public string[] Columns { get; set; }

        [JsonPropertyName("matched")]
        public string[] Matched { get; set; }
    }

    public partial class SubitemMap
    {
    }

    public partial class Version
    {
        [JsonPropertyName("rowVersion")]
        public long RowVersion { get; set; }
    }
}
 