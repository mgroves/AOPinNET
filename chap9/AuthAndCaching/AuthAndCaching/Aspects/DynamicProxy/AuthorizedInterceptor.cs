using Castle.DynamicProxy;

namespace AuthAndCaching.Aspects.DynamicProxy
{
    public class AuthorizedInterceptor : IInterceptor
    {
        readonly IAuthorizationConcern _authConcern;
        readonly string _role;

        public AuthorizedInterceptor(IAuthorizationConcern authConcern, string role)
        {
            _authConcern = authConcern;
            _role = role;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodContext = new CdpMethodContextAdapter(invocation);
            _authConcern.OnEntry(methodContext, _role);
            if (methodContext.Proceed)
                invocation.Proceed();
        }
    }
}