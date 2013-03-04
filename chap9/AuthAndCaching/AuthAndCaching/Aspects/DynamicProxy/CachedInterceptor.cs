using System;
using Castle.DynamicProxy;

namespace AuthAndCaching.Aspects.DynamicProxy
{
    public class CachedInterceptor : IInterceptor
    {
        readonly ICachingConcern _cacheConcern;

        public CachedInterceptor(ICachingConcern cacheConcern)
        {
            _cacheConcern = cacheConcern;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodContext = new CdpMethodContextAdapter(invocation);
            _cacheConcern.OnEntry(methodContext);
            if (!methodContext.Proceed)
                return;
            invocation.Proceed();
            _cacheConcern.OnSuccess(methodContext);
        }
    }
}