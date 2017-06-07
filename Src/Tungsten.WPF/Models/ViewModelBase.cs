﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W;
using W.Logging;
using W.WPF.Core;

namespace W.WPF.Models
{
    /// <summary>
    /// The base class for W.WPF PageFramework view models
    /// </summary>
    public class ViewModelBase : DependencyObjectBase /*, IModel , ISaveCommander, IRefreshCommander*/
    {
        //#region IBusy
        ///// <summary>
        ///// Get or set the IsBusy flag
        ///// </summary>
        //public Property<ModelBase, bool> IsBusy { get; } = new Property<ModelBase, bool>((m, oldValue, newValue) => { m.BusyVisibility.Value = newValue ? Visibility.Visible : Visibility.Hidden; });
        ///// <summary>
        ///// Get or set a title string which can be displayed while busy
        ///// </summary>
        //public Property<string> BusyTitle { get; } = new Property<string>("Loading");
        ///// <summary>
        ///// Get or set a message to display while busy
        ///// </summary>
        //public Property<string> BusyMessage { get; } = new Property<string>("");
        ///// <summary>
        ///// Visible if IsBusy.Value is True, otherwise Hidden
        ///// </summary>
        //public Property<System.Windows.Visibility> BusyVisibility { get; } = new Property<System.Windows.Visibility>(Visibility.Hidden);
        //#endregion

        /// <summary>
        /// Information related to this model being busy
        /// </summary>
        public BusyIndication BusyIndication { get; } = new BusyIndication();
        /// <summary>
        /// Everything needs a title
        /// </summary>
        public Property<string> Title { get; } = new Property<string>("");

        /// <summary>
        /// Called immediately upon object creation.  Place initialization code here.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }
        /// <summary>
        /// Called instead of OnCreate if the code is running in DesignMode
        /// </summary>
        protected override void OnCreateInDesignMode()
        {
            base.OnCreateInDesignMode();
        }
        /// <summary>
        /// Called immediately after OnInitialize (when no in DesignMode)
        /// </summary>
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        /// <summary>
        /// Constructs a new ViewModel
        /// </summary>
        public ViewModelBase() : base()
        {
            Log.i("{0}.InitializeProperties()", this.GetType().Name);
            this.InitializeProperties();
        }
    }
}
