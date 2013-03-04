using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace MulticastingExample
{
    [LogAspect(AspectPriority = 2, AttributeTargetElements = MulticastTargets.InstanceConstructor)]
    [AnotherAspect(AspectPriority = 1)]
    public class MyClass
    {
        public MyClass() { }
        public MyClass(int x) { }
        public void Method1() { }
        public void Method2()
        {
            Method3();
        }
        //[LogAspect(AttributeExclude = true)]
        private void Method3() { }
    }

    [Serializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Aspect was applied to {0}", args.Method.Name);
        }
    }

    [Serializable]
    public class AnotherAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Another Aspect was applied to {0}", args.Method.Name);
        }
    }
}