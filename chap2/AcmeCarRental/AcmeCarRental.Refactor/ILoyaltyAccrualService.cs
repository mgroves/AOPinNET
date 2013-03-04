using AcmeCarRental.Data.Entities;

namespace AcmeCarRental.Refactor
{
    public interface ILoyaltyAccrualService
    {
        void Accrue(RentalAgreement agreement);
    }
}