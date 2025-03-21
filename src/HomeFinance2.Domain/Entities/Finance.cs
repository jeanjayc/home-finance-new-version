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

        private string Name { get; set; }
        private string  Description { get; set; }
        private decimal Value { get; set; }
        private int? NumberInstallments { get; set; }
        private bool HasInstallments { get; set; }  
        private int ValueInstallments { get; set; }
        private bool Payd { get; set; } 

        public void CalculateInstallments()
        {
            if(HasInstallments)
                NumberInstallments = (int?)(Value / NumberInstallments);
        }

        public bool Pay()
        {
            return Payd;
        }
    }
}
