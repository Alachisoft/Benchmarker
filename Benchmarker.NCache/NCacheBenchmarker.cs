using Alachisoft.NCache.Client;
using Benchmarker.Framework;

namespace Benchmarker.NCache
{
    public class NCacheBenchmarker : IBenchmarkableCache
    {
        private ICache _cache;

        public void Add(string key, object value)
        {
            _cache.Add(key, value);
        }

        public void Dispose()
        {
            if(_cache != null)
                _cache.Dispose();
        }
        
        public void Get(string key)
        {
            _cache.Get<byte[]>(key);
        }

        public void Init(string cacheName)
        {
            _cache = CacheManager.GetCache(cacheName);
        }
        
        public void Update(string key, object value)
        {
            _cache.Insert(key, value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
