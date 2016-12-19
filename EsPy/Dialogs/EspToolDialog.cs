using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Dialogs
{
    public partial class EspToolDialog : Form
    {
        public EspToolDialog()
        {
            InitializeComponent();

            this.BaudRate = Properties.Settings.Default.EspToolBaud;
            this.cbBaudrate.Text = this.BaudRate.ToString();
            this.tbPython.Text = Properties.Settings.Default.PythonExe;
            if (tbPython.Text == "")
                this.tbPython.Text = Helpers.GetPythonPath();

            this.tbEsptool.Text = Properties.Settings.Default.EspToolPy;
            this.tbFirmware.Text = Properties.Settings.Default.FrimwareBin;

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            if (ports != null)
            {
                this.cbPort.Items.AddRange(ports);
            }
        }

        public string PortName
        {
            get
            {
                if (this.cbPort.SelectedItem != null)
                    return this.cbPort.SelectedItem.ToString();

                return "";
            }
            set
            {
                this.cbPort.Text = value;
            }
        }

        private void btnPython_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "python.exe|python.exe";
            if (this.tbPython.Text != "" && Directory.Exists(Path.GetDirectoryName(this.tbPython.Text)))
                d.InitialDirectory = Path.GetDirectoryName(this.tbPython.Text);

            if (d.ShowDialog() == DialogResult.OK)
            {
                this.tbPython.Text = d.FileName;
            }
            d.Dispose();
        }

        private void btnEsptool_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "esptool.py|esptool.py";
            if (this.tbEsptool.Text != "" && Directory.Exists(Path.GetDirectoryName(this.tbEsptool.Text)))
                d.InitialDirectory = Path.GetDirectoryName(this.tbEsptool.Text);

            if (d.ShowDialog() == DialogResult.OK)
            {
                this.tbEsptool.Text = d.FileName;
            }
            d.Dispose();
        }

        private void btnFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "firmware.bin|*.bin";
            if (this.tbFirmware.Text != "" && Directory.Exists(Path.GetDirectoryName(this.tbFirmware.Text)))
                d.InitialDirectory = Path.GetDirectoryName(this.tbFirmware.Text);

            if (d.ShowDialog() == DialogResult.OK)
            {
                this.tbFirmware.Text = d.FileName;
            }
            d.Dispose();
        }

         private string Run(string cmd, string args)
        {
            this.textBox4.Text = "Please wait...\r\n";
            
            this.Enabled = false;
            Application.DoEvents();

            try
            {
                System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo(
                    cmd, "" + args);
                inf.RedirectStandardOutput = true;
                inf.RedirectStandardError = true;

                inf.UseShellExecute = false;
                inf.CreateNoWindow = true;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                //proc.OutputDataReceived += Proc_OutputDataReceived;
                //proc.ErrorDataReceived += Proc_ErrorDataReceived;
                proc.StartInfo = inf;

                proc.Start();
                string res = "";
                res = proc.StandardOutput.ReadToEnd();
                res += proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                while (!proc.HasExited)
                {
                    Thread.Sleep(100);
                }

                proc.Close();
                proc.Dispose();

                for (int i = 1; i < res.Length;)
                {
                    if (res[i]  < 10)
                    {                   
                        res = res.Remove(i - 1 , 2);
                        i--;
                    }
                    else i++;
                }

                return res;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                this.Enabled = true;
            }
        }


       //public delegate void DataReceivedEventHandler(string data);
       // private void UpdateUI(string data)
       // {
       //     if (this.InvokeRequired)
       //     {
       //         this.Invoke(new DataReceivedEventHandler(this.UpdateUI), new object[] { data });
       //     }
       //     else
       //     {
       //         if (data != null)
       //         {
       //             this.textBox4.Text = data;
       //             this.textBox4.Invalidate();
       //         }
       //     }
       // }
       // private void Proc_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
       // {
       //     this.UpdateUI(e.Data);
       // }

       // private void Proc_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
       // {
       //     this.UpdateUI(e.Data);
       // }

        private void btnMac_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths(false))
            {
                string args = String.Format("{0} -p {1} -b {2} read_mac", this.tbEsptool.Text, this.PortName, this.BaudRate);
                this.textBox4.Text = this.Run(this.tbPython.Text, args);
            }
        }

        private void btnFlashID_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths(false))
            {
                string args = String.Format("{0} -p {1} -b {2} flash_id", this.tbEsptool.Text, this.PortName, this.BaudRate);
                this.textBox4.Text = this.Run(this.tbPython.Text, args);
            }
        }

        private void btnChipID_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths(false))
            {
                string args = String.Format("{0} -p {1} -b {2} chip_id", this.tbEsptool.Text, this.PortName, this.BaudRate);
                this.textBox4.Text = this.Run(this.tbPython.Text, args);
            }
        }

        private void Erase_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths(false))
            {
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string args = String.Format("{0} -p {1} -b {2} erase_flash", this.tbEsptool.Text, this.PortName, this.BaudRate);
                    this.textBox4.Text = this.Run(this.tbPython.Text, args);
                }
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths())
            {
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string args = String.Format("{0} -p {1} -b {2} write_flash --verify --flash_size=detect 0 {3}", this.tbEsptool.Text, this.PortName, this.BaudRate, this.tbFirmware.Text);
                    this.textBox4.Text = this.Run(this.tbPython.Text, args);
                }
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths())
            {
                string args = String.Format("{0} -p {1} -b {2} verify_flash 0x40000 {3}", this.tbEsptool.Text, this.PortName, this.BaudRate, this.tbFirmware.Text);
                this.textBox4.Text = this.Run(this.tbPython.Text, args);
            }
        }

        private int BaudRate
        { get; set; }

        private bool CheckPaths(bool firmware = true)
        {
            string err = "";

            if (!File.Exists(this.tbPython.Text))
                err += "python.exe does not exists!\r\n";

            if (!File.Exists(this.tbEsptool.Text))
                err += "esptool.py does not exists!\r\n";

            if (firmware && !File.Exists(this.tbFirmware.Text))
                err += "firmware.bin does not exists!\r\n";

            if (err != "")
            {
                Helpers.ErrorBox(err);
                return false;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.CheckPaths(false))
            {
                Properties.Settings.Default.PythonExe = this.tbPython.Text;
                Properties.Settings.Default.EspToolPy = this.tbEsptool.Text;
                Properties.Settings.Default.FrimwareBin = this.tbFirmware.Text;
                Properties.Settings.Default.EspToolBaud = this.BaudRate;
                Properties.Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void cbBaudrate_Validating(object sender, CancelEventArgs e)
        {
            int baud = 0;
            if (!int.TryParse(this.cbBaudrate.Text, out baud))
            {
                this.cbBaudrate.BackColor = Color.Red;
                this.cbBaudrate.ForeColor = Color.White;
                Helpers.ErrorBox("Baud rate has invalid value!\r\n");
                e.Cancel = true;
            }

            this.cbBaudrate.BackColor = Color.White;
            this.cbBaudrate.ForeColor = Color.Black;

            this.BaudRate = baud;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel2.Text);
        }

        private void EspToolDialog_Load(object sender, EventArgs e)
        {
        }
    }
}
