using System;
using PostSharp.Aspects;

namespace TweetPostSharp
{
    public class TwitterClient
    {
        [MyInterceptorAspect]
        public void Send(string msg)
        {
            Console.WriteLine("Sending: {0}", msg);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new TwitterClient();
            svc.Send("hi");
        }
    }

    [Serializable]
    public class MyInterceptorAspect : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Console.WriteLine("Interceptor 1");
            args.Proceed();
            Console.WriteLine("Interceptor 2");
        }
    }
}
