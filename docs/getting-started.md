# Getting Started with ConcurrentProcessor

## Prerequisites

- **.NET 10 SDK** or later
- A C# development environment (Visual Studio 2022, VS Code, Rider, or CLI)

**Install .NET 10 SDK:**
- Download: https://dotnet.microsoft.com/download/dotnet/10.0
- Verify installation: `dotnet --version` (should show 10.0.x or later)

## Project Setup

Create a new console application targeting .NET 10:

```bash
dotnet new console -n MyConcurrentApp -f net10.0
cd MyConcurrentApp
```

Or update an existing project's `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
</Project>
```

## Using ConcurrentProcessor

To use the `ConcurrentProcessor` class, set up your C# project and create a subclass that implements the abstract `ProcessAsync` method.

## Create a Subclass

```csharp
public class SampleTaskProcessor : ConcurrentProcessor<SampleTaskResult>
{
    public SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
        : base(maxTaskCount, maxConcurrency)
    {
    }
}
```

## Override ProcessAsync

```csharp
protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
{
    // Note: Explicit cast for .NET 10 TimeSpan overload clarity
    await Task.Delay(TimeSpan.FromMilliseconds((double)new Random().Next(10, 20)));
    return new SampleTaskResult(taskData);
}
```

## Running the Processor

```csharp
var processor = new MyConcurrentProcessor(maxTaskCount, maxConcurrency);
var results = await processor.RunAsync();
```

## Quick Start Example

Complete working example:

```csharp
using ConcurrentProcessing.Concurrent;

// Create processor with 100 tasks and 10 concurrent executions
var processor = new SampleTaskProcessor(maxTaskCount: 100, maxConcurrency: 10);

// Run all tasks concurrently
var results = await processor.RunAsync();

// Process results
Console.WriteLine($"Processed {results.Count} tasks");
```

## Building and Running

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Run the application
dotnet run --configuration Release
```

## Performance Considerations

**.NET 10 Improvements:**
- Enhanced JIT compilation optimizations
- Improved garbage collection performance
- Better async/await performance
- Optimized System.Threading primitives

**Best Practices:**
- Use appropriate concurrency limits based on workload type
- Monitor semaphore wait times for bottlenecks
- Consider using `ValueTask<T>` for hot paths (available in .NET 10)
- Profile with `dotnet-counters` and PerfView

## Next Steps

- Read [Understanding the ConcurrentProcessor Class](concurrentprocessor.md) for in-depth details
- Explore [Creating Metrics](metrics.md) for performance monitoring
- Review [CI/CD Pipeline](cicd.md) for automated builds and testing
- Check [Contributing Guidelines](CONTRIBUTING.md) to contribute improvements
