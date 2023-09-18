# The `ConcurrentProcessor` Class <a id="top"></a>

In this repository, we will dive into the `ConcurrentProcessor` class.
The `ConcurrentProcessor` class is a versatile tool designed to help developers efficiently manage
and process multiple tasks concurrently in a controlled manner. This documentation will guide you
through the steps to use the `ConcurrentProcessor` class effectively in your C# applications.

## Table of Contents

1. [Understanding the ConcurrentProcessor Class](#UnderstandingTheConcurrentProcessorClass)
1. [Getting Started](#getting-started)
1. [Creating Metrics for 'ConcurrentProcessor'](#creating-metrics)
1. [Contributing](#contributing)
1. [License](#license)
1. [References](#references)

## Understanding the `ConcurrentProcessor` Class <a id="UnderstandingTheConcurrentProcessorClass"></a> | [Back to Top](#top)

The `ConcurrentProcessor` class is an abstract class defined within the `ConcurrentProcessing.Concurrent` namespace.  
This class is designed to facilitate concurrent processing of tasks of generic type `T`.
Let's break down its key components and functionality:

### ConcurrentProcessor Constructor

```csharp
protected ConcurrentProcessor(int maxTaskCount, int maxConcurrency)
```

- The constructor initializes the `ConcurrentProcessor` class with two parameters:
  - `maxTaskCount`: The maximum number of tasks to be processed.
  - `maxConcurrency`: The maximum level of concurrency allowed during task processing.

### SemaphoreSlim and Task List

```csharp
private readonly SemaphoreSlim semaphore;
private readonly List<Task<T>> tasks;
```

- `SemaphoreSlim` (`semaphore`) is used to control access to a limited number of concurrent tasks. It ensures that no more than `maxConcurrency` tasks run simultaneously.
- `List<Task<T>>` (`tasks`) keeps track of the tasks that are currently being processed.

### AwaitSemaphoreAsync Method

```csharp
protected async Task<long> AwaitSemaphoreAsync()
```

- This method asynchronously waits for the semaphore to become available and measures the time it takes to acquire the semaphore.

### GetNextTaskId Method

```csharp
protected virtual int? GetNextTaskId(int? taskId)
```

- This method calculates the next task ID to be processed. It ensures that the number of tasks does not exceed `maxTaskCount`.

### ManageProcessAsync Method

```csharp
protected async Task<T> ManageProcessAsync(int taskId, int taskCount, long waitMS, SemaphoreSlim semaphore)
```

- This method manages the asynchronous processing of a single task.
- It records information about the task, such as task ID, task count, wait time, semaphore count, and semaphore wait time.
- It then calls the abstract `ProcessAsync` method to perform the actual processing of the task.
- After processing, it releases the semaphore and records the time taken for task execution.

### ProcessAsync Method (Abstract)

```csharp
protected abstract Task<T> ProcessAsync(ConcurrentProcessorModel taskData);
```

- This abstract method defines the logic to process a single task of type `T`. Subclasses must implement this method according to their specific processing requirements.

### RunAsync Method

```csharp
public async Task<List<T>> RunAsync()
```

- This method orchestrates the concurrent processing of tasks.
- It uses a loop to continuously process tasks until all tasks are completed.
- For each task, it waits for the semaphore, manages the task processing, and adds the tasks to the `tasks` list.
- If the number of tasks reaches `maxConcurrency`, it asynchronously waits for the first completed task and adds its result to the `results` list.
- After all tasks are started, it waits for all of them to complete using `Task.WhenAll`.

## Getting Started <a id="getting-started"></a> | [Back To Top](#top)

To begin using the `ConcurrentProcessor` class, ensure that you have the C# project set up in your development environment.

### Create a Subclass

To use the `ConcurrentProcessor` class, you have create a subclass that extends it.
This subclass should implement the abstract `ProcessAsync` method to define the logic for processing a single task of type `T`.

Here is an example of how you can create a subclass called 'SampleTaskProcessor':

```csharp
public class SampleTaskProcessor : ConcurrentProcessor<SampleTaskResult>
{
    public SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
        : base(maxTaskCount, maxConcurrency)
    {
    }
}
```

### Override ProcessAsync

In your subclass, you must override the `ProcessAsync` method. This is where you define how each task should be processed. The method receives a parameter of type `ConcurrentProcessorModel` that provides information about the task and the current state of concurrency. Make sure to return a result of type `Task<SampleTaskResult>`.

```csharp
    protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(10, 20)));
        return new SampleTaskResult(taskData);
    }
```

### Running the Concurrent Processor

To start the concurrent processing of tasks, create an instance of your subclass and call the `RunAsync` method. This method will handle the concurrent execution of tasks based on the parameters you provided during initialization.

```csharp
var processor = new MyConcurrentProcessor(maxTaskCount, maxConcurrency);
var results = await processor.RunAsync();
```

## Creating Metrics for 'ConcurrentProcessor' <a id="creating-metrics"></a> | [Back To Top](#top)

Concurrency is a crucial aspect of modern software development, allowing programs to efficiently execute multiple tasks simultaneously. One essential aspect of managing concurrent processes is monitoring and optimizing performance, which can be achieved by collecting and analyzing metrics. In this article, we will explore how to create metrics for a ConcurrentProcessor, a fundamental component in concurrent programming.

### The Importance of Metrics

Metrics provide insights into the performance and behavior of a ConcurrentProcessor. By tracking various metrics, you can identify bottlenecks, optimize resource usage, and fine-tune the concurrency settings. Some critical metrics to monitor include:

1. **Task Execution Time**: Measure the time it takes to execute each task. This helps identify tasks that take longer to complete.
1. **Semaphore Wait Time**: Monitor how long tasks wait in the semaphore queue before execution. Excessive wait times may indicate semaphore contention.
1. **Semaphore Count**: Keep track of the number of available semaphore slots. It helps ensure that the maximum concurrency limit is not exceeded.
1. **Task Count**: Observe the number of tasks currently being processed. It allows you to balance the workload and avoid overloading the system.

### Implementing Metrics

To implement metrics for your ConcurrentProcessor, you can follow these steps:

The  `ConcurrentProcessorModel` class includes properties for tracking the metrics.

```csharp
public class ConcurrentProcessorModel
{
    public int TaskId { get; set; }
    public long TaskExecutionTime { get; set; }
    public long SemaphoreWaitTime { get; set; }
    public int SemaphoreCount { get; set; }
    public int TaskCount { get; set; }
}
```

Inside the `ConcurrentProcessor` class, capture the metrics during task execution. Update the `ManageProcessAsync` method to record relevant metrics:

```csharp
protected async Task<T> ManageProcessAsync(int taskId, int taskCount, long waitTicks, SemaphoreSlim semaphore) 
{
    Stopwatch sw = Stopwatch.StartNew();
    sw.Start();
    T result;
    try
    {
        ConcurrentProcessorModel taskData = new()
        {
            TaskId = taskId,
            TaskCount = taskCount,
            SemaphoreCount = semaphore.CurrentCount,
            SemaphoreWaitTime = waitTicks
        };

        result = await ProcessAsync(taskData);

        // Calculate task execution time
        taskData.TaskExecutionTime = sw.ElapsedTicks;
    }
    finally
    {
        semaphore.Release();
        sw.Stop();
    }
    return result;
}
```

After executing tasks with your ConcurrentProcessor, aggregate the collected metrics and report them. You can create a separate method for this purpose:

```csharp
public void ReportMetrics(List<ConcurrentProcessorModel> metrics)
{
    // Calculate and report minimum, maximum, and average values for each metric
    foreach (var metricName in GetMetricNames())
    {
        var metricValues = metrics.Select(m => GetMetricValue(m, metricName)).ToList();
        long min = metricValues.Min();
        long max = metricValues.Max();
        double average = metricValues.Average();

        Console.WriteLine($"{metricName.PadRight(20)}\tMinimum: {min}\tMaximum: {max}\tAverage: {average:F2}");
    }
}
```

### Metrics Review

Now that we have metrics configured, we run a few different scenarios to compare the results.

```output
Starting 100 tasks with a max concurrency of 1...
TaskCount               Minimum: 0      Maximum: 0      Average: 0.00
WaitTicks               Minimum: 2      Maximum: 638    Average: 14.12
SemaphoreCount          Minimum: 0      Maximum: 0      Average: 0.00
SemaphoreWait           Minimum: 2      Maximum: 638    Average: 14.12
TaskDuration            Minimum: 12     Maximum: 33     Average: 20.05
Total Duration: 2083ms
Starting 100 tasks with a max concurrency of 10...
TaskCount               Minimum: 0      Maximum: 9      Average: 8.55
WaitTicks               Minimum: 1      Maximum: 9      Average: 1.87
SemaphoreCount          Minimum: 0      Maximum: 9      Average: 2.51
SemaphoreWait           Minimum: 1      Maximum: 9      Average: 1.87
TaskDuration            Minimum: 9      Maximum: 33     Average: 19.58
Total Duration: 213ms
Starting 100 tasks with a max concurrency of 50...
TaskCount               Minimum: 0      Maximum: 49     Average: 36.75
WaitTicks               Minimum: 1      Maximum: 3085   Average: 39.04
SemaphoreCount          Minimum: 0      Maximum: 49     Average: 21.66
SemaphoreWait           Minimum: 1      Maximum: 3085   Average: 39.04
TaskDuration            Minimum: 9      Maximum: 30     Average: 16.73
Total Duration: 62ms
```


Creating metrics for your ConcurrentProcessor is a critical step in optimizing concurrent task execution. By monitoring task execution times, semaphore wait times, and other relevant metrics, you can gain valuable insights into your system's performance. These insights can guide you in making informed decisions to fine-tune your concurrency settings and improve the efficiency of your concurrent processing tasks.

Remember that metrics should be an integral part of your software development process, helping you identify and address performance bottlenecks proactively. With well-implemented metrics, you can ensure that your ConcurrentProcessor operates at its optimal capacity, delivering efficient concurrent task execution.

## Contributing <a id="contributing"></a> | [Back To Top](#top)

Contributions to the ConcurrentProcessor project are welcome! If you have suggestions, bug reports, or want to contribute code, please follow our [Contribution Guidelines](CONTRIBUTING.md).

## License <a id="license"></a> | [Back To Top](#top)

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## References <a id="references"></a> | [Back To Top](#top)

1. [SemaphoreSlim Class (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim)
1. [Microsoft Docs: Stopwatch Class](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch)
1. [Understanding Performance Counters in .NET](https://docs.microsoft.com/en-us/dotnet/core/diagnostics/performance-counters)
1. [Task Class (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task)
1. [C# Asynchronous Programming (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
1. [C# Abstract Classes (Microsoft Docs)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-classes)
