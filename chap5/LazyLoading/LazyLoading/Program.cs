using System;
using System.Threading;
using PostSharp.Aspects;
using StructureMap;

namespace LazyLoading
{
    public class SlowConstructor
    {
        IMyService _myService;

        public SlowConstructor(IMyService myService)
        {
            _myService = myService;
            Console.WriteLine("Doing something slow");
            Thread.Sleep(5000);
        }

        public void DoSomething()
        {
            _myService.DoSomething();
        }
    }
    
    class Program
    {
        [LazyLoadGetter]
        static SlowConstructor MyProperty
        {
            get { return new SlowConstructor(new MyService());}
        }

        [LazyLoadStructureMap]
        static SlowConstructor MyField;  

        static void Main(string[] args)
        {
            MyProperty.DoSomething();
            MyProperty.DoSomething();

            ObjectFactory.Initialize(x =>
                {
                    x.For<IMyService>().Use<MyService>();
                    x.For<SlowConstructor>();
                });

            MyField.DoSomething();
            MyField.DoSomething();
        }
    }

    [Serializable]
    public class LazyLoadStructureMap : LocationInterceptionAspect
    {
        volatile object _backingField;
        object _syncRoot = new object();

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (_backingField == null)
                lock (_syncRoot)
                    if (_backingField == null)
                    {
                        var locationType = args.Location.PropertyInfo.PropertyType;
                        _backingField = ObjectFactory.GetInstance(locationType);
                    }
            args.Value = _backingField;
        }
    }

    [Serializable]
    public class LazyLoadActivator : LocationInterceptionAspect
    {
        volatile object _backingField;
        object _syncRoot = new object();

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (_backingField == null)
                lock (_syncRoot)
                    if (_backingField == null)
                        _backingField = Activator.CreateInstance(args.Location.LocationType);
            args.Value = _backingField;
        }
    }

    [Serializable]
    public class LazyLoadGetter : LocationInterceptionAspect
    {
        volatile object _backingField;
        object _syncRoot = new object();

        public override void OnGetValue(LocationInterceptionArgs args)
        {
             if(_backingField == null)
                lock(_syncRoot)
                    if(_backingField == null)
                    {
                        args.ProceedGetValue();
                        _backingField = args.Value;
                    }
            args.Value = _backingField;
        }
    }

    public interface IMyService
    {
        void DoSomething();
    }

    public class MyService : IMyService
    {
        public void DoSomething()
        {
            Console.WriteLine("Hello, it's {0}", DateTime.Now);
        }
    }
}
