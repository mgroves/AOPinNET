namespace MulticastingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // multicasting at class level
            var m = new MyClass();
            m.Method1();
            m.Method2();
        }
    }
}
