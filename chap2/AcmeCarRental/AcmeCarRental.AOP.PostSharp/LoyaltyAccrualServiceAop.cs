using System;
using AcmeCarRental.AOP.PostSharp.Aspects;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.AOP.PostSharp
{
    public class LoyaltyAccrualServiceAop : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _dataService;

        public LoyaltyAccrualServiceAop(ILoyaltyDataService service)
        {
            _dataService = service;
        }

        [DefensiveProgramming]
        [ExceptionAspect]
        [LoggingAspect]
        [TransactionManagement]
        public void Accrue(RentalAgreement agreement)
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
        }
    }
}