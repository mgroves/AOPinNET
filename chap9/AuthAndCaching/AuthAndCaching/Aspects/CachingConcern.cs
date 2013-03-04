using System;
using AuthAndCaching.Services;

namespace AuthAndCaching.Aspects
{
    public interface ICachingConcern
    {
        void OnEntry(IMethodContextAdapter methodContext);
        void OnSuccess(IMethodContextAdapter methodContext);
    }

    public class CachingConcern : ICachingConcern
    {
        readonly ICacheService _cache;

        public CachingConcern(ICacheService cache)
        {
            _cache = cache;
        }

        public void OnEntry(IMethodContextAdapter methodContext)
        {
            var cacheKey = BuildCacheKey(methodContext);
            if (!_cache.ContainsKey(cacheKey))
            {
                Console.WriteLine("[Cache] MISS for {0}", cacheKey);
                methodContext.Tag = cacheKey;
                return;
            }
            Console.WriteLine("[Cache] HIT for {0}", cacheKey);
            methodContext.ReturnValue = _cache[cacheKey];
            methodContext.AbortMethod();
        }

        public void OnSuccess(IMethodContextAdapter methodContext)
        {
            var cacheKey = (string)methodContext.Tag;
            Console.WriteLine("[Cache] storing value for {0}", cacheKey);
            _cache[cacheKey] = methodContext.ReturnValue;
        }

        string BuildCacheKey(IMethodContextAdapter methodContext)
        {
            var key = methodContext.MethodName;
            foreach (var argument in methodContext.Arguments)
                key += "_" + argument.ToString();
            return key;
        }
    }
}