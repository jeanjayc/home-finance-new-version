using System.Text.Json.Serialization;

namespace HomeFinance2.Domain.Entities
{
    public abstract class Entity
    {
        [JsonPropertyName("Pk")]
        public string Pk { get; set; }
    }
}
