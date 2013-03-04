using System;

namespace AcmeCarRental.Data.Entities
{
    public class RentalAgreement : ILoggable
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string LogInformation()
        {
            return "Customer: " + Customer.Id
                   + "\n" +
                   "Vehicle: " + Vehicle.Id;
        }
    }
}