using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FindMinimal
{
    public static class MinFunctionsAggregator
    {
        private static object Locker = new object();

        public static int FindMinimalInOneThread(int[] data)
        {
            var min = data[0];

            foreach (var i in data)
            {
                if (min > i)
                {
                    min = i;
                }
            }

            return min;
        }

        public static int FindMinimalByMultiThreading(int[] data)
        {
            var coresNumber = Environment.ProcessorCount;
            var itemsPerCore = data.Length / coresNumber;
            var itemsForLastCore = itemsPerCore + data.Length % coresNumber;

            var threadsCalculationsResult = new int[coresNumber];
            var threads = new Thread[coresNumber];

            for (int i = 0; i < coresNumber; i++)
            {
                var endIndex = (i != coresNumber - 1) ? (i + 1) * itemsPerCore : i * itemsPerCore + itemsForLastCore;
                var delegateData = new FindMinimalDelegateData
                {
                    SourceArray = data,
                    StartIndex = i * itemsPerCore,
                    EndIndex = endIndex,
                    ResultsStorage = threadsCalculationsResult,
                    ThreadIndex = i
                };

                threads[i] = new Thread(FindMinimalInPartOfArray);
                threads[i].Start(delegateData);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return FindMinimalInOneThread(threadsCalculationsResult);
        }

        public static int FindMinimalByThreadPool(int[] data)
        {
            var coresNumber = Environment.ProcessorCount;
            var itemsPerCore = data.Length / coresNumber;
            var itemsForLastCore = itemsPerCore + data.Length % coresNumber;

            var countdown = new CountdownEvent(coresNumber);
            var results = new List<int>();

            for (int i = 0; i < coresNumber; i++)
            {
                var endIndex = (i != coresNumber - 1) ? (i + 1) * itemsPerCore : i * itemsPerCore + itemsForLastCore;
                var delegateData = new ThreadPoolDelegateData
                {
                    SourceArray = data,
                    StartIndex = i * itemsPerCore,
                    EndIndex = endIndex,
                    ResultsStorage = results,
                    CountdownEvent = countdown
                };

                ThreadPool.QueueUserWorkItem(FindMinimalInPartOfArray_ThreadPool, delegateData);
            }

            countdown.Wait();

            return results.Min();
        }

        private static void FindMinimalInPartOfArray_ThreadPool(object delegateDataObject)
        {
            var delegateData = (ThreadPoolDelegateData) delegateDataObject;
            var min = delegateData.SourceArray[delegateData.StartIndex];

            for (int i = delegateData.StartIndex; i < delegateData.EndIndex; i++)
            {
                if (min > delegateData.SourceArray[i])
                {
                    min = delegateData.SourceArray[i];
                }
            }

            lock (Locker)
            {
                delegateData.ResultsStorage.Add(min); 
            }

            delegateData.CountdownEvent.Signal();
        }

        private static void FindMinimalInPartOfArray(object delegateDataObject)
        {
            var delegateData = (FindMinimalDelegateData) delegateDataObject;

            var min = delegateData.SourceArray[delegateData.StartIndex];

            for (int i = delegateData.StartIndex; i < delegateData.EndIndex; i++)
            {
                if (min > delegateData.SourceArray[i])
                {
                    min = delegateData.SourceArray[i];
                }
            }

            delegateData.ResultsStorage[delegateData.ThreadIndex] = min;
        }
    }
}
