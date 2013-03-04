using System;
using System.Linq;
using System.Reflection;
using PostSharp.Constraints;
using PostSharp.Extensibility;

namespace NHibernateExample
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Class)]
    public class NHEntityAttribute : ScalarConstraint
    {
        public override void ValidateCode(object target)
        {
            var targetType = (Type)target;
            var properties = targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.GetGetMethod().IsVirtual);
            foreach (var propertyInfo in properties)
            {
                Message.Write(propertyInfo,
                        SeverityType.Error,
                        "NHVIRTUAL001",
                        "Property '{0}' in NH Entity class '{1}' is not virtual",
                        propertyInfo.Name, targetType.FullName);
            }
        }
    }
}