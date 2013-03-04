using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernateExample
{
    class Program
    {
        static ISession _sess;

        static void Main(string[] args)
        {
            _sess = GetNHibernateSession();

            var book = new Entities.Book
            {
                Name = "Aspect-Oriented Programming in .NET",
                Price = 49.99M,
                Publisher = "Manning"
            };

            _sess.Save(book);
            _sess.Flush();

            ListAllBookNames();
        }

        static ISession GetNHibernateSession()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Entities.Book).Assembly);

            var schema = new SchemaExport(cfg);
            var writeDbScriptToConsole = false;
            var createDatabaseFileAndSchemaIfNecessary = true;
            schema.Create(writeDbScriptToConsole, createDatabaseFileAndSchemaIfNecessary);

            var sessions = cfg.BuildSessionFactory();
            return sessions.OpenSession();
        }

        static void ListAllBookNames()
        {
            var nhQuery = _sess.CreateQuery("FROM Book");
            var list = nhQuery.List<Entities.Book>();
            foreach (var book in list)
                Console.WriteLine(book.Name);
        }
    }
}
