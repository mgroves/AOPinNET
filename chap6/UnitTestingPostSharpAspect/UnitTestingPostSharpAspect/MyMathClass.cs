using System;
using System.Reflection;
using NUnit.Framework;
using PostSharp.Aspects;

namespace UnitTestingPostSharpAspect
{
    public class MyMathClass {
        [MyLoggerAspect]
        public int Add(int x, int y) {
            return x + y;
        }
    }

    [Serializable]
    public class MyLoggerAspect : OnMethodBoundaryAspect {
        [NonSerialized] LoggingCrossCuttingConcern logging;

        public override void RuntimeInitialize(MethodBase method) {
            logging = new LoggingCrossCuttingConcern();
        }

        public override void OnEntry(MethodExecutionArgs args) {
            logging.OnEntry(args);
        }
    }

    public class LoggingCrossCuttingConcern {
        public void OnEntry(MethodExecutionArgs args) {
            Console.WriteLine("Enter method!");
        }
    }

    [TestFixture]
    public class TestMyMathClass {
        [Test]
        public void TestAdd() {
            var math = new MyMathClass();
            var sum = math.Add(2, 2);
            Assert.That(sum, Is.EqualTo(4));
        }
    }

    [TestFixture]
    public class TestMyLoggerCrossCuttingConcern {
        [Test]
        public void TestOnEntry() {
            var args = TypeMock.GetMock<MethodExecutionArgs>();  // I haven't used TypeMock I don't know the syntax
            // ...arrange args...

            var ccc = new LoggingCrossCuttingConcern();
            ccc.OnEntry(args);
        }
    }
}