using System;
using Moq;
using NUnit.Framework;
using PostSharp.Aspects;
using StructureMap;

namespace UnitTestingPostSharpAspect
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
        public MyService(IMyOtherService other) { }

        [MyLoggingAspect]
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

    [Serializable]
    public class MyLoggingAspect : OnMethodBoundaryAspect
    {
        [NonSerialized] ILoggingService _loggingService;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            _loggingService = ObjectFactory.GetInstance<ILoggingService>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _loggingService.Write("Log start");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
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
            var args = new MethodExecutionArgs(null, Arguments.Empty);

            ObjectFactory.Initialize(x => x.For<ILoggingService>().Use(mockLoggingService.Object));
            var loggingAspect = new MyLoggingAspect();
            loggingAspect.RuntimeInitialize(null);

            loggingAspect.OnEntry(args);
            loggingAspect.OnSuccess(args);

            mockLoggingService.Verify(x => x.Write("Log start"));
            mockLoggingService.Verify(x => x.Write("Log end"));
        }
    }
}
