# Star Wars Console Application

This is a simple .NET console application that interacts with the Star Wars API to fetch character data. It implements an in-memory cache with high performance, thread safety, limited capacity, and a pluggable eviction strategy.

## Features

- Fetches data from the Star Wars API.
- Implements an in-memory cache to store API responses.
- Supports multiple eviction strategies (LRU and FIFO).
- Thread-safe operations for concurrent access.

## Getting Started

### Prerequisites

- .NET SDK (version 6.0 or later)

### Running the Application

1. Clone the repository:
   ```
   git clone <repository-url>
   ```

2. Navigate to the project directory:
   ```
   cd StarWarsConsoleApp
   ```

3. Restore the dependencies:
   ```
   dotnet restore
   ```

4. Run the application:
   ```
   dotnet run --project src/StarWarsConsoleApp.csproj
   ```

## Project Structure

- **src/Program.cs**: Entry point of the application.
- **src/Services**: Contains interfaces and implementations for web services.
- **src/Cache**: Contains the in-memory cache implementation and eviction strategies.
- **src/Models**: Contains models representing data structures.

## Contributing

Feel free to submit issues or pull requests for improvements or bug fixes.