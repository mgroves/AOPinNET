using System;
using System.Linq;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Interceptors;

namespace CompositionDynamicProxyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                            {
                                scan.TheCallingAssembly();
                                scan.WithDefaultConventions();
                            });
                var proxyHelper = new ProxyHelper();
                x.For<IMyClass>().EnrichAllWith(r =>
                    proxyHelper.Proxify<IMyClass, Aspect1>(
                    proxyHelper.Proxify<IMyClass, Aspect2>(r))
                );
            });

            var obj = ObjectFactory.GetInstance<IMyClass>();
            obj.MyMethod();
        }
    }

    public interface IMyClass
    {
        void MyMethod();
    }

    public class MyClass : IMyClass
    {
        public void MyMethod()
        {
            Console.WriteLine("My Method");
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
