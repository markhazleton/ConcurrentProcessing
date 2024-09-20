using ConcurrentProcessing.Concurrent;
using ConcurrentProcessing.Sample;
using System.Diagnostics;

// Run tests with various task counts and concurrency levels
await RunConcurrent(100, 1);
await RunConcurrent(100, 10);
await RunConcurrent(100, 50);
await RunConcurrent(100, 100);
await RunConcurrent(1000, 500);
await RunConcurrent(1000, 1000);

/// <summary>
/// Executes concurrent tasks based on the specified task count and concurrency level.
/// </summary>
/// <param name="maxTaskCount">Total number of tasks to be executed.</param>
/// <param name="maxConcurrency">Maximum number of concurrent tasks allowed.</param>
static async Task RunConcurrent(int maxTaskCount, int maxConcurrency)
{
    // Parameter validation
    if (maxTaskCount <= 0 || maxConcurrency <= 0)
    {
        Console.WriteLine("maxTaskCount and maxConcurrency must be greater than zero.");
        return;
    }

    // Start measuring execution time
    var sw = Stopwatch.StartNew();
    Console.WriteLine($"Starting {maxTaskCount} tasks with a max concurrency of {maxConcurrency}...");

    try
    {
        // Initialize the task processor with the provided settings
        var taskProcessor = new SampleTaskProcessor(maxTaskCount: maxTaskCount, maxConcurrency: maxConcurrency);

        // Execute tasks and collect results
        var results = await taskProcessor.RunAsync();

        // Convert results to a common model type for metrics calculation
        var concurrentModels = results.Cast<ConcurrentProcessorModel>().ToList();

        // Calculate and display metrics based on the task results
        DisplayMetrics(concurrentModels);
    }
    catch (Exception ex)
    {
        // Log any exceptions that occur during task execution
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

    // Stop the stopwatch and display total execution duration
    sw.Stop();
    Console.WriteLine($"Total Duration: {sw.ElapsedMilliseconds}ms");
}

/// <summary>
/// Calculates and displays performance metrics based on the provided models.
/// </summary>
/// <param name="concurrentModels">List of models representing task results.</param>
static void DisplayMetrics(List<ConcurrentProcessorModel> concurrentModels)
{
    // Calculate and display various metrics such as average execution time, success rate, etc.
    MetricCalculator.CalculateAndDisplayMetrics(concurrentModels);
}
