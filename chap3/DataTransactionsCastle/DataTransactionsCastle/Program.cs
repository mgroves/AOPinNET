using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Castle.DynamicProxy;

namespace DataTransactionsCastle
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyGenerator = new ProxyGenerator();
            var invoiceService = proxyGenerator
                .CreateClassProxy<InvoiceService>(
                    new TransactionWithRetries(3));

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                InvoiceDate = DateTime.Now,
                Items = new List<string> {
                    "Item1","Item2","Item3"
                }
            };

            invoiceService.Save(invoice);
            //invoiceService.SaveRetry(invoice);
            //invoiceService.SaveFail(invoice);
            Console.WriteLine("Save successful");
        }
    }

    public class TransactionWithRetries : IInterceptor
    {
        readonly int _maxRetries;

        public TransactionWithRetries(int maxRetries)
        {
            _maxRetries = maxRetries;
        }

        public void Intercept(IInvocation invocation)
        {
            var retries = _maxRetries;
            var succeeded = false;    
            while (!succeeded)
            {
                using (var trans = new TransactionScope())
                {
                    try
                    {
                        invocation.Proceed();
                        trans.Complete();
                        succeeded = true;
                    }
                    catch(Exception)
                    {
                        if (retries >= 0)
                        {
                            Console.WriteLine("Retrying {0}", invocation.Method.Name);
                            retries--;
                        }
                        else
                            throw;
                    }
                }
            }
        }
    }
    
    public class InvoiceService
    {
        public virtual void Save(Invoice invoice)
        {
            // always succeeds
        }

        bool _isRetry;
        public virtual void SaveRetry(Invoice invoice)
        {
            // fails first time
            if(!_isRetry)
            {
                _isRetry = true;
                throw new DataException();
            }
        }

        public virtual void SaveFail(Invoice invoice)
        {
            // always fails
            throw new DataException();
        }
    }

    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<string> Items { get; set; }
    }
}
