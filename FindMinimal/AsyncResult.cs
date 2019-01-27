using System;
using System.Threading;

namespace FindMinimal
{
    // Copy-Paste from demo project
    public class AsyncResult<T> : IAsyncResult, IDisposable
    {
        private readonly AsyncCallback _callback;
        private bool _completed;
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private T _result;
        private readonly object _syncRoot = new object();

        public AsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            AsyncState = state;
        }

        #region IAsyncResult Members

        public object AsyncState { get; }

        public WaitHandle AsyncWaitHandle => _waitHandle;

        public bool CompletedSynchronously => false;

        public bool IsCompleted
        {
            get
            {
                lock (_syncRoot)
                {
                    return _completed;
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _waitHandle.Dispose();
            }
        }

        public T Result
        {
            get
            {
                lock (_syncRoot)
                {
                    return _result;
                }
            }
        }

        public void Complete(T result)
        {
            lock (_syncRoot)
            {
                _completed = true;
                _result = result;
            }

            _waitHandle.Set();
            if (_callback != null)
            {
                ThreadPool.QueueUserWorkItem(state => _callback(this));
            }
        }
    }
}
