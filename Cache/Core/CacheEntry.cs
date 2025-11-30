using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Core
{
    public class CacheEntry<T>
    {
        public T Value { get; }
        public DateTime LastAccessed { get; set; }
        public DateTime CreatedAt { get; }

        public CacheEntry(T value)
        {
            Value = value;
            CreatedAt = DateTime.UtcNow;
            LastAccessed = DateTime.UtcNow;
        }
    }
}
