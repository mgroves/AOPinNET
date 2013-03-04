using System;
using System.Transactions;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental
{
    public class LoyaltyRedemptionService : ILoyaltyRedemptionService
    {
        readonly ILoyaltyDataService _dataService;

        public LoyaltyRedemptionService(ILoyaltyDataService service)
        {
            _dataService = service;
        }

        public void Redeem(Invoice invoice, int numberOfDays)
        {
            // defensive programming
            if(invoice == null) throw new ArgumentNullException("invoice");
            if(numberOfDays <= 0)
                throw new ArgumentException("","numberOfDays");

            // logging
            Console.WriteLine("Redeem: {0}", DateTime.Now);
            Console.WriteLine("Invoice: {0}", invoice.Id);

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
                            var pointsPerDay = 10;
                            if (invoice.Vehicle.Size >= Size.Luxury)
                                pointsPerDay = 15;
                            var points = numberOfDays*pointsPerDay;
                            _dataService.SubtractPoints(invoice.Customer.Id, points);
                            invoice.Discount = numberOfDays*invoice.CostPerDay;

                            // complete transaction
                            scope.Complete();
                            succeeded = true;

                            // logging
                            Console.WriteLine("Redeem complete: {0}", DateTime.Now);
                        }
                        catch
                        {
                            // don't re-throw until the
                            // retry limit is reached
                            if (retries >= 0)
                                retries--;
                            else
                                throw;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (!Exceptions.Handle(ex))
                    throw;
            }
        }
    }
}