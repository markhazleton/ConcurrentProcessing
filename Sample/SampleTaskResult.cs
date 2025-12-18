using ConcurrentProcessing.Concurrent;

namespace ConcurrentProcessing.Sample;

/// <summary>
/// Represents a sample task result extending the base concurrent processor model.
/// </summary>
/// <param name="model">The base concurrent processor model to initialize from.</param>
public sealed class SampleTaskResult(ConcurrentProcessorModel model) : ConcurrentProcessorModel
{
    /// <inheritdoc/>
    public new int TaskId { get; init; } = model.TaskId;

    /// <inheritdoc/>
    public new int TaskCount { get; init; } = model.TaskCount;

    /// <inheritdoc/>
    public new long WaitTicks { get; init; } = model.WaitTicks;

    /// <inheritdoc/>
    public new int SemaphoreCount { get; init; } = model.SemaphoreCount;

    /// <inheritdoc/>
    public new long SemaphoreWait { get; init; } = model.SemaphoreWait;
}
