using System;

namespace FindMinimal
{
    class Program
    {
        private const int BenchmarkIterations = 10;
        private const int ArraySize = 100000000;

        static void Main(string[] args)
        {
            var data = ArrayGenerator.GetArrayOfIntNumbers(ArraySize);

            #region Test for async search call
            Console.WriteLine($"Test for Async search method");
            var asyncArray = new AsyncArray(data);
            var ar = asyncArray.BeginSearchMinimal(null, null);
            Console.WriteLine("Working on searching...");
            var asyncMinimal = asyncArray.EndSearchMinimal(ar);
            var classicMinimal = MinFunctionsAggregator.FindMinimalInOneThread(data);

            Console.WriteLine($"'Classic search result - {classicMinimal}. Async search result - {asyncMinimal}.'"); 
            #endregion

            #region Simple benchmark
            Console.WriteLine($"\nBenchmark for one/multi threading searches.\n");
            Benchmark.Profile($"Find minimal in one thread. Number of items: {ArraySize}.", BenchmarkIterations,
                () => MinFunctionsAggregator.FindMinimalInOneThread(data));

            Benchmark.Profile($"Find minimal in Multi Threads. Number of items: {ArraySize}.", BenchmarkIterations,
                () => MinFunctionsAggregator.FindMinimalByMultiThreading(data));
            #endregion

            #region Find optimal size for running search on multi threads
            Console.WriteLine("\nStart to calculating an optimal array size for multi threads...");
            FindOptimalArraySizeForMultiThreading(); 
            #endregion

            Console.WriteLine("Done. Press any key.");
            Console.ReadLine();
        }

        static void FindOptimalArraySizeForMultiThreading()
        {
            var minItemsNumber = 10;
            var maxItemsNumber = ArraySize;
            var processingItemsNumber = (minItemsNumber + maxItemsNumber) / 2;

            double oneThreadPerformance = 0;
            double multiThreadsPerformance = 0;

            while (maxItemsNumber - minItemsNumber > 100 )
            {
                var data = ArrayGenerator.GetArrayOfIntNumbers(processingItemsNumber);
                Console.WriteLine("***");

                oneThreadPerformance = Benchmark.Profile($"One thread search. Array size {processingItemsNumber}.",
                    BenchmarkIterations, () => MinFunctionsAggregator.FindMinimalInOneThread(data));

                multiThreadsPerformance = Benchmark.Profile($"Multi thread search. Array size {processingItemsNumber}.",
                    BenchmarkIterations, () => MinFunctionsAggregator.FindMinimalByMultiThreading(data));

                if (oneThreadPerformance < multiThreadsPerformance)
                {
                    minItemsNumber = processingItemsNumber;
                }
                else
                {
                    maxItemsNumber = processingItemsNumber;
                }

                processingItemsNumber = (minItemsNumber + maxItemsNumber) / 2;

                if ((oneThreadPerformance - multiThreadsPerformance) / BenchmarkIterations > 0 &&
                    (oneThreadPerformance - multiThreadsPerformance) / BenchmarkIterations < 3)
                {
                    break;
                }
            }

            Console.WriteLine(
                "************ \n" +
                $"The multi threads processing is faster after {processingItemsNumber} items (+/- 100) in array. " +
                $"Time to search: one thread - {oneThreadPerformance / BenchmarkIterations}, " +
                $"multi threads - {multiThreadsPerformance / BenchmarkIterations}");
        }
    }
}
