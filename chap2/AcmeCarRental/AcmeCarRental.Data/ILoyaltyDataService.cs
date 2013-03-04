using System;

namespace AcmeCarRental.Data
{
    public interface ILoyaltyDataService
    {
        void AddPoints(Guid customerId, int points);
        void SubtractPoints(Guid customerId, int points);
    }
}