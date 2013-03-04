using System;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.Refactor
{
    public class LoyaltyAccrualServiceRefactored : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _loyaltyDataService;
        readonly IExceptionHandler _exceptionHandler;
        readonly ITransactionManager _transactionManager;

        public LoyaltyAccrualServiceRefactored(ILoyaltyDataService service, IExceptionHandler exceptionHandler, ITransactionManager transactionManager)
        {
            _loyaltyDataService = service;
            _exceptionHandler = exceptionHandler;
            _transactionManager = transactionManager;
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
            _exceptionHandler.Wrapper(() =>
                {
                    _transactionManager.Wrapper(() =>
                        {
                            var rentalTimeSpan =
                                (agreement.EndDate.Subtract(agreement.StartDate));
                            var numberOfDays = (int) Math.Floor(rentalTimeSpan.TotalDays);
                            var pointsPerDay = 1;
                            if (agreement.Vehicle.Size >= Size.Luxury)
                                pointsPerDay = 2;
                            var points = numberOfDays*pointsPerDay;
                            _loyaltyDataService.AddPoints(agreement.Customer.Id, points);

                            // logging
                            Console.WriteLine("Accrue complete: {0}", DateTime.Now);
                        });
                });
        }
    }
}