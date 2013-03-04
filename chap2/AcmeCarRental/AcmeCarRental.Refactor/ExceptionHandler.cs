using System;
using AcmeCarRental.Data;

namespace AcmeCarRental.Refactor
{
    public interface IExceptionHandler
    {
        void Wrapper(Action method);
    }

    public class ExceptionHandler : IExceptionHandler
    {
        public void Wrapper(Action method)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                if (!Exceptions.Handle(ex))
                    throw;
                throw;
            }
        }
    }
}