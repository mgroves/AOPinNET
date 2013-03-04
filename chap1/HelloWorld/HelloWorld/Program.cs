using System;
using PostSharp.Aspects;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var myObject = new MyClass();
            myObject.MyMethod();
        }
    }

    public class MyClass
    {
        [MyAspect]
        public void MyMethod()
        {
            Console.WriteLine("Hello, world!");
        }
    }

    [Serializable]
    public class MyAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Before the method");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("After the method");
        }
    }

}
