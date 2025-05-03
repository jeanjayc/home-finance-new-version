using System.Text.Json.Serialization;

namespace HomeFinance2.Domain.Entities
{
    public class Finance : Entity
    {
        public Finance(string name, string description, decimal value, int? numberInstallments)
        {
            Name = name;
            Description = description;
            Value = value;
            NumberInstallments = numberInstallments;
        }

        [JsonPropertyName("Id")] public string Id { get; set; }

        [JsonPropertyName("Name")] public string Name { get; set; }

        [JsonPropertyName("Description")] public string Description { get; set; }

        [JsonPropertyName("Value")] public decimal Value { get; set; }

        [JsonPropertyName("NumberInstallments")]
        public int? NumberInstallments { get; set; }

        [JsonPropertyName("HasInstallments")] public bool HasInstallments { get; set; }

        [JsonPropertyName("ValueInstallments")]
        public int ValueInstallments { get; set; }

        [JsonPropertyName("Payd")] public bool Payd { get; set; }

        public void CalculateInstallments()
        {
            if (HasInstallments)
                NumberInstallments = (int?)(Value / NumberInstallments);
        }

        public bool Pay()
        {
            return Payd;
        }
    }
}