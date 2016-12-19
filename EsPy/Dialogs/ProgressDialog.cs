using EsPy.Units;
using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Dialogs
{
    public partial class ProgressDialog : Form, IDisposable
    {
        public enum Modes { Upload, Download};

        public ProgressDialog()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Port != null)
                this.Port = null;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Modes FMode = Modes.Upload;
        public Modes Mode
        { get { return this.FMode; }
            set
            {
                this.FMode = value;
                this.Text = value.ToString();
            }
        }

        private string FFileName = "";
        public string FileName
        {
            get { return this.FFileName; }
            set
            {
                this.FFileName = value;
                this.label1.Text = value;
            }
        }

        public byte[] Buffer
        { get; set; }

        private PySerial FPort = null;
        public PySerial Port
        {
            get { return this.FPort; }
            set
            {
                if (this.FPort != null)
                {
                    this.FPort.FileProgress -= Port_FileProgress;
                }

                this.FPort = value;
                if(this.FPort != null)
                {
                    this.FPort.FileProgress += Port_FileProgress;
                }
            }
        }

        private void Port_FileProgress(object sender, FileProgressEventArgs e)
        {
            this.label1.Text = e.FName + ": " + e.Bytes.ToString() + " / " + e.Size.ToString();
            this.progressBar1.Maximum = e.Size;
            if (e.Bytes > e.Size)
                e.Bytes = e.Size;
            this.progressBar1.Value = e.Bytes;
            this.progressBar1.Invalidate();
            Application.DoEvents();
        }

        private void ProgressDialog_Shown(object sender, EventArgs e)
        {
            ResultStatus res = null;
            DialogResult dres = DialogResult.Abort;
            try
            {
                if (this.Mode == Modes.Upload)
                {
                    res = this.Port.Upload(this.FileName, this.Buffer);
                    if (res.Result == ResultStatus.Statuses.Success) ;
                        dres = DialogResult.OK;
                }
                else
                {
                    res = this.Port.Download(this.FileName);
                    if (res.Result == ResultStatus.Statuses.Success)
                    {
                        this.Buffer = res.Data as Byte[];
                        dres = DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorBox(ex);
            }
            finally
            {
                this.DialogResult = dres;
            }
        }
    }
}
