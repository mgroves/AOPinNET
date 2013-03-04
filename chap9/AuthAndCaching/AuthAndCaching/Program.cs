using System;
using AuthAndCaching.Aspects.DynamicProxy;
using AuthAndCaching.Services;
using StructureMap;

namespace AuthAndCaching
{
    class Program
    {
        static void Main(string[] args)
        {
            // choose between using PostSharp or Castle DynamicProxy by un-commenting the appropriate line
            PostSharpObjectFactoryInitialize();
            //CastleDynamicProxyObjectFactoryInitialize();  // when using this, make sure to comment out the attributes on BudgetService

            var accountNumber = "00112";
            var budgetService = ObjectFactory.GetInstance<IBudgetService>();

            try
            {
                var budget = budgetService.GetBudgetForAccount(accountNumber);
                Console.WriteLine("The budget for account {0} is {1:C}", accountNumber, budget);

                var budgetAgain = budgetService.GetBudgetForAccount(accountNumber);
                Console.WriteLine("The budget for account {0} is {1:C}", accountNumber, budgetAgain);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retrive budget. Error: {0}", ex.Message);
            }
        }

        static void CastleDynamicProxyObjectFactoryInitialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.ForConcreteType<AuthorizedInterceptor>().Configure.Ctor<string>("role").Is("Manager").Named("ManagerAuth");
                var proxyHelper = new ProxyHelper();
                x.For<IBudgetService>()
                 .EnrichAllWith(proxyHelper.Proxify<IBudgetService, CachedInterceptor>)
                 .EnrichAllWith(o => proxyHelper.Proxify<IBudgetService, AuthorizedInterceptor>("ManagerAuth", o));
            });
        }

        static void PostSharpObjectFactoryInitialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                        {
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                        });
            });
        }
    }
}
