using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;

namespace UnitTestingCastleDynamicProxy
{
    public static class Log
    {
        public static List<string> _messages = new List<string>();
        public static List<string> Messages
        {
            get { return _messages; }
        }

        public static void Write(string message)
        {
            _messages.Add(message);
        }
    }

    public class MyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Log.Write("Before " + invocation.Method.Name);
            invocation.Proceed();
            Log.Write("After " + invocation.Method.Name);
        }
    }

    [TestFixture]
    public class MyInterceptorTest
    {
        [Test]
        public void TestIntercept()
        {
            var myInterceptor = new MyInterceptor();

            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.Method.Name).Returns("MyMethod");
            var invocation = invocationMock.Object;

            myInterceptor.Intercept(invocation);

            Assert.IsTrue(Log.Messages.Contains("Before " + invocation.Method.Name));
            Assert.IsTrue(Log.Messages.Contains("After " + invocation.Method.Name));
        }
    }
}