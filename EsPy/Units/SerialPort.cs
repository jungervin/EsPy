using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Units
{
    public class SerialPort 
    {
        private System.IO.Ports.SerialPort SP = null;

        public delegate void PortDataReceivedEvent(object sender, string data);
        public delegate void PortErrorReceivedEvent(object sender, string data);
        public delegate void PortOpenEvent(object sender, EventArgs e);
        public delegate void PortCloseEvent(object sender, EventArgs e);
        public delegate void PortBusyEvent(object sender, EventArgs e);
        public delegate void PortFreeEvent(object sender, EventArgs e);

        public event PortDataReceivedEvent DataReceived = null;
        public event PortErrorReceivedEvent ErrorReceived = null;
        public event PortOpenEvent PortOpen = null;
        public event PortCloseEvent PortClose = null;
        public event PortBusyEvent PortBusy = null;
        public event PortFreeEvent PortFree = null;

        public const string PROMPT = ">>> ";
        public const string DOTS = "... ";
        //private char C0 = '\0';
        //private char C1 = '\0';
        //private char C2 = '\0';
        //private char C3 = '\0';

        public SerialPort()
        {
            this.SP = new System.IO.Ports.SerialPort();
            this.Sync(false);
        }

        public void Dispose()
        {
            //if (this.IsOpen)
            //{
            //    try
            //    {
            //        this.Sync(false);
            //        this.Close();
            //    }
            //    catch (Exception)
            //    { }
            //}
        }

        public string PortName
        {
            get { return this.SP.PortName; }
            set { this.SP.PortName = value; }
        }

        public int BaudRate
        {
            get { return this.SP.BaudRate; }
            set { this.SP.BaudRate = value; }
        }

        public int ReadTimeout
        {
            get { return this.SP.ReadTimeout; }
            set { this.SP.ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return this.SP.WriteTimeout; }
            set { this.SP.WriteTimeout = value; }
        }

        public string LineSeparator
        {
            get { return this.SP.NewLine; }
            set { this.SP.NewLine = value; }
        }

        public bool IsOpen
        {
            get
            {
                return this.SP.IsOpen;
            }
        }

        public void Open()
        {
            try
            {
                if (this.SP.IsOpen)
                {
                    return;
                }
                this.SP.Open();
                if (this.PortOpen != null)
                {
                    this.PortOpen(this, new EventArgs());
                }
                
            }
            catch (Exception ex)
            {
                Utility.Helpers.ErrorBox(ex);
            }
        }

        public void Close()
        {
            try
            {
                if (this.IsOpen)
                {
                    
                    if (this.PortClose != null)
                    {
                        this.PortClose(this, new EventArgs());
                    }
                    this.SP.Close();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorBox(ex);
            }
        }

        public void SetDTR(bool value)
        {
            this.SP.DtrEnable = value;
        }

        public void SetRTS(bool value)
        {
            this.SP.RtsEnable = value;
        }

        private bool FSync = false;
        public void Sync(bool sync)
        {
            this.FSync = sync;
            if (sync)
            {
                this.SP.DataReceived -= Sp_DataReceived;
                this.SP.ErrorReceived -= Sp_ErrorReceived;
                this.SP.PinChanged -= Sp_PinChanged;
            }
            else
            {
                this.SP.DataReceived += Sp_DataReceived;
                this.SP.ErrorReceived += Sp_ErrorReceived;
                this.SP.PinChanged += Sp_PinChanged;
            }
        }

        private void Sp_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            //Todo:
        }

        private void Sp_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            if (this.ErrorReceived != null)
                this.ErrorReceived(this, e.EventType.ToString());
        }

        private bool FBusy = false;
        public bool Busy
        { get { return this.FBusy; }
            set
            {
                if (this.FBusy != value)
                {
                    this.FBusy = value;
                    if (FBusy)
                    {
                        if (this.PortBusy != null)
                            this.PortBusy(this, new EventArgs());
                    }
                    else
                    {
                        if (this.PortFree != null)
                            this.PortFree(this, new EventArgs());
                    }
                }
            }
        }

        private void Sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string rec = "";
            try
            {
                rec += this.ReadExisting();
            }
            catch
            { }
            finally
            {
                if (this.DataReceived != null)
                    this.DataReceived(sender, rec);
            }
        }

        public void ReadChar(int n = 1)
        {
            this.Busy = true;
            this.SP.ReadChar();
        }

        public void Write(byte[] buff, int offset, int count)
        {
            this.Busy = true;
            this.SP.Write(buff, offset, count);
        }

        public void Write(byte data)
        {
            this.Busy = true;
            this.SP.Write(new byte[] { data }, 0, 1);
        }

        public void Write(string text)
        {
            this.Busy = true;
            this.SP.Write(text);
        }

        public void WriteLine(string line)
        {
            this.Busy = true;
            try
            {
                this.SP.WriteLine(line);
            }
            catch (Exception ex)
            {
                Helpers.ErrorBox(ex);
            }
        }

        private char[] FPrompt = new char[] { '\0', '\0', '\0', '\0' };
        private void PromptShift(char c)
        {
            //for (int i = 0; i < this.FPrompt.Length - 1; i++)
            //    this.FPrompt[i] = this.FPrompt[i + 1];

            //this.FPrompt[this.FPrompt.Length - 1] = c;

            this.FPrompt[0] = this.FPrompt[1];
            this.FPrompt[1] = this.FPrompt[2];
            this.FPrompt[2] = this.FPrompt[3];
            this.FPrompt[3] = c;
        }

        private void Prompt(string data)
        {
            int len = Math.Min(this.FPrompt.Length, data.Length);
            int p = data.Length - len;
            for (int i = 0; i < len; i++)
            {
                this.PromptShift(data[p + i]);
            }

            this.Busy = this.FPrompt[0] != PROMPT[0]
                || this.FPrompt[1] != PROMPT[1]
                || this.FPrompt[2] != PROMPT[2]
                || this.FPrompt[3] != PROMPT[3];
           
        }

        public string ReadExisting()
        {
            this.Busy = true;
            string part = this.SP.ReadExisting();
            this.Prompt(part);
            return part;
        }

        public void DiscardInBuffer()
        {
            this.SP.DiscardInBuffer();
        }

        public void DiscardOutBuffer()
        {
            this.SP.DiscardOutBuffer();
        }

        public string ReadLine()
        {          
            string line = this.SP.ReadLine();
            this.Busy = line.EndsWith(PROMPT);
            return line;
        }
    }
}
