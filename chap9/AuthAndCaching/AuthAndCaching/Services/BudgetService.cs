using System;
using AuthAndCaching.Aspects.PostSharp;

namespace AuthAndCaching.Services
{
    public interface IBudgetService
    {
        decimal GetBudgetForAccount(string accountNumber);
    }

    public class BudgetService : IBudgetService
    {
        [Cached]
        [Authorized("Manager")]
        public decimal GetBudgetForAccount(string accountNumber)
        {
            var rand = new Random();
            return rand.Next(1000, 5000);
        }
    }
}