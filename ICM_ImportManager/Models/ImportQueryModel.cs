using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ICM_ImportManager.Models
{
    public partial class ImportQueryModel
    {
        [JsonPropertyName("columnDefinitions")]
        public ColumnDefinition[] ColumnDefinitions { get; set; }

        [JsonPropertyName("data")]
        public Datum[] Data { get; set; }
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

    public partial class Datum
    {
        public Object[] content { get; set; }
    }

    //public partial struct Datum
    //{
    //    public long? Integer;
    //    public string String;

    //    public static implicit operator Datum(long Integer) => new Datum { Integer = Integer };
    //    public static implicit operator Datum(string String) => new Datum { String = String };
    //}

    public class ImportQuery
    {
        [JsonPropertyName("name")]
        public int ImportID { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }
    }

    internal class DatumConverter : JsonConverter<Datum>
    {
        public override bool CanConvert(Type t) => t == typeof(Datum);

        public override Datum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    var integerValue = reader.GetInt64();
                    return new Datum { Integer = integerValue };
                case JsonTokenType.String:
                    var stringValue = reader.GetString();
                    return new Datum { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Datum");
        }

        public override void Write(Utf8JsonWriter writer, Datum value, JsonSerializerOptions options)
        {
            if (value.Integer != null)
            {
                JsonSerializer.Serialize(writer, value.Integer.Value, options);
                return;
            }
            if (value.String != null)
            {
                JsonSerializer.Serialize(writer, value.String, options);
                return;
            }
            throw new Exception("Cannot marshal type Datum");
        }

        public static readonly DatumConverter Singleton = new DatumConverter();
    }
}
