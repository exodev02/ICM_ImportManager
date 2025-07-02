using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ICM_ImportManager.Models
{
    public partial class ImportQueryApiResponse
    {
        [JsonPropertyName("columnDefinitions")]
        public ColumnDefinition[] ColumnDefinitions { get; set; }

        [JsonPropertyName("data")]
        public List<List<JsonElement>> Data { get; set; }
    }

    public partial class ColumnDefinition
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("nullable")]
        public bool Nullable { get; set; }
    }

    public class ImportQuery
    {
        public int ImportID { get; set; }
        public string Query { get; set; }
    }
}
