using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindMinimal
{
    public static class MinFunctionsAggregator
    {
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
                FindMinimalDelegateData delegateData;

                if (i != coresNumber - 1)
                {
                    delegateData = new FindMinimalDelegateData()
                    {
                        SourceArray = data,
                        StartIndex = i * itemsPerCore,
                        EndIndex = (i + 1) * itemsPerCore,
                        ResultsStorage = threadsCalculationsResult,
                        ThreadIndex = i
                    };
                }
                else
                {
                    delegateData = new FindMinimalDelegateData()
                    {
                        SourceArray = data,
                        StartIndex = i * itemsPerCore,
                        EndIndex = i * itemsPerCore + itemsForLastCore,
                        ResultsStorage = threadsCalculationsResult,
                        ThreadIndex = i
                    };
                }

                threads[i] = new Thread(FindMinimalInPartOfArray);
                threads[i].Start(delegateData);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return FindMinimalInOneThread(threadsCalculationsResult);
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

    internal class FindMinimalDelegateData
    {
        public int[] SourceArray { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int[] ResultsStorage { get; set; }
        public int ThreadIndex { get; set; }
    }

}
