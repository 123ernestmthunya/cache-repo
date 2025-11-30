using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Core
{
    public interface IEvictionStrategy
    {
        void Evict(InMemoryCache cache);
        void OnAdd(string key);
        void OnAccess(string key);
    }
}
