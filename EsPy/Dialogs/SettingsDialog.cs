using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Dialogs
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
            this.tbPythonPath.Text = Properties.Settings.Default.PythonExe;
            this.ShowServer.Checked = Properties.Settings.Default.ShowPyServer;
        }

        private void btnPythonPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "python.exe|python.exe";
            if (d.ShowDialog() == DialogResult.OK)
            {
                this.tbPythonPath.Text = d.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PythonExe = tbPythonPath.Text;
            Properties.Settings.Default.ShowPyServer = this.ShowServer.Checked;
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bat = Path.Combine(Application.StartupPath, "pip.bat");
            if (File.Exists(bat))
            {
                Process.Start(bat);
            }
        }
    }
}
