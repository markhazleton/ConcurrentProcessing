namespace ConcurrentProcessing.Concurrent;

public static class MetricCalculator
{
    /// <summary>
    /// Calculates and displays the metrics for the given list of ConcurrentProcessorModel objects.
    /// </summary>
    /// <param name="models">The list of ConcurrentProcessorModel objects.</param>
    public static void CalculateAndDisplayMetrics(List<ConcurrentProcessorModel> models)
    {
        if (models == null || models.Count == 0)
        {
            Console.WriteLine("No data to calculate metrics.");
            return;
        }

        // Calculate and display metrics for each property
        CalculateAndDisplayMetric(models, "TaskCount", m => m.TaskCount);
        CalculateAndDisplayMetric(models, "WaitTicks", m => m.WaitTicks);
        CalculateAndDisplayMetric(models, "SemaphoreCount", m => m.SemaphoreCount);
        CalculateAndDisplayMetric(models, "SemaphoreWait", m => m.SemaphoreWait);
        CalculateAndDisplayMetric(models, "TaskDuration", m => m.TaskDurationMS);
    }

    /// <summary>
    /// Calculates and displays the metric for the specified property of the ConcurrentProcessorModel objects.
    /// </summary>
    /// <param name="models">The list of ConcurrentProcessorModel objects.</param>
    /// <param name="metricName">The name of the metric.</param>
    /// <param name="metricSelector">The selector function to extract the metric value from a ConcurrentProcessorModel object.</param>
    private static void CalculateAndDisplayMetric(
        List<ConcurrentProcessorModel> models,
        string metricName,
        Func<ConcurrentProcessorModel, long> metricSelector)
    {
        var metricValues = models.Select(metricSelector).ToList();

        if (metricValues.Count == 0)
        {
            Console.WriteLine($"No data for {metricName}.");
            return;
        }
        long min = metricValues.Min();
        long max = metricValues.Max();
        double average = metricValues.Average();
        Console.WriteLine($"{metricName,-20}\tMinimum: {min}\tMaximum: {max}\tAverage: {average:F2}");
    }
}
