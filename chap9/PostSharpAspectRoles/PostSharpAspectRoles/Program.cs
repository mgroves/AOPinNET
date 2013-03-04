using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace PostSharpAspectRoles
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
        [Authorization]
        public void MyMethod()
        {
        }
    }


    [Serializable]
    [AspectRoleDependency(AspectDependencyAction.Require, StandardRoles.Tracing)]
    public class AuthorizationAttribute : OnMethodBoundaryAspect
    {
    }

    [Serializable]
    [ProvideAspectRole(StandardRoles.Tracing)]
    public class TracingAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
        }
    }
}
