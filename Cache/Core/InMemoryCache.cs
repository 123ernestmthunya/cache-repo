using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache.Services;

namespace Cache.Core
{
    public class InMemoryCache : ICache
    {
        private readonly ConcurrentDictionary<string, CacheEntry<string>> _cache;
        private readonly int _capacity;
        private readonly IEvictionStrategy _evictionStrategy;

        public ConcurrentDictionary<string, CacheEntry<string>> CacheEntries => _cache;

        public InMemoryCache(int capacity, IEvictionStrategy evictionStrategy)
        {
            _capacity = capacity;
            _evictionStrategy = evictionStrategy;
            _cache = new ConcurrentDictionary<string, CacheEntry<string>>();
        }

        public void Add(string key, string value)
        {
            if (_cache.Count >= _capacity && !_cache.ContainsKey(key))
            {
                _evictionStrategy.Evict(this);
            }

            var cacheEntry = new CacheEntry<string>(value);
            _cache[key] = cacheEntry;
            _evictionStrategy.OnAdd(key);
        }

        public string Get(string key)
        {
            if (_cache.TryGetValue(key, out var cacheEntry))
            {
                cacheEntry.LastAccessed = DateTime.UtcNow;
                _evictionStrategy.OnAccess(key);
                return cacheEntry.Value;
            }

            return null;
        }

        public void Remove(string key)
        {
            _cache.TryRemove(key, out _);
        }

        public int Count => _cache.Count;
    }
}
