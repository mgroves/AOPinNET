using System;

namespace ConventionDynamicProxyExample.Repositories
{
    public class MyClassRepository : IMyClassRepository
    {
        public void MyMethod()
        {
            Console.WriteLine("My Method");
        }
    }
}