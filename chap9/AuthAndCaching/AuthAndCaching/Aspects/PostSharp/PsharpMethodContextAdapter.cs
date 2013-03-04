using PostSharp.Aspects;

namespace AuthAndCaching.Aspects.PostSharp
{
    public class PsharpMethodContextAdapter : IMethodContextAdapter
    {
        readonly MethodExecutionArgs _args;

        public PsharpMethodContextAdapter(MethodExecutionArgs args)
        {
            _args = args;
        }

        public string MethodName { get { return _args.Method.Name; } }

        public object Tag
        {
            get { return _args.MethodExecutionTag; }
            set { _args.MethodExecutionTag = value; }
        }

        public object[] Arguments
        {
            get { return _args.Arguments.ToArray(); }
        }

        public object ReturnValue
        {
            get { return _args.ReturnValue; }
            set { _args.ReturnValue = value; }
        }

        public void AbortMethod()
        {
            _args.FlowBehavior = FlowBehavior.Return;
        }
    }
}