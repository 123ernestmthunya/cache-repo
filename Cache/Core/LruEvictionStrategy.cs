using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Core
{
    public class LruEvictionStrategy : IEvictionStrategy
    {
        private readonly ConcurrentDictionary<string, LinkedListNode<string>> _accessOrder;
        private readonly LinkedList<string> _lruList;
        private readonly object _lock = new object();

        public LruEvictionStrategy()
        {
            _accessOrder = new ConcurrentDictionary<string, LinkedListNode<string>>();
            _lruList = new LinkedList<string>();
        }

        public void Evict(InMemoryCache cache)
        {
            lock (_lock)
            {
                if (_lruList.Count > 0)
                {
                    var keyToEvict = _lruList.First.Value;
                    _lruList.RemoveFirst();
                    _accessOrder.TryRemove(keyToEvict, out _);
                    cache.CacheEntries.TryRemove(keyToEvict, out _);
                }
            }
        }

        public void OnAccess(string key)
        {
            lock (_lock)
            {
                if (_accessOrder.TryGetValue(key, out var node))
                {
                    _lruList.Remove(node);
                    _lruList.AddLast(node);
                }
            }
        }

        public void OnAdd(string key)
        {
            lock (_lock)
            {
                if (_accessOrder.ContainsKey(key))
                {
                    OnAccess(key);
                }
                else
                {
                    var node = _lruList.AddLast(key);
                    _accessOrder[key] = node;
                }
            }
        }
    }
}
