using System;
using System.Transactions;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental
{
    public class LoyaltyAccrualService : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _dataService;

        public LoyaltyAccrualService(ILoyaltyDataService service)
        {
            _dataService = service;
        }

        public void Accrue(RentalAgreement agreement)
        {
            // defensive programming
            if(agreement == null) throw new ArgumentNullException("agreement");

            // logging
            Console.WriteLine("Accrue: {0}", DateTime.Now);
            Console.WriteLine("Customer: {0}", agreement.Customer.Id);
            Console.WriteLine("Vehicle: {0}", agreement.Vehicle.Id);

            // exception handling
            try
            {
                // start new transaction
                using (var scope = new TransactionScope())
                {
                    // retry up to three times
                    var retries = 3;
                    var succeeded = false;
                    while (!succeeded)
                    {
                        try
                        {
                            var rentalTime =
                                (agreement.EndDate.Subtract(
                                    agreement.StartDate));
                            var days = (int) Math.Floor(rentalTime.TotalDays);
                            var pointsPerDay = 1;
                            if (agreement.Vehicle.Size >= Size.Luxury)
                                pointsPerDay = 2;
                            var pts = days * pointsPerDay;
                            _dataService.AddPoints(agreement.Customer.Id, pts);

                            // complete transaction
                            scope.Complete();
                            succeeded = true;
                        }
                        catch
                        {
                            // don't re-throw until the
                            // retry limit is reached
                            if(retries >=0)
                                retries--;
                            else
                                throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Exceptions.Handle(ex))
                    throw;
            }

            // logging
            Console.WriteLine("Accrue complete: {0}", DateTime.Now);
        }
    }
}