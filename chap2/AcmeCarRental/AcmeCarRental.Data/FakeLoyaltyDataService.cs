using System;

namespace AcmeCarRental.Data
{
    public class FakeLoyaltyDataService : ILoyaltyDataService
    {
        public void AddPoints(Guid customerId, int points)
        {
            Console.WriteLine("Adding {0} points for customer '{1}'"
                , points, customerId);
        }

        public void SubtractPoints(Guid customerId, int points)
        {
            Console.WriteLine("Subtracting {0} points for customer '{1}'"
                , points, customerId);
        }
    }
}