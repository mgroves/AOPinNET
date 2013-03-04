using System;
using System.Transactions;
using PostSharp.Aspects;

namespace AcmeCarRental.AOP.PostSharp.Aspects
{
    [Serializable]
    public class TransactionManagement : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Console.WriteLine("Starting transaction");
            // start new transaction
            using (var scope = new TransactionScope())
            {
                // retry up to three times
                var retries = 3;
                var succeeded = false;
                while (!succeeded)
                {
                    try
                    {
                        args.Proceed();

                        // complete transaction
                        scope.Complete();
                        succeeded = true;
                    }
                    catch
                    {
                        // don't re-throw until the
                        // retry limit is reached
                        if (retries >= 0)
                            retries--;
                        else
                            throw;
                    }
                }
            }
            Console.WriteLine("Transaction complete");
        }
    }
}