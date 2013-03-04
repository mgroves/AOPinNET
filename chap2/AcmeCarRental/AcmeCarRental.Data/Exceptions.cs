using System;

namespace AcmeCarRental.Data
{
    public static class Exceptions
    {
        public static bool Handle(Exception ex)
        {
            // this code could check to see if it's a type
            // of exception that can be handled
            // or maybe it can notify the user
            // or system admins
            // etc

            if (ex.GetType() == typeof(ArithmeticException))
                return false;
            if (ex.GetType() == typeof(TimeoutException))
                return true;

            // etc...
            return false;
        }
    }
}