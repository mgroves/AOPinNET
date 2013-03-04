using System;
using AcmeCarRental.Data.Entities;
using PostSharp.Aspects;

namespace AcmeCarRental.AOP.PostSharp.Aspects
{
    [Serializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("{0}: {1}", args.Method.Name, DateTime.Now);

            foreach (var argument in args.Arguments)
                //if(argument != null)
                    if (typeof(ILoggable).IsAssignableFrom(argument.GetType()))
                        Console.WriteLine(((ILoggable)argument).LogInformation());
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine("{0} complete: {1}", args.Method.Name, DateTime.Now);
        }
    }
}