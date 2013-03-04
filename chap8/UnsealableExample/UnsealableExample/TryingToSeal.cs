using System;
using System.Linq;
using System.Reflection;
using PostSharp.Constraints;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace UnsealableExample
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Class)]
    public class Unsealable : ReferentialConstraint
    {
        public override void ValidateCode(object target, Assembly assembly)
        {
            var targetType = (Type)target;

            // using standard Reflection
            var sealedSubClasses = assembly.GetTypes()
                    .Where(t => t.IsSealed)
                    .Where(t => targetType.IsAssignableFrom(t))
                    .ToList();

            // using PostSharp Reflection helper API
//            var sealedSubClasses = ReflectionSearch.GetDerivedTypes(targetType)
//                                        .Where(t => t.DerivedType.IsSealed)
//                                        .Select(t => t.DerivedType)
//                                        .ToList();

            sealedSubClasses.ForEach(c =>
                Message.Write(c, SeverityType.Error, 
                "UNSEAL001", 
                "Error on {0}: subclasses of {1} cannot be sealed.",
                c.FullName,
                targetType.FullName));
        }
    }

    [Unsealable]
    public class MyUnsealableClass
    {
        protected string _value;

        public MyUnsealableClass()
        {
            _value = "I'm unsealable!";
        }

        public string GetValue()
        {
            return _value;
        }
    }

    public sealed class TryingToSeal : MyUnsealableClass
    {
        public TryingToSeal()
        {
            _value = "I'm sealed!";
        }
    }
}
