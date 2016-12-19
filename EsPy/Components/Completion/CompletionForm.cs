using EsPy.Python;
using EsPy.Python.Jedi;
using EsPy.Utility;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Components.Completion
{
    public partial class CompletionForm : Form
    {
        public class CompletionEventArgs : EventArgs
        {
            public string Word = "";
            public CompletionEventArgs(string word)
            {
                this.Word = word;
            }
        }

        public delegate void SelectedEvent(object sender, CompletionEventArgs e);
        public delegate void ClosingEvent(object sender, CompletionEventArgs e);
        public SelectedEvent Selected = null;
        public ClosingEvent Closing = null;

        public CompletionForm()
        {
            InitializeComponent();
            //this.Capture = true;
        }

        public Scintilla Scintilla
        {
            get { return this.listBox1.Scintilla; }
            set { this.listBox1.Scintilla = value; }
        }

        private List<BaseDefinition> FDefinitions = null;
        public List<BaseDefinition> Definitions
        {
            get { return this.FDefinitions; }
            set
            {
                this.listBox1.Items.Clear();
                this.FDefinitions = value;
                
                this.listBox1.Items.AddRange(value.ToArray());
                this.listBox1.Focus();// .Focused = true; //.SelectedIndex = 0;
            }
        }

        public void Home()
        {
            
        }
      
    }
}
