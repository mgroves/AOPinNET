using Castle.DynamicProxy;

namespace AuthAndCaching.Aspects.DynamicProxy
{
    public class CdpMethodContextAdapter : IMethodContextAdapter
    {
        readonly IInvocation _invocation;

        public CdpMethodContextAdapter(IInvocation invocation)
        {
            _invocation = invocation;
            Proceed = true;
        }

        // Proceed is not a member of the IMethodContextAdapter interface
        // its a unique detail for this implementation
        public bool Proceed { get; private set; }

        public object Tag { get; set; }
        public object ReturnValue
        {
            get { return _invocation.ReturnValue; } 
            set { _invocation.ReturnValue = value; }
        }

        public string MethodName
        {
            get { return _invocation.Method.Name; }
        }

        public object[] Arguments
        {
            get { return _invocation.Arguments; }
        }

        public void AbortMethod()
        {
            Proceed = false;
        }
    }
}