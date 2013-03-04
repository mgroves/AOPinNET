using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using StructureMap;

namespace AuthAndCaching.Aspects.PostSharp
{
    [Serializable]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Caching)]
    public class AuthorizedAttribute : OnMethodBoundaryAspect
    {
        [NonSerialized]
        IAuthorizationConcern _authConcern;
        readonly string _role;

        public AuthorizedAttribute(string role)
        {
            _role = role;
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            _authConcern = ObjectFactory.GetInstance<IAuthorizationConcern>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodContext = new PsharpMethodContextAdapter(args);
            _authConcern.OnEntry(methodContext, _role);
        }
    }
}