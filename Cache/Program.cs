using System;
using System.Threading.Tasks;
using Cache.Core;
using Cache.Models;
using Cache.Services;

namespace Cache
{
    class Program
    {
        private static readonly string ApiUrl = "https://swapi.dev/api/people/";
        private static ICache cache;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Star Wars Character Cache Demo");
            Console.WriteLine("================================\n");

            // Initialize cache with LRU eviction strategy (capacity: 5 for demo)
            cache = new InMemoryCache(5, new LruEvictionStrategy());
            var webService = new StarWarsApiService();

            // Fetch 10 characters
            for (int i = 1; i <= 10; i++)
            {
                var character = await GetCharacterAsync(webService, i.ToString());
                Console.WriteLine($"Character {i}: {character}");
                Console.WriteLine($"Cache size: {cache.Count}\n");
            }

            Console.WriteLine("\n--- Re-fetching first 3 characters (should come from cache) ---\n");
            
            for (int i = 1; i <= 3; i++)
            {
                var character = await GetCharacterAsync(webService, i.ToString());
                Console.WriteLine($"Character {i}: {character}");
            }

            Console.ReadKey();
        }

        static async Task<string> GetCharacterAsync(IWebService webService, string id)
        {
            // Try to get from cache first
            var cachedData = cache.Get(id);
            if (cachedData != null)
            {
                Console.WriteLine($"[CACHE HIT] Retrieved character {id} from cache");
                return cachedData;
            }

            // If not in cache, fetch from API
            Console.WriteLine($"[CACHE MISS] Fetching character {id} from API...");
            var data = await webService.GetDataAsync($"{ApiUrl}{id}/");
            
            // Add to cache
            cache.Add(id, data);
            
            return data;
        }
    }
}