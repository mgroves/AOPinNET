using System;

namespace AcmeCarRental.Refactor
{
    public interface ITransactionFacade
    {
        void Wrapper(Action action);
    }

    public class TransactionFacade : ITransactionFacade
    {
        readonly IExceptionHandler _exceptionHandler;
        readonly ITransactionManager _transactionManager;

        public TransactionFacade(IExceptionHandler exceptionHandler, ITransactionManager transactionManager)
        {
            _exceptionHandler = exceptionHandler;
            _transactionManager = transactionManager;
        }

        public void Wrapper(Action action)
        {
            _exceptionHandler.Wrapper(() =>
            {
                _transactionManager.Wrapper(() =>
                {
                    action();
                });
            });

            // or more concisely:
            //_exceptionHandler.Wrapper(() => _transactionManager.Wrapper(action));
        }
    }
}