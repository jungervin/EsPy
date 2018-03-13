using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Dialogs
{
    public partial class DeviceEditorDialog : Form
    {
        public DeviceEditorDialog()
        {
            InitializeComponent();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox1.Text += "# Supported variables:\r\n";
            this.textBox1.Text += "#     $PORT\r\n";
            this.textBox1.Text += "#     $BAUDRATE\r\n";
            this.textBox1.Text += "#     $FIRMWARE\r\n";
            this.textBox1.Text += "# \r\n";
            this.textBox1.Text += "# Format (Separated by semicolon!!): \r\n";
            this.textBox1.Text += "#     Device Name; Parameters\r\n";
            this.textBox1.Text += "# \r\n";
            this.textBox1.Text += "\r\n";
            this.textBox1.Text += "Wemos Mini D1; -p $PORT - b $BAUDRATE write_flash -fm dio - fs detect 0x0000 \"$FIRMWARE\"\r\n";
            this.textBox1.Text += "Node MCU; -p $PORT - b $BAUDRATE write_flash -fm dio - fs detect 0x0000 \"$FIRMWARE\"\r\n";
            this.textBox1.Text += "Sonoff Basic; -p $PORT - b $BAUDRATE write_flash -fm dout - ff 20m - fs detect 0x0000 \"$FIRMWARE\"\r\n";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(Globals.DevicesFile, this.textBox1.Text);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Helpers.ErrorBox(ex.Message);
            }

        }

        private void DeviceEditorDialog_Load(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Text = File.ReadAllText(Globals.DevicesFile);
                this.textBox1.SelectionStart = this.textBox1.Text.Length;
            }
            catch(Exception ex)
            {
                Helpers.ErrorBox(ex.Message);
            }
        }
    }
}
