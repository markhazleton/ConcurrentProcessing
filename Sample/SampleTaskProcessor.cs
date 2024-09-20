using ConcurrentProcessing.Concurrent;

namespace ConcurrentProcessing.Sample;
public class SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
    : ConcurrentProcessor<SampleTaskResult>(maxTaskCount, maxConcurrency)
{
    protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(10, 20)));
        return new SampleTaskResult(taskData);
    }
}
