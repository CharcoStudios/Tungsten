﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using W;
using W.InvokeExtensions;
using W.Logging;
using W.PropertyHostExtensions;
using W.Threading;

namespace W.Demo.Winforms
{
    /// <summary>
    /// Sample Winforms form.  This form uses the InvokeEx extension method extensively.
    /// </summary>
    public partial class frmMain : Form
    {
        private readonly PersonModel _vm = new PersonModel(); //weak example of a viewmodel
        private bool _isLoading = true; //if this value could be read or modified on separate threads, you could use Lockable<bool> instead.
        private int _keySize = 2048;
        private W.Encryption.PublicPrivateKeyPair _keys;
        private bool _isBusy = false;

        //this is an example of how to use W.Threading.Gate (though there's really no need to do it this way)
        //It's also probably a little more work because we have to assign CustomData and also dispose the gate
        private readonly W.Threading.Gate<frmMain> _showSaveComplete = new Gate<frmMain>((form, cts) =>
        {
            if (form == null)
                throw new Exception("You must assign a value to the Gate's CustomData");
            form.ShowSaveComplete();
        });

        private bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                grpMain.Enabled = !value;
                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
        }

        public frmMain()
        {
            InitializeComponent();
            this.InitializeProperties();

            //_showSaveComplete.CustomData = this; //this must be called, otherwise

            //setup some event handlers
            base.Closing += (sender, args) => { _showSaveComplete.Dispose(); };

            base.Load += async (sender, args) =>
            {
                //simulate loading data
                await _vm.LoadAsync(result =>
                {
                    this.InvokeEx(f =>
                    {
                        //executes on UI thread, so it's fine
                        txtLast.Text = _vm.Last.Value;
                        txtFirst.Text = _vm.First.Value;
                        _isLoading = false;
                    });
                });
            };

            //update lblFullName whenever the value changes
            _vm.FullName.ValueChanged += (vm, oldValue, newValue) =>
                {
                    //because the flow of code does not leave the UI thread, this works fine
                    lblFullName.Text = newValue;
                };
            _vm.IsBusy.ValueChanged += (vm, oldValue, newValue) =>
            {
                //the first time IsBusy changes, it's on the UI thread, but when it changes back to false, it's not.  So we have to use InvokeEx.
                //Note that for separation of concerns, this InvokeEx code should always be performed in UI code (don't pass a reference of the form to the model) 
                this.InvokeEx(f =>
                {
                    //support a responsive window, yet disabled controls, while saving
                    IsBusy = newValue;
                    //grpMain.Enabled = !newValue;
                    //Cursor = newValue ? Cursors.WaitCursor : Cursors.Default;
                });
            };
            //trap changes to SaveProgress and update the progressbar appropriately
            _vm.SaveProgress.ValueChanged += (vm, oldValue, newValue) =>
            {
                this.InvokeEx(f =>
                {
                    pbSaveProgress.Value = newValue;
                    pbSaveProgress.Refresh();
                });
            };

            txtLast.Enter += (sender, args) => { txtLast.SelectAll(); };
            txtFirst.Enter += (sender, args) => { txtFirst.SelectAll(); };
        }
        ~frmMain()
        {
            _keys = null;
        }

        private void txtLast_TextChanged(object sender, EventArgs e)
        {
            //You could set this value when txtLast loses focus too.  Your call.
            if (!_isLoading)
                _vm.Last.Value = txtLast.Text;
        }
        private void txtFirst_TextChanged(object sender, EventArgs e)
        {
            //You could set this value when txtLast loses focus too.  Your call.
            if (!_isLoading)
                _vm.First.Value = txtFirst.Text;
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            pbSaveProgress.Visible = true;
            await _vm.SaveAsync(result =>
            {
                this.InvokeEx(f =>
                {
                    pbSaveProgress.Visible = false;
                });
            });
            //allow the _showSaveComplete gate to run
            _showSaveComplete.Run(this);
        }

        //show the Save Complete message for 3 seconds
        private void ShowSaveComplete()
        {
            this.InvokeEx(f =>
            {
                lblSaveComplete.Visible = true;
            });
            System.Threading.Thread.Sleep(3000);
            this.InvokeEx(f =>
            {
                lblSaveComplete.Visible = false;
            });
        }

        private void CreateRSA()
        {
            if (_keys == null)
            {
                _keys = W.Encryption.RSAMethods.CreateKeyPair(_keySize);// new Encryption.RSA(2048);
            }
        }
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                CreateRSA();
                var cipher = W.Encryption.RSAMethods.Encrypt(txtEncryption.Text, _keys.PublicKey);
                txtEncryption.Text = cipher;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                CreateRSA();
                var text = W.Encryption.RSAMethods.Decrypt(txtEncryption.Text, _keys.PrivateKey);
                txtEncryption.Text = text;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
