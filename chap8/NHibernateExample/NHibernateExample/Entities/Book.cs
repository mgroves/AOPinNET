using System;

namespace NHibernateExample.Entities
{
    public class Book
    {
        public virtual Guid Id { get; set; }
        public string Name { get; set; }
        public virtual string Publisher { get; set; }
        public virtual decimal Price { get; set; }         
    }
}