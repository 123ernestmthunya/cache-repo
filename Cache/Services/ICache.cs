using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Services
{
    public interface ICache
    {
        void Add(string key, string value);
        string Get(string key);
        void Remove(string key);
        int Count { get; }
    }
}
