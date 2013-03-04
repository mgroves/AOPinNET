using System;
using PostSharp.Aspects;

namespace AcmeCarRental.AOP.PostSharp.Aspects
{
    [Serializable]
    public class DefensiveProgramming : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            var parameters = args.Method.GetParameters();
            var arguments = args.Arguments;
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] == null)
                    throw new ArgumentNullException(parameters[i].Name);
                if (arguments[i].GetType() == typeof(int) && (int)arguments[i] <= 0)
                    throw new ArgumentException("", parameters[i].Name);
            }
        }
    }
}