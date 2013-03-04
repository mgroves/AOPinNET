using System;

namespace ProxyPatternReview
{
    class Program
    {
        static void Main(string[] args)
        {
            IBusinessModule module = new BusinessModule();
            module.Method1();

            Console.WriteLine("---------------");

            module = new BusinessModuleProxy();
            module.Method1();
        }
    }

    public interface IBusinessModule
    {
        void Method1();
    }

    public class BusinessModule : IBusinessModule
    {
        public void Method1()
        {
            Console.WriteLine("Method1");
        }
    }

    public class BusinessModuleProxy : IBusinessModule
    {
        BusinessModule _realObject;

        public BusinessModuleProxy()
        {
            _realObject = new BusinessModule();
        }

        public void Method1()
        {
            Console.WriteLine("BusinessModuleProxy before");
            _realObject.Method1();
            Console.WriteLine("BusinessModuleProxy after");
        }
    }
}
