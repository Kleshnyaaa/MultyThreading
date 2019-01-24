using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMinimal
{
    class Program
    {
        static void Main(string[] args)
        {
            const int arraySize = 100000000;

            var data = ArrayGenerator.GetArrayOfIntNumbers(arraySize);

            Benchmark.Profile($"Find minimal in one thread. Number of items: {arraySize}.", 20,
                () => MinFunctionsAggregator.FindMinimalInOneThread(data));

            Benchmark.Profile($"Find minimal in Multi Threads. Number of items: {arraySize}.", 20,
                () => MinFunctionsAggregator.FindMinimalByMultiThreading(data));

            //Console.WriteLine($"Minimal in one thread is {MinFunctionsAggregator.FindMinimalInOneThread(data)}");
            //Console.WriteLine($"Minimal in MultiThreading is {MinFunctionsAggregator.FindMinimalByMultiThreading(data)}");

            Console.ReadLine();
        }
    }
}
