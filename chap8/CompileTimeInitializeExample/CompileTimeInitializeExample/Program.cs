using System;
using System.Reflection;
using PostSharp.Aspects;

namespace CompileTimeInitializeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new MyClass();
            obj.MyMethod();
        }
    }

    public class MyClass
    {
        [MyLoggingAspect]
        public void MyMethod()
        {
            Console.WriteLine("Code in MyMethod");
        }
    }

//    [Serializable]
//    public class MyLoggingAspect : OnMethodBoundaryAspect
//    {
//        public override void OnEntry(MethodExecutionArgs args)
//        {
//            Console.WriteLine("Method was called: {0}", args.Method.Name);
//        }
//    }

    [Serializable]
    public class MyLoggingAspect : OnMethodBoundaryAspect
    {
        string _methodName;

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _methodName = method.Name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Method was called: {0}", _methodName);
        }
    }
}
