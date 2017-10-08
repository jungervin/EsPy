using EsPy.Python.Jedi;
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

namespace EsPy.Components
{
    public partial class CompletionForm : Form
    {
        public event EventHandler FormDisposing = null;
        public CompletionForm()
        {
            InitializeComponent();
            //this.Capture = true;
            this.listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
           
            
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

                return baseParams;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.ToolTip != null)
            {
                this.ToolTip.Dispose();
            }

            Globals.MainForm.Deactivate -= MainForm_Deactivate;
            Globals.MainForm.Resize -= MainForm_Resize;

            if (this.FormDisposing != null)
                this.FormDisposing(this, new EventArgs());

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Scintilla FScintilla = null;
        public Scintilla Scintilla
        {
            get { return this.FScintilla; }
            set
            {
                this.FScintilla = value;
                if (this.FScintilla != null)
                {
                    this.listBox.Font = this.Scintilla.Font;
                }
            }
        }

        public void Clear()
        {
            this.listBox.Items.Clear();
        }

        //public void Add(Completion item)
        //{
        //    this.listBox.Items.Add(item);
        //}

        //public void AddRange(List<Completion> list)
        //{
        //    this.listBox.Items.AddRange(list.ToArray());
        //}

        public int Count
        { get { return this.listBox.Items.Count; } }

        public int SelectedIndex
        {
            get { return this.listBox.SelectedIndex; }
            set {

                if (this.ToolTip != null)
                {
                    this.ToolTip.Dispose();
                    this.ToolTip = null;
                }
                this.listBox.SelectedIndex = value;
            }
        }

        //public void Home()
        //{
        //    this.SelectedIndex = 0;
        //}

        //public void End()
        //{
        //    this.SelectedIndex = this.listBox.Items.Count - 1;
        //}

        //public BaseDefinition SelectedItem
        //{ get { return this.listBox.SelectedItem as BaseDefinition; } }

        public void SelectPrevious()
        {
            if (this.SelectedIndex < 0)
            {
                this.SelectedIndex = 0;
            }
            else if (this.SelectedIndex > 0)
            {
                this.SelectedIndex--;
            }
        }

        public void SelectNext()
        {
            if (this.SelectedIndex < 0)
            {
                this.SelectedIndex = 0;
            }
            else if (this.SelectedIndex < this.listBox.Items.Count - 1)
            {
                this.SelectedIndex++;
            }
        }

        //public void PageUp()
        //{

        //}

        //public void PageDown()
        //{

        //}

        private ToolTip ToolTip = null;
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox.SelectedItem != null)
            {
                BaseDefinition def = (this.listBox.SelectedItem as BaseDefinition);

                string doc = string.IsNullOrEmpty(def.docstring) ? "No documentation" : def.docstring;
                string module = string.IsNullOrEmpty(def.module_name) ? "" : "module: " + def.module_name + "\r\n";
                string type = string.IsNullOrEmpty(def.type) ? "" : def.type + " ";
                string parm = def.parameters != null ? "(" + string.Join(", ", def.parameters) + ")" : "";
                string title = type + def.name + parm;
                if (ToolTip == null)
                {
                    //ToolTip.Hide(this);
                    //ToolTip.Dispose();
                    this.ToolTip = new ToolTip();
                }
                //ToolTip = new ToolTip();
                if (!ToolTip.Equals(ToolTip.Tag))
                {
                    ToolTip.ShowAlways = true;
                    ToolTip.ToolTipTitle = title;
                    Point p = this.Scintilla.PointToClient(new Point(this.Right + 2, this.Top + 2));
                    ToolTip.Tag = this.listBox.SelectedItem;
                    ToolTip.Show(module + doc, this.Scintilla, p);
                }
            }
            else if (this.ToolTip != null)
            {
                this.ToolTip.Dispose();
                this.ToolTip = null;
            }
        }

        //private void listBox_MouseDown(object sender, MouseEventArgs e)
        //{
           
        //}

        private void CompletionForm_Shown(object sender, EventArgs e)
        {
            Globals.MainForm.Deactivate += MainForm_Deactivate;
            Globals.MainForm.Resize += MainForm_Resize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompletionForm_Load(object sender, EventArgs e)
        {
            this.Height = this.listBox.Height + 2;
        }
    }
}
