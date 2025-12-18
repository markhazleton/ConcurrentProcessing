using System.Diagnostics;

namespace ConcurrentProcessing.Concurrent;

/// <summary>
/// Represents an abstract concurrent processor that manages task execution with controlled concurrency.
/// </summary>
/// <typeparam name="T">The type of the concurrent processor model, which must inherit from <see cref="ConcurrentProcessorModel"/>.</typeparam>
/// <remarks>
/// This class provides a framework for executing tasks concurrently while limiting the maximum number of concurrent operations.
/// It uses a semaphore to control concurrency and tracks detailed metrics for each task execution.
/// </remarks>
/// <param name="maxTaskCount">The maximum number of tasks to process.</param>
/// <param name="maxConcurrency">The maximum number of tasks that can run concurrently.</param>
public abstract class ConcurrentProcessor<T>(int maxTaskCount, int maxConcurrency) where T : ConcurrentProcessorModel
{
    private readonly SemaphoreSlim semaphore = new(maxConcurrency);
    private readonly List<Task<T>> tasks = [];

    /// <summary>
    /// Asynchronously waits for the semaphore and returns the elapsed ticks.
    /// </summary>
    /// <returns>The elapsed ticks.</returns>
    protected async Task<long> AwaitSemaphoreAsync()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        await semaphore.WaitAsync();
        stopwatch.Stop();
        return stopwatch.ElapsedTicks;
    }

    /// <summary>
    /// Gets the next task ID based on the current task ID.
    /// </summary>
    /// <param name="taskId">The current task ID.</param>
    /// <returns>The next task ID or null if there are no more tasks.</returns>
    protected virtual int? GetNextTaskId(int? taskId)
    {
        if (taskId < maxTaskCount) return taskId + 1;
        else return null;
    }

    /// <summary>
    /// Manages the process asynchronously.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="taskCount">The total number of tasks.</param>
    /// <param name="waitTicks">The elapsed ticks while waiting for the semaphore.</param>
    /// <param name="semaphore">The semaphore.</param>
    /// <returns>The result of the process.</returns>
    protected async Task<T> ManageProcessAsync(int taskId, int taskCount, long waitTicks, SemaphoreSlim semaphore)
    {
        Stopwatch sw = Stopwatch.StartNew();
        T? result = default;
        try
        {
            ConcurrentProcessorModel taskData = new()
            {
                TaskId = taskId,
                TaskCount = taskCount,
                WaitTicks = waitTicks,
                SemaphoreCount = semaphore.CurrentCount,
                SemaphoreWait = waitTicks
            };

            result = await ProcessAsync(taskData);
        }
        finally
        {
            semaphore.Release();
            sw.Stop();
            if (result is not null)
                result.TaskDurationMS = sw.ElapsedMilliseconds;
        }
        return result;
    }

    /// <summary>
    /// Processes the task asynchronously.
    /// </summary>
    /// <param name="taskData">The task data.</param>
    /// <returns>The result of the process.</returns>
    protected abstract Task<T> ProcessAsync(ConcurrentProcessorModel taskData);

    /// <summary>
    /// Runs the concurrent processor asynchronously.
    /// </summary>
    /// <returns>The list of results.</returns>
    public async Task<List<T>> RunAsync()
    {
        int? taskId = 1;
        List<T> results = [];
        while (taskId is not null)
        {
            long waitTicks = await AwaitSemaphoreAsync();
            Task<T> task = ManageProcessAsync(taskId.Value, tasks.Count, waitTicks, semaphore);
            tasks.Add(task);
            taskId = GetNextTaskId(taskId);

            if (tasks.Count >= maxConcurrency)
            {
                Task<T> finishedTask = await Task.WhenAny(tasks);
                results.Add(await finishedTask);
                tasks.Remove(finishedTask);
            }
        }
        await Task.WhenAll(tasks);
        foreach (var task in tasks)
        {
            results.Add(await task); // Add the remaining task results to the list
        }
        return results;
    }
}
