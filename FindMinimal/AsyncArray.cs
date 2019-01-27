using System;
using System.Threading;

namespace FindMinimal
{
    public class AsyncArray
    {
        private readonly int[] _data;

        public AsyncArray(int[] array)
        {
            _data = array;
        }

        public IAsyncResult BeginSearchMinimal(AsyncCallback callback, object state)
        {
            var ar = new AsyncResult<int>(callback, state);
            var th = new Thread(() =>
            {
                var result = MinFunctionsAggregator.FindMinimalInOneThread(_data);
                ar.Complete(result);
            });

            th.Start();
            return ar;
        }

        public int EndSearchMinimal(IAsyncResult asyncResult)
        {
            var ar = (AsyncResult<int>)asyncResult;
            ar.AsyncWaitHandle.WaitOne();
            return ar.Result;
        }
    }
}
