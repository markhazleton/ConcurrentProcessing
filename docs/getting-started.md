# Getting Started with ConcurrentProcessor

To use the `ConcurrentProcessor` class, set up your C# project and create a subclass that implements the abstract `ProcessAsync` method.

## Create a Subclass

```csharp
public class SampleTaskProcessor : ConcurrentProcessor<SampleTaskResult>
{
    public SampleTaskProcessor(int maxTaskCount, int maxConcurrency)
        : base(maxTaskCount, maxConcurrency)
    {
    }
}
```

## Override ProcessAsync

```csharp
protected override async Task<SampleTaskResult> ProcessAsync(ConcurrentProcessorModel taskData)
{
    await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(10, 20)));
    return new SampleTaskResult(taskData);
}
```

## Running the Processor

```csharp
var processor = new MyConcurrentProcessor(maxTaskCount, maxConcurrency);
var results = await processor.RunAsync();
```
