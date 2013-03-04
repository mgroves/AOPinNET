using System;
using System.Transactions;
using AcmeCarRental.Data;

namespace AcmeCarRental.Refactor
{
    public interface ITransactionManager2
    {
        void Wrapper(Action method);
    }

    public class TransactionManager2 : ITransactionManager2
    {
        public void Wrapper(Action method)
        {
            using (var scope = new TransactionScope())
            {
                var retries = 3;
                var succeeded = false;
                while (!succeeded)
                {
                    try
                    {
                        method();
                        scope.Complete();
                        succeeded = true;
                    }
                    catch (Exception ex)
                    {
                        if (retries >= 0)
                            retries--;
                        else
                        {
                            if (!Exceptions.Handle(ex))
                                throw;
                        }
                    }
                }
            }
        }
    }
}