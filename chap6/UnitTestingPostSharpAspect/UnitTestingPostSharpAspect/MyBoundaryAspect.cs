using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using NUnit.Framework;
using PostSharp.Aspects;

namespace UnitTestingPostSharpAspect
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

    [Serializable]
    public class MyBoundaryAspect : OnMethodBoundaryAspect {
        public override void OnEntry(MethodExecutionArgs args) {
            Log.Write("Before: " + args.Method.Name);
        }
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Log.Write("After: " + args.Method.Name);
        }
    }

    [TestFixture]
    public class TestMyLoggerCrossCuttingConcern {
        [Test]
        public void TestMyBoundaryAspect() {
            var args = new MethodExecutionArgs(null, Arguments.Empty);
            args.Method = new DynamicMethod("FooBar",null,null);

            var aspect = new MyBoundaryAspect();
            aspect.OnEntry(args);
            aspect.OnSuccess(args);

            Assert.IsTrue(Log.Messages.Contains("Before: " + args.Method.Name));
            Assert.IsTrue(Log.Messages.Contains("After: " + args.Method.Name));
        }
    }
}