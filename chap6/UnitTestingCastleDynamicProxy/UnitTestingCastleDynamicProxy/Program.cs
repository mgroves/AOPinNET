using System;
using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace UnitTestingCastleDynamicProxy
{
    class Program
    {
        public class ProxyHelper
        {
            readonly ProxyGenerator _proxyGenerator;

            public ProxyHelper()
            {
                _proxyGenerator = new ProxyGenerator();
            }

            public object Proxify<T, K>(object obj) where K: IInterceptor
            {
                var interceptor = (IInterceptor) ObjectFactory.GetInstance<K>();
                var result = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                    typeof (T), obj, interceptor);
                return result;
            }
        }

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
                    x.For<IMyService>().Use<MyService>().EnrichWith(proxyHelper.Proxify<IMyService, MyLoggingAspect>);
                });

            var myObj = ObjectFactory.GetInstance<IMyService>();
            myObj.DoSomething();
        }
    }

    public interface IMyService
    {
        void DoSomething();
    }

    public class MyService : IMyService
    {
        public MyService(IMyOtherService other)
        {
        }

        public void DoSomething()
        {
            Console.WriteLine("Something was done");
        }
    }

    public interface IMyOtherService
    {
        void DoOtherThing();
    }

    public class MyOtherService : IMyOtherService
    {
        public void DoOtherThing()
        {
            // this is just to demonstrate complex dependencies
        }
    }

    public class MyLoggingAspect : IInterceptor
    {
        readonly ILoggingService _loggingService;

        public MyLoggingAspect(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Intercept(IInvocation invocation)
        {
            _loggingService.Write("Log start");
            invocation.Proceed();
            _loggingService.Write("Log end");
        }
    }

    public interface ILoggingService
    {
        void Write(string message);
    }

    public class LoggingService : ILoggingService
    {
        public void Write(string message)
        {
            Console.WriteLine("Logging: " + message);
        }
    }

    [TestFixture]
    public class MyLoggingAspectTest
    {
        [Test]
        public void TestIntercept()
        {
            var mockLoggingService = new Mock<ILoggingService>();
            var loggingAspect = new MyLoggingAspect(mockLoggingService.Object);
            var mockInvocation = new Mock<IInvocation>();

            loggingAspect.Intercept(mockInvocation.Object);

            mockLoggingService.Verify(x => x.Write("Log start"));
            mockLoggingService.Verify(x => x.Write("Log end"));
        }
    }
}
