using Cache.Core;
using Cache.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Cache.Test
{
    [TestFixture]
    public class InMemoryCacheTests
    {
        [Test]
        public void Add_SingleItem_ShouldStoreInCache()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());

            // Act
            cache.Add("key1", "value1");

            // Assert
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual("value1", cache.Get("key1"));
        }

        [Test]
        public void Get_ExistingKey_ShouldReturnValue()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());
            cache.Add("key1", "value1");

            // Act
            var result = cache.Get("key1");

            // Assert
            Assert.AreEqual("value1", result);
        }

        [Test]
        public void Get_NonExistingKey_ShouldReturnNull()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());

            // Act
            var result = cache.Get("nonexistent");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Remove_ExistingKey_ShouldRemoveFromCache()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());
            cache.Add("key1", "value1");

            // Act
            cache.Remove("key1");

            // Assert
            Assert.AreEqual(0, cache.Count);
            Assert.IsNull(cache.Get("key1"));
        }

        [Test]
        public void Add_ExceedingCapacity_WithLRU_ShouldEvictLeastRecentlyUsed()
        {
            // Arrange
            var cache = new InMemoryCache(3, new LruEvictionStrategy());
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");

            // Access key1 and key2 to make them recently used
            cache.Get("key1");
            cache.Get("key2");

            // Act - Add 4th item, should evict key3 (least recently used)
            cache.Add("key4", "value4");

            // Assert
            Assert.AreEqual(3, cache.Count);
            Assert.IsNotNull(cache.Get("key1"));
            Assert.IsNotNull(cache.Get("key2"));
            Assert.IsNull(cache.Get("key3")); // Should be evicted
            Assert.IsNotNull(cache.Get("key4"));
        }

        [Test]
        public void Add_ExceedingCapacity_WithFIFO_ShouldEvictFirstAdded()
        {
            // Arrange
            var cache = new InMemoryCache(3, new FifoEvictionStrategy());
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");

            // Act - Add 4th item, should evict key1 (first added)
            cache.Add("key4", "value4");

            // Assert
            Assert.AreEqual(3, cache.Count);
            Assert.IsNull(cache.Get("key1")); // Should be evicted
            Assert.IsNotNull(cache.Get("key2"));
            Assert.IsNotNull(cache.Get("key3"));
            Assert.IsNotNull(cache.Get("key4"));
        }

        [Test]
        public void Count_ShouldReflectNumberOfItems()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());

            // Act & Assert
            Assert.AreEqual(0, cache.Count);

            cache.Add("key1", "value1");
            Assert.AreEqual(1, cache.Count);

            cache.Add("key2", "value2");
            Assert.AreEqual(2, cache.Count);

            cache.Remove("key1");
            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Add_ThreadSafety_MultipleConcurrentAdds()
        {
            // Arrange
            var cache = new InMemoryCache(100, new LruEvictionStrategy());
            var tasks = new Task[10];

            // Act
            for (int i = 0; i < 10; i++)
            {
                int index = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        cache.Add($"key_{index}_{j}", $"value_{index}_{j}");
                    }
                });
            }

            Task.WaitAll(tasks);

            // Assert
            Assert.LessOrEqual(cache.Count, 100);
            Assert.Greater(cache.Count, 0);
        }

        [Test]
        public void Get_UpdatesLastAccessed_ForLRU()
        {
            // Arrange
            var cache = new InMemoryCache(2, new LruEvictionStrategy());
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");

            // Act - Access key1 to make it recently used
            cache.Get("key1");
            
            // Add 3rd item, should evict key2 (not key1 which was just accessed)
            cache.Add("key3", "value3");

            // Assert
            Assert.IsNotNull(cache.Get("key1")); // Should still be in cache
            Assert.IsNull(cache.Get("key2")); // Should be evicted
            Assert.IsNotNull(cache.Get("key3"));
        }

        [Test]
        public void Add_SameKey_ShouldUpdateValue()
        {
            // Arrange
            var cache = new InMemoryCache(5, new LruEvictionStrategy());
            cache.Add("key1", "value1");

            // Act
            cache.Add("key1", "value2");

            // Assert
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual("value2", cache.Get("key1"));
        }

        [Test]
        public void LRU_MultipleEvictions_ShouldMaintainCorrectOrder()
        {
            // Arrange
            var cache = new InMemoryCache(3, new LruEvictionStrategy());
            
            // Act
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");
            
            // Access key1
            cache.Get("key1");
            
            // Add key4 - should evict key2
            cache.Add("key4", "value4");
            
            // Add key5 - should evict key3
            cache.Add("key5", "value5");

            // Assert
            Assert.AreEqual(3, cache.Count);
            Assert.IsNotNull(cache.Get("key1")); // Accessed, should remain
            Assert.IsNull(cache.Get("key2")); // Should be evicted first
            Assert.IsNull(cache.Get("key3")); // Should be evicted second
            Assert.IsNotNull(cache.Get("key4"));
            Assert.IsNotNull(cache.Get("key5"));
        }

        [Test]
        public void FIFO_MultipleEvictions_ShouldMaintainInsertionOrder()
        {
            // Arrange
            var cache = new InMemoryCache(3, new FifoEvictionStrategy());
            
            // Act
            cache.Add("key1", "value1");
            cache.Add("key2", "value2");
            cache.Add("key3", "value3");
            
            // Access doesn't matter for FIFO
            cache.Get("key1");
            cache.Get("key2");
            
            // Add key4 - should evict key1 (first in)
            cache.Add("key4", "value4");
            
            // Add key5 - should evict key2 (second in)
            cache.Add("key5", "value5");

            // Assert
            Assert.AreEqual(3, cache.Count);
            Assert.IsNull(cache.Get("key1")); // First added, first evicted
            Assert.IsNull(cache.Get("key2")); // Second added, second evicted
            Assert.IsNotNull(cache.Get("key3"));
            Assert.IsNotNull(cache.Get("key4"));
            Assert.IsNotNull(cache.Get("key5"));
        }
    }
}