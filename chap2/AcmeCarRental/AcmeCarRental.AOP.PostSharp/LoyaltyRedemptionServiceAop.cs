using AcmeCarRental.AOP.PostSharp.Aspects;
using AcmeCarRental.Data;
using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.AOP.PostSharp
{
    public class LoyaltyRedemptionServiceAop : ILoyaltyRedemptionService
    {
        readonly ILoyaltyDataService _dataService;

        public LoyaltyRedemptionServiceAop(ILoyaltyDataService service)
        {
            _dataService = service;
        }

        [DefensiveProgramming]
        [ExceptionAspect]
        [LoggingAspect]
        [TransactionManagement]
        public void Redeem(Invoice invoice, int numberOfDays)
        {
            var pointsPerDay = 10;
            if (invoice.Vehicle.Size >= Size.Luxury)
                pointsPerDay = 15;
            var points = numberOfDays*pointsPerDay;
            _dataService.SubtractPoints(invoice.Customer.Id, points);
            invoice.Discount = numberOfDays*invoice.CostPerDay;
        }
    }
}