using System;
using AuthAndCaching.Services;
using Castle.DynamicProxy;
using StructureMap;

namespace AuthAndCaching.Aspects.DynamicProxy
{
    public class ProxyHelper
    {
        readonly ProxyGenerator _proxyGenerator;

        public ProxyHelper()
        {
            _proxyGenerator = new ProxyGenerator();
        }

        public object Proxify<T, K>(object obj) where K : IInterceptor
        {
            return Proxify<K>(typeof(T), obj);
        }

        public object Proxify<K>(Type t, object obj) where K : IInterceptor
        {
            var interceptor = (IInterceptor)ObjectFactory.GetInstance<K>();
            var result = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                t, obj, interceptor);
            return result;
        }

        public object Proxify<T>(IInterceptor interceptor, object obj)
        {
            var result = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                typeof(T), obj, interceptor);
            return result;            
        }

        public object Proxify<T, K>(string name, object obj) where K : IInterceptor
        {
            var interceptor = (IInterceptor)ObjectFactory.GetNamedInstance<K>(name);
            var result = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                typeof(T), obj, interceptor);
            return result;
        }
    }
}