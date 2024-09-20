namespace ConcurrentProcessing.Concurrent;

/// <summary>
/// Represents a concurrent processor model.
/// </summary>
public class ConcurrentProcessorModel
{
    /// <summary>
    /// Gets or sets the task ID.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the task count.
    /// </summary>
    public int TaskCount { get; set; }

    /// <summary>
    /// Gets or sets the wait ticks.
    /// </summary>
    public long WaitTicks { get; set; }

    /// <summary>
    /// Gets or sets the semaphore count.
    /// </summary>
    public int SemaphoreCount { get; set; }

    /// <summary>
    /// Gets or sets the semaphore wait.
    /// </summary>
    public long SemaphoreWait { get; set; }

    /// <summary>
    /// Gets or sets the task duration in milliseconds.
    /// </summary>
    public long TaskDurationMS { get; set; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string? ToString()
    {
        return $"Task:{TaskId:D3} DurationMS:{TaskDurationMS:D5} WaitTicks:{WaitTicks:D5} TaskList:{TaskCount:D2} SemaphoreCount:{SemaphoreCount:D2} SemaphoreWait:{SemaphoreWait:D4}";
    }
}

