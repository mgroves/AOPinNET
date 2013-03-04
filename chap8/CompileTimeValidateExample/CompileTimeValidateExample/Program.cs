using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace CompileTimeValidateExample
{
    class Program
    {
        [MyAspect]
        public static string MyName { get; set; }

        static void Main(string[] args)
        {
            MyName = "Matthew D. Groves";
            Console.WriteLine(MyName);
        }
    }

    [Serializable]
    public class MyAspect : LocationInterceptionAspect
    {
        public override bool CompileTimeValidate(LocationInfo locationInfo)
        {
            if (locationInfo.Name != "Horse")
            {
                Message.Write(locationInfo, SeverityType.Error, "MYERRORCODE01", "Location name must be 'horse'");
                return false;
            }
            return true;
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            Console.WriteLine("Property 'getter' was used");
            args.ProceedGetValue();
        }
    }
}
