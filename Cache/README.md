# Cache Implementation with LRU Eviction Strategy

A high-performance, thread-safe in-memory cache implementation in C# with pluggable eviction strategies, demonstrated with a Star Wars API integration.

## Features

- **In-memory cache** with configurable capacity
- **High performance** - O(1) operations for millions of entries
- **Thread-safe** - Concurrent access support
- **Limited capacity** - Automatic eviction when full
- **Pluggable eviction strategies**:
  - LRU (Least Recently Used) - Primary implementation
  - FIFO (First In First Out) - Alternative strategy
- **Developer-friendly** - Simple and intuitive API

## Getting Started

### Prerequisites

- .NET SDK (version 6.0 or later)

### Running the Application

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd Cache
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the console application:
   ```bash
   dotnet run --project Cache/Cache.csproj
   ```

4. Run the unit tests:
   ```bash
   dotnet test
   ```

## Usage Example

```csharp
// Create cache with LRU eviction strategy (capacity: 5)
ICache cache = new InMemoryCache(5, new LruEvictionStrategy());

// Add items to cache
cache.Add("key1", "value1");
cache.Add("key2", "value2");

// Retrieve from cache
string value = cache.Get("key1"); // Returns "value1"

// When capacity is reached, least recently used items are evicted
cache.Add("key6", "value6"); // Evicts the LRU item

// Use alternative eviction strategy
ICache fifoCache = new InMemoryCache(5, new FifoEvictionStrategy());
```

## Project Structure

```
Cache/
├── Cache/                              # Main application project
│   ├── Core/
│   │   ├── InMemoryCache.cs           # Main cache implementation
│   │   ├── CacheEntry.cs              # Cache entry wrapper
│   │   ├── IEvictionStrategy.cs       # Eviction strategy interface
│   │   ├── LruEvictionStrategy.cs     # LRU eviction implementation
│   │   └── FifoEvictionStrategy.cs    # FIFO eviction implementation
│   ├── Services/
│   │   ├── ICache.cs                  # Cache interface
│   │   ├── IWebService.cs             # Web service interface
│   │   └── StarWarsApiService.cs      # Star Wars API integration
│   ├── Models/
│   │   └── StarWarsCharacter.cs       # Data model
│   ├── Program.cs                     # Demo application entry point
│   └── Cache.csproj                   # Project file
├── Cache.Test/                         # Unit tests project
│   ├── InMemoryCacheTests.cs          # Cache unit tests
│   ├── EvictionStrategyTests.cs       # Eviction strategy tests
│   └── Cache.Test.csproj              # Test project file
├── Cache.sln                           # Solution file
├── .gitignore                          # Git ignore file
└── README.md                           # This file
```

## Architecture

### Cache Implementation
- Uses `ConcurrentDictionary` for thread-safe storage
- Wraps values in `CacheEntry<T>` to track metadata (creation time, last accessed)
- Enforces capacity limits with automatic eviction

### LRU Eviction Strategy
- **O(1) eviction** using `LinkedList` + `ConcurrentDictionary`
- Maintains access order: Head = LRU, Tail = MRU
- Thread-safe with lock-based synchronization
- Perfect for high-performance scenarios with millions of entries

### FIFO Eviction Strategy
- **O(1) eviction** using `ConcurrentQueue`
- Evicts items in insertion order
- Demonstrates pluggable architecture

## Creating Custom Eviction Strategies

Implement the `IEvictionStrategy` interface:

```csharp
public class MyCustomStrategy : IEvictionStrategy
{
    public void Evict(InMemoryCache cache)
    {
        // Your eviction logic here
    }

    public void OnAdd(string key)
    {
        // Track new entries
    }

    public void OnAccess(string key)
    {
        // Track access patterns
    }
}

// Use your custom strategy
var cache = new InMemoryCache(100, new MyCustomStrategy());
```

## Performance

| Operation | Time Complexity | Description |
|-----------|----------------|-------------|
| Add | O(1) | Constant time insertion |
| Get | O(1) | Constant time lookup |
| Evict | O(1) | Constant time eviction |
| Memory | ~80 bytes/entry | Efficient memory usage |

Suitable for **millions of cached entries** with consistent performance.

## Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~LruEvictionStrategy_ShouldEvictLeastRecentlyUsed"
```

## Demo Application

The console application demonstrates:
- Fetching Star Wars character data from SWAPI (https://swapi.dev/)
- Caching API responses
- Cache hits vs misses
- Automatic eviction when capacity is reached
- LRU behavior with re-access patterns

## License

This project is for educational purposes as part of a practical assignment.

## Author

[Your Name]