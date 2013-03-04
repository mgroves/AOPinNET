using System;
using AcmeCarRental.Data;
using PostSharp.Aspects;

namespace AcmeCarRental.AOP.PostSharp.Aspects
{
    [Serializable]
    public class ExceptionAspect : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (Exceptions.Handle(args.Exception))
                args.FlowBehavior = FlowBehavior.Continue;
        }
    }
}