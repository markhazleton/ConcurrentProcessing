namespace ConcurrentProcessing.Concurrent;

/// <summary>
/// Represents a concurrent processor model with execution metrics.
/// </summary>
public class ConcurrentProcessorModel
{
    /// <summary>
    /// Gets or init the task ID.
    /// </summary>
    public int TaskId { get; init; }

    /// <summary>
    /// Gets or init the task count.
    /// </summary>
    public int TaskCount { get; init; }

    /// <summary>
    /// Gets or init the wait ticks.
    /// </summary>
    public long WaitTicks { get; init; }

    /// <summary>
    /// Gets or init the semaphore count.
    /// </summary>
    public int SemaphoreCount { get; init; }

    /// <summary>
    /// Gets or init the semaphore wait.
    /// </summary>
    public long SemaphoreWait { get; init; }

    /// <summary>
    /// Gets or sets the task duration in milliseconds.
    /// Set by the framework after task execution completes.
    /// </summary>
    public long TaskDurationMS { get; set; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() =>
        $"Task:{TaskId:D3} DurationMS:{TaskDurationMS:D5} WaitTicks:{WaitTicks:D5} TaskList:{TaskCount:D2} SemaphoreCount:{SemaphoreCount:D2} SemaphoreWait:{SemaphoreWait:D4}";
}

