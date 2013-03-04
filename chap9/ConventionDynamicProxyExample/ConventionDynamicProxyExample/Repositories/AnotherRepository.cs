using System;

namespace ConventionDynamicProxyExample.Repositories
{
    public class AnotherRepository : IAnotherRepository
    {
        public void AnotherMethod()
        {
            Console.WriteLine("Method in another repository");
        }
    }
}