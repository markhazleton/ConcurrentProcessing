using ConcurrentProcessing.Concurrent;

namespace ConcurrentProcessing.Sample;

/// <summary>
/// Sample task processor implementation for testing concurrent processing.
/// </summary>
public sealed class SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
    : ConcurrentProcessor<SampleTaskResult>(maxTaskCount, maxConcurrency)
{
    private static readonly Random Random = new();

    /// <summary>
    /// Processes a task asynchronously with a random delay.
    /// </summary>
    /// <param name="taskData">The task data containing execution context.</param>
    /// <returns>A sample task result.</returns>
    protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(10, 20)));
        return new SampleTaskResult(taskData);
    }
}
