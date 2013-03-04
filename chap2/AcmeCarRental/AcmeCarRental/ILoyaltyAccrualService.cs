using AcmeCarRental.Data.Entities;

namespace AcmeCarRental
{
    public interface ILoyaltyAccrualService
    {
        void Accrue(RentalAgreement agreement);
    }
}