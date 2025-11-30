using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Services
{
    public interface IWebService
    {
        Task<string> GetDataAsync(string url);
    }
}
