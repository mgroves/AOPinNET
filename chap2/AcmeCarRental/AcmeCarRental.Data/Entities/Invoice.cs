using System;

namespace AcmeCarRental.Data.Entities
{
    public class Invoice : ILoggable
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal Discount { get; set; }

        public string LogInformation()
        {
            return "Invoice: " + Id;
        }
    }
}