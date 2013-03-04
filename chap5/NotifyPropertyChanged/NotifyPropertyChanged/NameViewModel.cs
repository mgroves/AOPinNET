using System;
using System.ComponentModel;
using System.Reflection;
using PostSharp.Aspects;

namespace NotifyPropertyChanged
{
    public class NameViewModel_PostSharp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedAspect("FullName")]
        public string FirstName { get; set; }
        
        [NotifyPropertyChangedAspect("FullName")]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }

    [Serializable]
    public class NotifyPropertyChangedAspect : LocationInterceptionAspect
    {
        readonly string[] _derivedProperties;

        public NotifyPropertyChangedAspect(params string[] derived)
        {
            _derivedProperties = derived;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            var oldValue = args.GetCurrentValue();
            var newValue = args.Value;
            if (oldValue != newValue)
            {
                args.ProceedSetValue();
                RaisePropertyChanged(args.Instance, args.LocationName);
                if(_derivedProperties != null)
                    foreach (var derivedProperty in _derivedProperties)
                        RaisePropertyChanged(args.Instance, derivedProperty);
            }
        }

        private void RaisePropertyChanged(object instance, string propertyName)
        {
            var type = instance.GetType();
            var propertyChanged = type.GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            var handler = propertyChanged.GetValue(instance) as PropertyChangedEventHandler;
            handler(instance, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class NameViewModel_NotifyPropertyWeaver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }

    public class NameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if(value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("FullName");
                }
            }
        }
        
        string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                    OnPropertyChanged("FullName");
                }
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", _firstName, _lastName);
            }
        }
    }
}
