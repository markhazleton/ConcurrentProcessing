# ConcurrentProcessing

[![.NET Build and Test](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/dotnet.yml/badge.svg)](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/dotnet.yml)
[![Release](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/release.yml/badge.svg)](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/release.yml)
[![.NET Version](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> A high-performance, flexible concurrent task processing framework for .NET 10 that provides fine-grained control over parallelism with built-in performance metrics.

## 🎯 Overview

**ConcurrentProcessing** is a demonstration application showcasing advanced concurrent programming patterns in C# using .NET 10. It provides a robust, production-ready framework for managing parallel task execution with configurable concurrency limits, comprehensive performance tracking, and detailed metrics analysis.

📖 **[Read the full article](https://markhazleton.com/concurrent-processing.html)** for in-depth explanations, real-world use cases, and best practices.

### Key Features

- ✨ **Generic Abstract Base Class** - Easily extensible `ConcurrentProcessor<T>` for custom task processing
- 🚦 **Semaphore-Based Throttling** - Precise control over concurrent execution limits
- 📊 **Built-in Performance Metrics** - Real-time tracking of task duration, wait times, and throughput
- ⚡ **High Performance** - Optimized for .NET 10 runtime with minimal overhead
- 🔧 **Type-Safe Design** - Strongly-typed results with full IntelliSense support
- 📈 **Statistical Analysis** - Automatic calculation of min, max, and average metrics
- 🎓 **Educational Resource** - Well-documented code demonstrating best practices

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Visual Studio 2022 (17.10+) or Visual Studio Code with C# extension

### Installation

```bash
# Clone the repository
git clone https://github.com/markhazleton/ConcurrentProcessing.git

# Navigate to the project directory
cd ConcurrentProcessing

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## 📖 Usage

### Basic Example

The simplest way to use the framework is to inherit from `ConcurrentProcessor<T>` and implement the `ProcessAsync` method:

```csharp
using ConcurrentProcessing.Concurrent;

public class SampleTaskProcessor : ConcurrentProcessor<SampleTaskResult>
{
    public SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
        : base(maxTaskCount, maxConcurrency)
    {
    }

    protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
    {
        // Your custom processing logic here
        await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(10, 20)));
        
        return new SampleTaskResult(taskData);
    }
}
```

### Running Tasks

```csharp
// Initialize the processor with 100 total tasks and 10 concurrent limit
var processor = new SampleTaskProcessor(maxTaskCount: 100, maxConcurrency: 10);

// Execute tasks and collect results
var results = await processor.RunAsync();

// Calculate and display performance metrics
var models = results.Cast<ConcurrentProcessorModel>().ToList();
MetricCalculator.CalculateAndDisplayMetrics(models);
```

### Output Example

```
Starting 100 tasks with a max concurrency of 10...
TaskCount           	Minimum: 0	Maximum: 99	Average: 49.50
WaitTicks           	Minimum: 0	Maximum: 15234	Average: 4532.23
SemaphoreCount      	Minimum: 0	Maximum: 9	Average: 4.50
SemaphoreWait       	Minimum: 0	Maximum: 15234	Average: 4532.23
TaskDuration        	Minimum: 10	Maximum: 19	Average: 14.52
Total Duration: 245ms
```

## 🏗️ Architecture

### Core Components

#### 1. `ConcurrentProcessor<T>`
The abstract base class providing the core concurrent processing engine.

**Key Features:**
- Generic type parameter for strongly-typed results
- Semaphore-based concurrency control
- Automatic wait time tracking
- Efficient task batching and execution

**Key Methods:**
- `RunAsync()` - Executes all tasks with specified concurrency limit
- `ProcessAsync(ConcurrentProcessorModel)` - Abstract method for custom task implementation
- `AwaitSemaphoreAsync()` - Tracks and manages semaphore acquisition
- `GetNextTaskId(int?)` - Provides extensible task ID generation

#### 2. `ConcurrentProcessorModel`
Base data model tracking task execution metrics.

**Properties:**
- `TaskId` - Unique identifier for each task
- `TaskCount` - Current number of tasks in the execution queue
- `WaitTicks` - Time spent waiting for semaphore access
- `SemaphoreCount` - Available semaphore slots at task start
- `SemaphoreWait` - Duration of semaphore wait
- `TaskDurationMS` - Total task execution time in milliseconds

#### 3. `MetricCalculator`
Static utility class for statistical analysis of task execution.

**Features:**
- Calculates min, max, and average for all metrics
- Console-based output formatting
- Extensible metric analysis

#### 4. `SampleTaskProcessor` & `SampleTaskResult`
Example implementation demonstrating framework usage.

### Design Patterns

- **Template Method Pattern** - Abstract base class with customizable processing logic
- **Factory Pattern** - Task ID generation for extensibility
- **Strategy Pattern** - Pluggable task processing implementation
- **Resource Pool Pattern** - Semaphore-based resource management

## 📊 Performance Characteristics

The framework is designed for high-performance concurrent operations:

- **Minimal Overhead**: Direct semaphore usage without additional abstractions
- **Memory Efficient**: Reuses task lists and avoids unnecessary allocations
- **Scalable**: Tested with up to 1000+ concurrent tasks
- **Predictable**: Linear scaling with configurable concurrency limits

### Benchmark Results

| Tasks | Concurrency | Avg Duration | Total Time |
|-------|-------------|--------------|------------|
| 100   | 1           | ~15ms        | ~1500ms    |
| 100   | 10          | ~15ms        | ~250ms     |
| 100   | 50          | ~15ms        | ~75ms      |
| 1000  | 500         | ~15ms        | ~500ms     |
| 1000  | 1000        | ~15ms        | ~50ms      |

*Results may vary based on hardware and workload*

## 🎓 Educational Value

This project serves as an excellent learning resource for:

- **Concurrent Programming** - Understanding Task Parallel Library (TPL)
- **Semaphore Management** - Throttling and resource coordination
- **Generic Abstractions** - Building reusable, type-safe frameworks
- **Performance Analysis** - Measuring and optimizing concurrent operations
- **C# 12+ Features** - Primary constructors, pattern matching, nullable reference types
- **.NET 10 Features** - Latest runtime optimizations

## 📚 Documentation

### Core Documentation
- [Getting Started](docs/getting-started.md) - Setup and basic usage
- [Understanding the ConcurrentProcessor Class](docs/concurrentprocessor.md) - In-depth architecture
- [Creating Metrics](docs/metrics.md) - Performance monitoring and analysis
- [Contributing](docs/CONTRIBUTING.md) - Contribution guidelines

### CI/CD & DevOps
- [CI/CD Pipelines](docs/cicd.md) - Build and deployment automation
- [CI/CD Quick Reference](docs/cicd-quickref.md) - Fast command lookup

### Additional Resources
- [References](docs/references.md) - External resources and links
- [License](docs/license.md) - MIT License details
- [Changelog](CHANGELOG.md) - Version history

### Blog Article
For a comprehensive walkthrough with real-world scenarios, read the [Full Article on Concurrent Processing](https://markhazleton.com/concurrent-processing.html).

## 🔄 Latest Updates (v2.0.0)

**What's New:**
- ✅ Upgraded to .NET 10.0 LTS for enhanced performance and long-term support
- ✅ Primary constructor syntax for cleaner, more concise code
- ✅ Comprehensive GitHub Actions CI/CD pipelines
- ✅ Multi-platform automated releases (Windows, Linux, macOS)
- ✅ Enhanced XML documentation for better IntelliSense
- ✅ Automated dependency updates via Dependabot
- ✅ Security scanning and code analysis integration

📋 [**View Complete Changelog**](CHANGELOG.md)

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository** and create a feature branch
2. **Write clear commit messages** describing your changes
3. **Add tests** for new functionality
4. **Update documentation** to reflect your changes
5. **Submit a pull request** to the `main` branch

All contributions, questions, and bug reports should be submitted via [GitHub Issues](https://github.com/markhazleton/ConcurrentProcessing/issues).

### Development Workflow

```bash
# Create a feature branch
git checkout -b feature/your-feature-name

# Make your changes and commit
git add .
git commit -m "feat: add your feature description"

# Push to your fork
git push origin feature/your-feature-name

# Open a pull request on GitHub
```

See [CONTRIBUTING.md](docs/CONTRIBUTING.md) for detailed guidelines.

## 📜 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

The MIT License allows you to:
- ✅ Use the code commercially
- ✅ Modify the code
- ✅ Distribute the code
- ✅ Use the code privately

See [docs/license.md](docs/license.md) for more information.

## 🔒 Security

- Do not include sensitive or private information in issues or pull requests
- Report security vulnerabilities via [GitHub Security Advisories](https://github.com/markhazleton/ConcurrentProcessing/security/advisories)
- Keep dependencies up to date using Dependabot

## 🙏 Acknowledgments

- Built with [.NET 10](https://dotnet.microsoft.com/)
- Powered by [GitHub Actions](https://github.com/features/actions)
- Code analysis by [Microsoft.CodeAnalysis.NetAnalyzers](https://github.com/dotnet/roslyn-analyzers)

## 📞 Contact & Support

- **Issues**: [GitHub Issues](https://github.com/markhazleton/ConcurrentProcessing/issues)
- **Discussions**: [GitHub Discussions](https://github.com/markhazleton/ConcurrentProcessing/discussions)
- **Full Article**: [Concurrent Processing Deep Dive](https://markhazleton.com/concurrent-processing.html)
- **Author**: [Mark Hazleton](https://github.com/markhazleton)

---

<div align="center">

**[⭐ Star this repository](https://github.com/markhazleton/ConcurrentProcessing)** if you find it helpful!

Made with ❤️ by [Mark Hazleton](https://markhazleton.controlorigins.com)

</div>
