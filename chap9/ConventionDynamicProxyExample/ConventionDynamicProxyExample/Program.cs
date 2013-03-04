using System;
using Castle.DynamicProxy;
using ConventionDynamicProxyExample.Repositories;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace ConventionDynamicProxyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory.Initialize(x =>
            {
                var proxyHelper = new ProxyHelper();
                x.For<ProxyHelper>().Singleton().Use(proxyHelper);
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.Convention<RepositoryAspectConvention>();
                });
            });

            var obj = ObjectFactory.GetInstance<IMyClassRepository>();
            obj.MyMethod();
            
            var anotherObj = ObjectFactory.GetInstance<IAnotherRepository>();
            anotherObj.AnotherMethod();
        }
    }

    public class RepositoryAspectConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!type.IsInterface)
                return;
    
            var proxyHelper = ObjectFactory.GetInstance<ProxyHelper>();
            //if (type.Name.EndsWith("Repository"))
            if (type.Namespace == "ConventionDynamicProxyExample.Repositories")
                registry.For(type)
                        .EnrichWith(o => proxyHelper.Proxify<Aspect2>(type, o))
                        .EnrichWith(o => proxyHelper.Proxify<Aspect1>(type, o));
        }
    }

    public class Aspect1 : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Aspect 1");
            invocation.Proceed();
        }
    }

    public class Aspect2 : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Aspect 2");
            invocation.Proceed();
        }
    }
}
