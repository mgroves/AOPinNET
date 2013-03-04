using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.Refactor
{
    public interface ILoyaltyRedemptionService
    {
        void Redeem(Invoice invoice, int numberOfDays);
    }
}