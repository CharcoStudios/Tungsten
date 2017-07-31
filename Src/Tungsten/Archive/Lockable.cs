﻿using System;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// <para>
    /// Provides thread safety via locking
    /// </para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Lockable<TValue>
    {
        private readonly object _lockObject = new object();
        private TValue _value;

        /// <summary>
        /// The object used internally for lock statements
        /// </summary>
        public object LockObject => _lockObject;

        /// <summary>
        /// <para>
        /// Provides automatic locking during read/writes
        /// </para>
        /// </summary>
        public TValue Value
        {
            get
            {
                lock (_lockObject)
                {
                    return _value;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// <para>
        /// To be used by caller, with LockObject, to batch read/writes under one lock)
        /// </para>
        /// </summary>
        public TValue UnlockedValue
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Executes an action within a lock of the LockObject
        /// </summary>
        /// <param name="action">The action to call within a lock</param>
        public void ExecuteInLock(Action<TValue> action)
        {
            ExecuteInLockAsync(action).Wait();
        }
        /// <summary>
        /// Executes an action within a lock of the LockObject
        /// </summary>
        /// <param name="function">The function to call within a lock</param>
        public void ExecuteInLock(Func<TValue, TValue> function)
        {
            ExecuteInLockAsync(function).Wait();
        }
        /// <summary>
        /// Executes a task within a lock of the LockObject
        /// </summary>
        /// <param name="action">The action to call within a lock</param>
        public async Task ExecuteInLockAsync(Action<TValue> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    action?.Invoke(Value);
                }
            });
        }
        /// <summary>
        /// Executes a task within a lock of the LockObject
        /// </summary>
        /// <param name="function">The function to call within a lock</param>
        public async Task ExecuteInLockAsync(Func<TValue, TValue> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    Value = function.Invoke(Value);
                }
            });
        }

        /// <summary>
        /// <para>
        /// Constructor which initializes Value with the default of TValue
        /// </para>
        /// </summary>
        public Lockable() : this(default(TValue))
        {
        }
        /// <summary>
        /// Constructor which initializes Value with the specified value
        /// </summary>
        /// <param name="value">The initial value for Value</param>
        public Lockable(TValue value)
        {
            Value = value;
        }
    }
}
