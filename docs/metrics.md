# Creating Metrics for ConcurrentProcessor

Metrics help monitor and optimize concurrent processing. Key metrics include:

- **Task Execution Time**
- **Semaphore Wait Time**
- **Semaphore Count**
- **Task Count**

## Implementing Metrics

The `ConcurrentProcessorModel` class tracks metrics:

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

Update `ManageProcessAsync` to record metrics:

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

## Reporting Metrics

Aggregate and report metrics after execution:

```csharp
public void ReportMetrics(List<ConcurrentProcessorModel> metrics)
{
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

## Example Metrics Output

```
Starting 100 tasks with a max concurrency of 1...
TaskCount               Minimum: 0      Maximum: 0      Average: 0.00
WaitTicks               Minimum: 2      Maximum: 638    Average: 14.12
SemaphoreCount          Minimum: 0      Maximum: 0      Average: 0.00
SemaphoreWait           Minimum: 2      Maximum: 638    Average: 14.12
TaskDuration            Minimum: 12     Maximum: 33     Average: 20.05
Total Duration: 2083ms
...
```
