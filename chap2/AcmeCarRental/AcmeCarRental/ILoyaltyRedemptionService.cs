using AcmeCarRental.Data.Entities;

namespace AcmeCarRental
{
    public interface ILoyaltyRedemptionService
    {
        void Redeem(Invoice invoice, int numberOfDays);
    }
}