using System;
using AcmeCarRental.AOP.PostSharp;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataService = new FakeLoyaltyDataService();
            var accrualService = new LoyaltyAccrualServiceAop(dataService);
            SimulateAddingPoints(accrualService);

            Console.WriteLine();
            Console.WriteLine(" ***");
            Console.WriteLine();

            var redeemService = new LoyaltyRedemptionServiceAop(dataService);
            SimulateRemovingPoints(redeemService);

            Console.WriteLine();
            Console.WriteLine();

            //var svc = new LoyaltyRedemptionServiceAop(dataService);
            //svc.Redeem(new Invoice(), 0);
        }

        static void SimulateAddingPoints(ILoyaltyAccrualService service)
        {
            var rentalAgreement = new RentalAgreement
            {
                Customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Matthew D. Groves",
                    DateOfBirth = new DateTime(1980, 2, 10),
                    DriversLicense = "RR123456"
                },
                Vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    Make = "Honda",
                    Model = "Accord",
                    Size = Size.Compact,
                    Vin = "1HABC123"
                },
                StartDate = DateTime.Now.AddDays(-3),
                EndDate = DateTime.Now
            };
            service.Accrue(rentalAgreement);
        }

        static void SimulateRemovingPoints(ILoyaltyRedemptionService service)
        {
            var invoice = new Invoice
            {
                Customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Jacob Watson",
                    DateOfBirth = new DateTime(1977, 4, 15),
                    DriversLicense = "RR009911"
                },
                Vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    Make = "Cadillac",
                    Model = "Sedan",
                    Size = Size.Luxury,
                    Vin = "2BDI"
                },
                CostPerDay = 29.95m,
                Id = Guid.NewGuid()
            };
            service.Redeem(invoice, 3);
        }
    }
}
