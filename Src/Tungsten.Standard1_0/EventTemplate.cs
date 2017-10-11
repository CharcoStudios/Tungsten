﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W
{
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    public class EventTemplate
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        /// <param name="args">An array of untyped arguments</param>
        public delegate void EventDelegate(object sender, string callerMemberName, params object[] args);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        /// <param name="args">An array of untyped arguments</param>
        public void Raise(object sender, string callerMemberName = "", params object[] args)
        {
            var evt = OnRaised;
            evt?.Invoke(sender, callerMemberName, args);
        }
    }

    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TEventArg">The detailed event argument</typeparam>
    public class EventTemplate<TEventArg>
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg">The detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public delegate void EventDelegate(object sender, TEventArg arg, string callerMemberName);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg">The detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public void Raise(object sender, TEventArg arg, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName="")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg, callerMemberName);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    /// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    public class EventTemplate<TEventArg1, TEventArg2>
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public delegate void EventDelegate(object sender, TEventArg1 arg1, TEventArg2 arg2, string callerMemberName);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, callerMemberName);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    /// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    /// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    public class EventTemplate<TEventArg1, TEventArg2, TEventArg3>
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public delegate void EventDelegate(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, string callerMemberName);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, callerMemberName);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    /// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    /// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    /// <typeparam name="TEventArg4">The fourth detailed event argument</typeparam>
    public class EventTemplate<TEventArg1, TEventArg2, TEventArg3, TEventArg4>
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="arg4">The fourth detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public delegate void EventDelegate(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, string callerMemberName);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="arg4">The fourth detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, arg4, callerMemberName);
        }
    }
    /// <summary>
    /// Wraps the functionality of delegate, event and RaiseXXX into a single class
    /// </summary>
    /// <typeparam name="TEventArg1">The first detailed event argument</typeparam>
    /// <typeparam name="TEventArg2">The second detailed event argument</typeparam>
    /// <typeparam name="TEventArg3">The third detailed event argument</typeparam>
    /// <typeparam name="TEventArg4">The fourth detailed event argument</typeparam>
    /// <typeparam name="TEventArg5">The fifth detailed event argument</typeparam>
    public class EventTemplate<TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>
    {
        /// <summary>
        /// The template delegate
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="arg4">The fourth detailed event argument</param>
        /// <param name="arg5">The fifth detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public delegate void EventDelegate(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, TEventArg5 arg5, string callerMemberName);
        /// <summary>
        /// The template event
        /// </summary>
        public event EventDelegate OnRaised;
        /// <summary>
        /// Raises the template event
        /// </summary>
        /// <param name="sender">The object which raised this event</param>
        /// <param name="arg1">The first detailed event argument</param>
        /// <param name="arg2">The second detailed event argument</param>
        /// <param name="arg3">The third detailed event argument</param>
        /// <param name="arg4">The fourth detailed event argument</param>
        /// <param name="arg5">The fifth detailed event argument</param>
        /// <param name="callerMemberName">The name of the method which raised the event</param>
        public void Raise(object sender, TEventArg1 arg1, TEventArg2 arg2, TEventArg3 arg3, TEventArg4 arg4, TEventArg5 arg5, [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            var evt = OnRaised;
            evt?.Invoke(sender, arg1, arg2, arg3, arg4, arg5, callerMemberName);
        }
    }
}
