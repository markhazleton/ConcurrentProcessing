# Understanding the ConcurrentProcessor Class

The `ConcurrentProcessor` class is an abstract class defined within the `ConcurrentProcessing.Concurrent` namespace.  
This class is designed to facilitate concurrent processing of tasks of generic type `T`.

## Key Components and Functionality

### Constructor

```csharp
protected ConcurrentProcessor(int maxTaskCount, int maxConcurrency)
```

- Initializes with `maxTaskCount` and `maxConcurrency`.

### SemaphoreSlim and Task List

```csharp
private readonly SemaphoreSlim semaphore;
private readonly List<Task<T>> tasks;
```

- Controls concurrency and tracks running tasks.

### AwaitSemaphoreAsync

```csharp
protected async Task<long> AwaitSemaphoreAsync()
```

- Waits for the semaphore and measures wait time.

### GetNextTaskId

```csharp
protected virtual int? GetNextTaskId(int? taskId)
```

- Calculates the next task ID.

### ManageProcessAsync

```csharp
protected async Task<T> ManageProcessAsync(int taskId, int taskCount, long waitMS, SemaphoreSlim semaphore)
```

- Manages processing, records metrics, and releases semaphore.

### ProcessAsync (Abstract)

```csharp
protected abstract Task<T> ProcessAsync(ConcurrentProcessorModel taskData);
```

- Subclasses implement this to define task logic.

### RunAsync

```csharp
public async Task<List<T>> RunAsync()
```

- Orchestrates concurrent processing of all tasks.
