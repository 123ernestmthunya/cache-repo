using Cache.Core;
using NUnit.Framework;

namespace Cache.Test
{
    [TestFixture]
    public class EvictionStrategyTests
    {
        [Test]
        public void LruEvictionStrategy_ShouldEvictLeastRecentlyUsed()
        {
            // Arrange
            var strategy = new LruEvictionStrategy();
            var cache = new InMemoryCache(3, strategy);
            
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");
            
            // Access key1 to make it recently used
            cache.Get("key1");
            cache.Get("key3");

            // Act - Evict should remove key2 (least recently used)
            strategy.Evict(cache);

            // Assert
            Assert.AreEqual(2, cache.Count);
            Assert.IsNotNull(cache.Get("key1"));
            Assert.IsNull(cache.Get("key2")); // Should be evicted
            Assert.IsNotNull(cache.Get("key3"));
        }

        [Test]
        public void FifoEvictionStrategy_ShouldEvictFirstAdded()
        {
            // Arrange
            var strategy = new FifoEvictionStrategy();
            var cache = new InMemoryCache(3, strategy);
            
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");

            // Act - Evict should remove key1 (first added)
            strategy.Evict(cache);

            // Assert
            Assert.AreEqual(2, cache.Count);
            Assert.IsNull(cache.Get("key1")); // Should be evicted
            Assert.IsNotNull(cache.Get("key2"));
            Assert.IsNotNull(cache.Get("key3"));
        }

        [Test]
        public void Evict_EmptyCache_ShouldNotThrow()
        {
            // Arrange
            var lruStrategy = new LruEvictionStrategy();
            var fifoStrategy = new FifoEvictionStrategy();
            var cache = new InMemoryCache(3, lruStrategy);

            // Act & Assert
            Assert.DoesNotThrow(() => lruStrategy.Evict(cache));
            Assert.DoesNotThrow(() => fifoStrategy.Evict(cache));
        }

        [Test]
        public void LruEvictionStrategy_OnAccess_ShouldUpdateAccessOrder()
        {
            // Arrange
            var strategy = new LruEvictionStrategy();
            var cache = new InMemoryCache(2, strategy);
            
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            
            // Act - Access key1 multiple times
            cache.Get("key1");
            cache.Get("key1");
            
            // Add key3, should evict key2
            cache.Add("key3", "value3");

            // Assert
            Assert.IsNotNull(cache.Get("key1"));
            Assert.IsNull(cache.Get("key2"));
            Assert.IsNotNull(cache.Get("key3"));
        }
    }
}