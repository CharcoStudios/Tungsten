﻿using System;
using System.Collections.Generic;
using System.Text;

namespace W.Logging
{
    /// <summary>
    /// Allows the programmer to add a custom message logger
    /// </summary>
    public class CustomLogger : IDisposable
    {
        private LockableSlim<bool> _isDisposed = new LockableSlim<bool>(false);
        /// <summary>
        /// The name of this custom logger
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// If true, FormatLogMessage will, by default, add a timestamp prefix to the log message
        /// </summary>
        /// <remarks>Note that if FormatLogMessage is overridden, this functionality may be overridden</remarks>
        public bool AddTimestamp { get; private set; }
        /// <summary>
        /// True if OnDispose has been called
        /// </summary>
        protected bool IsDisposed => _isDisposed.Value;

        /// <summary>
        /// Formats the Log Messge (if AddTimestamp is true, the message is prefixed with a timestamp)
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        /// <returns>The formatted log message</returns>
        protected virtual string FormatLogMessage(Log.LogMessageCategory category, string message)
        {
            if (AddTimestamp)
                message = string.Format("{0}: {1} - {2}", DateTime.Now.TimeOfDay.ToString(), category.ToString(), message);
            else
                message = string.Format("{0} - {1}", category.ToString(), message);
            return message;
        }
        /// <summary>
        /// Log a message to the custom logger
        /// </summary>
        /// <param name="category">The log message category</param>
        /// <param name="message">The log message</param>
        protected virtual void LogMessage(W.Logging.Log.LogMessageCategory category, string message)
        {
        }
        /// <summary>
        /// Disposes the CustomLogger, releases resources and supresses the finalizer
        /// </summary>
        protected virtual void OnDispose()
        {
            _isDisposed.InLock(value =>
            {
                if (!value)
                {
                    W.Logging.Log.LogTheMessage -= LogMessage;
                    GC.SuppressFinalize(this);
                }
                //return true;
            });
        }

        /// <summary>
        /// Disposes the CustomLogger and releases resources
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Constructs a new CustomLogger
        /// </summary>
        /// <param name="name">The custom logger name</param>
        /// <param name="addTimestamp">If True the log messages will be prefixed with a timestamp</param>
        public CustomLogger(string name, bool addTimestamp)
        {
            Name = name;

            AddTimestamp = addTimestamp;
            W.Logging.Log.LogTheMessage += LogMessage;
        }
    }
}
