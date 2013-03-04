using System;
using System.Linq;
using NUnit.Framework;
using PostSharp.Aspects;
using System.Collections.Generic;
using StructureMap;

namespace UnitTestingPostSharpAspect
{
    public class MyStringClass
    {
        [LogAspect]
        public string Reverse(string str)
        {
            if (str == null)
                return null;
            return new string(str.Reverse().ToArray());
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class MyLogger : ILogger
    {
        readonly List<string> _messages;
        public IEnumerable<string> Messages { get { return _messages; } }

        public MyLogger()
        {
            _messages = new List<string>();
        }

        public void Log(string message)
        {
            _messages.Add(message);
        }
    }

    [Serializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        ILogger _log;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            _log = ObjectFactory.GetInstance<ILogger>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _log.Log("Entering " + args.Method.Name);
        }
    }

    [TestFixture]
    public class MyTests1
    {
        [Test]
        public void WhenICallReverseTheLogAspectIsApplies()
        {
            var logger = new MyLogger();
            ObjectFactory.Initialize(x => x.For<ILogger>().Use(logger));

            var obj = new MyStringClass();
            var reverseHello = obj.Reverse("hello");

            Assert.That(reverseHello, Is.EqualTo("olleh"));
            Assert.That(logger.Messages.Count(), Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class MyStringClassTests
    {
        [Test]
        public void TestReverse()
        {
            var myStringObject = new MyStringClass();
            var reversedString = myStringObject.Reverse("hello");
            Assert.That(reversedString, Is.EqualTo("olleh"));
        }

        [Test]
        public void TestReverseWithNull()
        {
            var myStringObject = new MyStringClass();
            var reversedString = myStringObject.Reverse(null);
            Assert.That(reversedString, Is.Null);
        }
    }
}