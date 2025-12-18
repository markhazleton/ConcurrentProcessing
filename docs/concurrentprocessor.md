# Understanding the ConcurrentProcessor Class

## Overview

The `ConcurrentProcessor` class is an abstract class defined within the `ConcurrentProcessing.Concurrent` namespace. This class is designed to facilitate concurrent processing of tasks of generic type `T`, leveraging .NET 10's enhanced threading and async capabilities for optimal performance.

**Built with .NET 10 LTS** - Takes advantage of modern runtime optimizations, improved async/await performance, and enhanced threading primitives.

## Architecture

### Design Pattern
The `ConcurrentProcessor` implements the **Template Method Pattern**, allowing subclasses to define specific task processing logic while the framework handles:
- Concurrency management via `SemaphoreSlim`
- Task coordination and execution
- Performance metrics collection
- Resource cleanup

### Thread Safety
The class is designed to be thread-safe through:
- Immutable configuration (maxTaskCount, maxConcurrency)
- Thread-safe `SemaphoreSlim` for concurrency control
- Task-based asynchronous operations
- No shared mutable state between tasks

## Key Components and Functionality

### Constructor

```csharp
protected ConcurrentProcessor(int maxTaskCount, int maxConcurrency)
```

**Parameters:**
- `maxTaskCount`: Total number of tasks to process (determines task IDs: 1 to maxTaskCount)
- `maxConcurrency`: Maximum number of tasks running simultaneously

**Initializes:**
- Semaphore with `maxConcurrency` initial count
- Task list collection
- Internal tracking structures

**Example:**
```csharp
// Process 1000 tasks with max 50 running concurrently
var processor = new MyProcessor(maxTaskCount: 1000, maxConcurrency: 50);
```

### SemaphoreSlim and Task List

```csharp
private readonly SemaphoreSlim semaphore;
private readonly List<Task<T>> tasks;
```

**Purpose:**
- **`semaphore`**: Controls concurrent execution limit using .NET's efficient `SemaphoreSlim`
- **`tasks`**: Tracks all initiated tasks for coordination via `Task.WhenAll()`

**How it Works:**
1. Semaphore initialized with `maxConcurrency` available slots
2. Each task waits for semaphore availability before executing
3. Task releases semaphore slot upon completion
4. Enables efficient resource utilization without thread pool exhaustion

### AwaitSemaphoreAsync

```csharp
protected async Task<long> AwaitSemaphoreAsync()
```

**Purpose:** Waits for semaphore availability and measures wait time for performance tracking.

**Returns:** Wait time in ticks (for metrics)

**Behavior:**
- Asynchronously waits for semaphore slot
- Measures time spent waiting using `Stopwatch`
- Non-blocking wait (leverages async/await)

**Performance Note:** .NET 10's optimized `SemaphoreSlim` implementation provides better performance and lower allocations compared to earlier versions.

### GetNextTaskId

```csharp
protected virtual int? GetNextTaskId(int? taskId)
```

**Purpose:** Calculates the next task ID to process.

**Parameters:**
- `taskId`: Current task ID (nullable)

**Returns:**
- Next task ID (1-based indexing)
- `null` when all tasks are scheduled

**Default Behavior:**
- First call (`taskId == null`): Returns 1
- Subsequent calls: Returns `taskId + 1`
- When `taskId >= maxTaskCount`: Returns `null`

**Extensibility:** Virtual method allows subclasses to implement custom task ordering strategies (e.g., priority-based, random, adaptive).

### ManageProcessAsync

```csharp
protected async Task<T> ManageProcessAsync(int taskId, int taskCount, long waitMS, SemaphoreSlim semaphore)
```

**Purpose:** Manages individual task execution lifecycle.

**Parameters:**
- `taskId`: Unique identifier for this task
- `taskCount`: Total concurrent tasks when this task started
- `waitMS`: Semaphore wait time in ticks
- `semaphore`: Reference to controlling semaphore

**Workflow:**
1. Creates `ConcurrentProcessorModel` with task metadata
2. Calls abstract `ProcessAsync()` with task data
3. Records performance metrics in result
4. Releases semaphore slot in `finally` block (guaranteed cleanup)

**Exception Handling:** Semaphore is always released even if `ProcessAsync()` throws, preventing deadlock.

### ProcessAsync (Abstract)

```csharp
protected abstract Task<T> ProcessAsync(ConcurrentProcessorModel taskData);
```

**Purpose:** Defines the actual work to be performed for each task. **Must be implemented by subclasses.**

**Parameters:**
- `taskData`: Contains task context (ID, counters, timing info)

**Returns:** Task result of type `T`

**Implementation Guidelines:**
- Keep processing logic focused and deterministic
- Avoid shared mutable state between task instances
- Use async operations for I/O-bound work
- Handle exceptions appropriately (they propagate to `RunAsync()`)
- For .NET 10, consider using `ValueTask<T>` for very hot paths

**Example:**
```csharp
protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
{
    // Simulate work with explicit cast for .NET 10 clarity
    await Task.Delay(TimeSpan.FromMilliseconds((double)Random.Shared.Next(10, 20)));
    
    // Return result with task metadata
    return new SampleTaskResult(taskData);
}
```

### RunAsync

```csharp
public async Task<List<T>> RunAsync()
```

**Purpose:** Orchestrates concurrent processing of all tasks from start to completion.

**Returns:** `List<T>` containing results from all processed tasks

**Execution Flow:**
1. **Initialization Phase:**
   - Create empty task list
   - Reset task counters

2. **Task Scheduling Phase:**
   ```
   while (more tasks exist):
       Wait for semaphore availability
       Create and start new task
       Add task to tracking list
   ```

3. **Completion Phase:**
   - `await Task.WhenAll(tasks)` - Waits for all tasks to complete
   - Collects all results
   - Returns results list

**Concurrency Model:**
- Up to `maxConcurrency` tasks execute simultaneously
- New tasks start as slots become available
- Efficient CPU and I/O resource utilization
- No manual thread management required

**Exception Handling:**
- If any task throws, `Task.WhenAll()` aggregates exceptions
- Caller should wrap `RunAsync()` in try-catch
- Failed tasks do not prevent other tasks from completing

**Performance Characteristics (.NET 10):**
- Optimized async state machines
- Reduced allocations for await operations
- Better task scheduling from thread pool
- Improved GC pressure management

## Usage Example

Complete implementation example:

```csharp
using ConcurrentProcessing.Concurrent;

// Define custom result type
public class MyTaskResult
{
    public int TaskId { get; set; }
    public string Result { get; set; }
    public ConcurrentProcessorModel Metadata { get; set; }
}

// Implement processor
public class MyTaskProcessor : ConcurrentProcessor<MyTaskResult>
{
    public MyTaskProcessor(int maxTaskCount, int maxConcurrency)
        : base(maxTaskCount, maxConcurrency)
    {
    }

    protected override async Task<MyTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
    {
        // Perform actual work (async I/O, computation, etc.)
        var result = await PerformWorkAsync(taskData.TaskId);
        
        return new MyTaskResult
        {
            TaskId = taskData.TaskId,
            Result = result,
            Metadata = taskData
        };
    }

    private async Task<string> PerformWorkAsync(int taskId)
    {
        // Simulate async work
        await Task.Delay(TimeSpan.FromMilliseconds((double)Random.Shared.Next(10, 100)));
        return $"Task {taskId} completed";
    }
}

// Use the processor
var processor = new MyTaskProcessor(maxTaskCount: 100, maxConcurrency: 10);

try
{
    var results = await processor.RunAsync();
    
    Console.WriteLine($"Processed {results.Count} tasks");
    
    // Analyze results
    foreach (var result in results)
    {
        Console.WriteLine($"{result.Result} - Wait time: {result.Metadata.SemaphoreWait}ms");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Processing failed: {ex.Message}");
}
```

## Performance Considerations

### .NET 10 Optimizations
The runtime provides significant improvements:
- **Async Performance**: Reduced allocations and faster state machines
- **Thread Pool**: Better work-stealing and task scheduling
- **Garbage Collection**: Improved Gen0/Gen1 collection efficiency
- **JIT Compilation**: Better optimization of hot paths

### Choosing maxConcurrency

**CPU-Bound Work:**
```csharp
int maxConcurrency = Environment.ProcessorCount; // Or ProcessorCount * 2
```

**I/O-Bound Work:**
```csharp
int maxConcurrency = 50; // Or higher, depending on external service limits
```

**Mixed Workload:**
- Profile and test different values
- Monitor CPU utilization and task wait times
- Start conservative, increase if resources underutilized

### Monitoring Performance
```csharp
var results = await processor.RunAsync();

// Analyze metrics from ConcurrentProcessorModel
var avgWaitTime = results.Average(r => r.Metadata.SemaphoreWait);
var maxWaitTime = results.Max(r => r.Metadata.SemaphoreWait);
var avgDuration = results.Average(r => r.Metadata.TaskDuration);

Console.WriteLine($"Average wait: {avgWaitTime}ms");
Console.WriteLine($"Max wait: {maxWaitTime}ms");
Console.WriteLine($"Average duration: {avgDuration}ms");
```

## Best Practices

1. **Immutable Results**: Return immutable or thread-safe result objects
2. **Resource Cleanup**: Use `using` statements in `ProcessAsync()` for disposable resources
3. **Cancellation**: Consider adding `CancellationToken` support for long-running operations
4. **Error Handling**: Handle expected errors within `ProcessAsync()`, log unexpected ones
5. **Metrics**: Use `ConcurrentProcessorModel` data for performance analysis
6. **Testing**: Test with various `maxConcurrency` values to find optimal performance

## Advanced Scenarios

### Custom Task Ordering
Override `GetNextTaskId()` for specialized ordering:

```csharp
protected override int? GetNextTaskId(int? taskId)
{
    // Priority-based selection
    return PriorityQueue.TryDequeue(out var nextId) ? nextId : null;
}
```

### Dynamic Concurrency
Adjust concurrency based on runtime conditions (requires architectural changes).

### Progress Reporting
Track completion via task counting in `ProcessAsync()`.

## See Also

- [Getting Started Guide](getting-started.md) - Setup and basic usage
- [Creating Metrics](metrics.md) - Performance measurement and analysis
- [CI/CD Pipeline](cicd.md) - Automated testing and deployment
- [Contributing](CONTRIBUTING.md) - How to contribute improvements
