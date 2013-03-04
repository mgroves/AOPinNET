using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using StructureMap;

namespace AuthAndCaching.Aspects.PostSharp
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Caching)]
    public class CachedAttribute : OnMethodBoundaryAspect
    {
        [NonSerialized]
        ICachingConcern _cacheConcern;

        public override void RuntimeInitialize(MethodBase method)
        {
            _cacheConcern = ObjectFactory.GetInstance<ICachingConcern>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            IMethodContextAdapter methodContext = new PsharpMethodContextAdapter(args);
            _cacheConcern.OnEntry(methodContext);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            IMethodContextAdapter methodContext = new PsharpMethodContextAdapter(args);
            _cacheConcern.OnSuccess(methodContext);
        }
    }
}