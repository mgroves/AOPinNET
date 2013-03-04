using Castle.DynamicProxy;
using StructureMap;

namespace CompositionDynamicProxyExample
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
            var interceptor = (IInterceptor)ObjectFactory.GetInstance<K>();
            var result = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                typeof(T), obj, interceptor);
            return result;
        }
    }
}