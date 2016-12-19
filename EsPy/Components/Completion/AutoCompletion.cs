using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Components.Completion
{
    public class AutoCompletion : ToolStripDropDown, IDisposable
    {
        public AutoCompletion(Scintilla scintilla)
        {
            AutoClose = false;
            AutoSize = false;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            BackColor = Color.White;
            CompletionListBox listView = new CompletionListBox(scintilla);
            ToolStripControlHost host = new ToolStripControlHost(listView);
            host.Margin = new Padding(2, 2, 2, 2);
            host.Padding = Padding.Empty;
            host.AutoSize = false;
            host.AutoToolTip = false;
            //CalcSize();
            base.Items.Add(host);
            listView.Parent = this;
            //SearchPattern = @"[\w\.]";
            //MinFragmentLength = 2;
        }
    }
}
