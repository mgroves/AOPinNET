using System;
using Castle.DynamicProxy;

namespace TweetDynamicProxy
{
    public class TwitterClient
    {
        public virtual void Send(string msg)
        {
            Console.WriteLine("Sending: {0}", msg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var proxyGenerator = new ProxyGenerator();
            var svc = proxyGenerator
                .CreateClassProxy<TwitterClient>(
                    new MyInterceptorAspect());
            svc.Send("hi");
        }
    }

    public class MyInterceptorAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Interceptor 1");
            invocation.Proceed();
            Console.WriteLine("Interceptor 2");
        }
    }
}
