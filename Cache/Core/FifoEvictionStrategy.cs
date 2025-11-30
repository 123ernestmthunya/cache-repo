using System;
using System.Collections.Concurrent;

namespace Cache.Core
{
    public class FifoEvictionStrategy : IEvictionStrategy
    {
        private readonly ConcurrentQueue<string> _fifoQueue;

        public FifoEvictionStrategy()
        {
            _fifoQueue = new ConcurrentQueue<string>();
        }

        public void Evict(InMemoryCache cache)
        {
            if (_fifoQueue.TryDequeue(out var keyToEvict))
            {
                cache.CacheEntries.TryRemove(keyToEvict, out _);
            }
        }

        public void OnAdd(string key)
        {
            _fifoQueue.Enqueue(key);
        }

        public void OnAccess(string key)
        {
            // FIFO doesn't care about access, only insertion order
        }
    }
}
