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
    public partial class UploadConfirmation : Form
    {
        public UploadConfirmation()
        {
            InitializeComponent();
        }

        public PySerial Port
        { get; set; }

        private void UploadConfirmation_Load(object sender, EventArgs e)
        {
            ResultStatus cwd = this.Port.Cwd();
            if (cwd.Result == ResultStatus.Statuses.Success)
            {
                this.Path.Text = cwd.ToString().Replace("'", "");
                if (this.Path.Text == "")
                    this.Path.Text = "/";
            }
            else
            {
                Helpers.ErrorBox(cwd);
                return;
            }
        }
    }
}
