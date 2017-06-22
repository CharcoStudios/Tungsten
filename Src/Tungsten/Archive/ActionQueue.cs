﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace W
{
    /// <summary><para>
    /// Allows the programmer to enqueue items for processing on a separate thread.  The ActionQueue will process items sequentially whenever an item is added.
    /// </para></summary>
    /// <typeparam name="T">The type of data to enqueue and process</typeparam>
    public class ActionQueue<T> : IDisposable
    {
        private string _caller;

        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _threadReleaser = new ManualResetEvent(false);

        /// <summary>
        /// Returns the number of items currently in the queue
        /// </summary>
        public int Count => _queue.Count;

        /// <summary>
        /// Places an item in the queue
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }
        /// <summary>
        /// Cancels processing of the queue
        /// </summary>
        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Releases resources related to the ActionQueue
        /// </summary>
        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            _threadReleaser?.Dispose();
        }

        /// <summary>
        /// The reference to the ConcurrentQueue being used
        /// </summary>
        public ConcurrentQueue<T> Queue
        {
            get
            {
                return _queue;
            }
        }

        /// <summary>
        /// Creates a new ActionQueue
        /// </summary>
        /// <param name="onItemAdded">A callback which is called whenever an item has been enqueued</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        public ActionQueue(Func<T, bool> onItemAdded, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            _caller = caller;
            W.Threading.Thread.Create((cts) =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    //is this the same as System.Threading.Thread.Sleep(0)?
                    _threadReleaser.WaitOne(0);
                    //Task.Delay(0); 
                    while (_queue.Count > 0)
                    {
                        T item;
                        if (_queue.TryDequeue(out item))
                        {
                            if (!onItemAdded(item))
                                _queue.Enqueue(item);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Creates a new ActionQueue
        /// </summary>
        /// <param name="onItemAdded">A callback which is called whenever an item has been enqueued</param>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        public ActionQueue(Action<T> onItemAdded, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            _caller = caller;
            W.Threading.Thread.Create((cts) =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    //is this the same as System.Threading.Thread.Sleep(0)?
                    _threadReleaser.WaitOne(0);
                    //Task.Delay(0); 
                    while (_queue.Count > 0)
                    {
                        T item;
                        if (_queue.TryDequeue(out item))
                        {
                            onItemAdded(item);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Disposes the ActionQueue
        /// </summary>
        ~ActionQueue()
        {
            Dispose();
        }
    }
}
