using System.Collections.Generic;

namespace AuthAndCaching.Services
{
    public interface ICacheService
    {
        object this[string cacheKey] { get; set; }
        bool ContainsKey(string cacheKey);
    }

    public class CacheService : ICacheService
    {
        static readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public object this[string cacheKey]
        {
            get { return _cache[cacheKey]; }
            set { _cache[cacheKey] = value; }
        }

        public bool ContainsKey(string cacheKey)
        {
            return _cache.ContainsKey(cacheKey);
        }
    }
}