using System;
using System.Diagnostics;

namespace FindMinimal
{
    public static class Benchmark
    {
        public static double Profile(string description, int iterations, Action method)
        {
            // warm up
            method();

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                method();
            }
            stopWatch.Stop();

            Console.WriteLine(description);
            Console.WriteLine($"Iterations: {iterations}. Total time ms: {stopWatch.Elapsed.TotalMilliseconds}. Average: {stopWatch.Elapsed.TotalMilliseconds / iterations}");

            return stopWatch.Elapsed.TotalMilliseconds;
        }
    }
}
