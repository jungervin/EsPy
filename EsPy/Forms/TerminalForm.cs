using EsPy.Units;
using EsPy.Utility;
using ScintillaNET;
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
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace EsPy.Forms
{
    public partial class TerminalForm : DockContent, IForm, IPort, IDisposable
    {
        public TerminalForm()
        {
            InitializeComponent();
            this.HideOnClose = true;
            this.DockAreas = DockAreas.Document 
                | DockAreas.DockBottom 
                | DockAreas.DockLeft
                | DockAreas.DockRight
                | DockAreas.DockTop;
            this.scintilla.ReadOnly = true;
            //this.scintilla.Preprocess += new Components.Terminal.PreprocessEvent(TerminalPreprocess);
            
            this.ViewEOL = Properties.Settings.Default.TerminalShowEol;
            this.ViewWhitespace = Properties.Settings.Default.TerminalShowWhitespace;
            this.scintilla.SetLang();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                try
                {
                    if (this.scintilla.HistoryModified)
                        this.scintilla.SaveHistory(this.HistoryFile);
                    this.Port = null;
                }
                catch
                { }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public PySerial Port
        {
            get { return this.scintilla.Port; }
            set
            {
                if (this.scintilla.Port != null)
                {
                    this.scintilla.Port.PortOpen -= Port_PortOpen;
                    this.scintilla.Port.PortClose -= Port_PortClose;
                    this.scintilla.Port.DataReceived -= Port_DataReceived;
                    this.scintilla.Port.ErrorReceived -= Port_ErrorReceived;
                    this.scintilla.Port.PortBusy -= Port_PortBusy;
                    this.scintilla.Port.PortFree -= Port_PortFree;
                }

                if (value != null)
                {
                    value.PortOpen += Port_PortOpen;
                    value.PortClose += Port_PortClose;
                    value.DataReceived += Port_DataReceived;
                    value.ErrorReceived += Port_ErrorReceived;
                    value.PortBusy += Port_PortBusy;
                    value.PortFree += Port_PortFree;
                }
                this.scintilla.Port = value;
            }
        }

        private delegate void UpdateStatusEvent(bool busy);
        public void UpdateBusy(bool busy)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateStatusEvent(UpdateBusy), new object[] { busy });
            }
            else
            {

            }
        }
        private void Port_PortFree(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Port_PortBusy(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Port_ErrorReceived(object sender, string data)
        {

        }

        private void Port_DataReceived(object sender, string data)
        {

        }

        private void Port_PortClose(object sender, EventArgs e)
        {
            this.scintilla.Append("\nDisconnected.\r\n");
            this.scintilla.ReadOnly = true;
        }

        private void Port_PortOpen(object sender, EventArgs e)
        {
            this.scintilla.ReadOnly = false;
            this.scintilla.Append($"{this.Port.PortName} {this.Port.BaudRate } Connected...\r\n");
            this.scintilla.Append("Press CTRL + D or Soft Reset Button on the Toolbar\r\n");
            this.scintilla.Append("Press CTRL + C to interrupt current program.\r\n");
        }

        public ToolStrip ToolStrip
        { get { return this.toolStrip1; } }

        public ToolStrip MenuStrip
        { get { return this.menuStrip1; } }

        private void bntClean_Click(object sender, EventArgs e)
        {
            this.scintilla.Clean();
        }

        private string HistoryFile
        { get { return Path.Combine(Application.StartupPath, "history.txt"); } }

        private void TerminalForm_Load(object sender, EventArgs e)
        {
            this.scintilla.LoadHistory(this.HistoryFile);
        }

        private void UpdateUI()
        {
            this.scintilla.Margins[0].Width = this.scintilla.TextWidth(Style.LineNumber, " " + (this.scintilla.Lines.Count + 1).ToString());

            this.cmSoftReset.Enabled = this.Port != null && this.Port.IsOpen;

            this.mnClean.Enabled =
                this.cmClean.Enabled =
                this.mnSelectAll.Enabled =
                this.cmSelectAll.Enabled = this.scintilla.TextLength > 0;

            this.mnUndo.Enabled =
                this.cmUndo.Enabled = this.scintilla.CanUndo;

            this.mnRedo.Enabled =
                this.cmRedo.Enabled = this.scintilla.CanRedo;

            this.mnCut.Enabled =
                this.mnCopy.Enabled =
                this.mnDelete.Enabled =
                this.cmCut.Enabled =
                this.cmCopy.Enabled =
                this.cmDelete.Enabled = this.scintilla.SelectionEnd > this.scintilla.SelectionStart;
        }

        private void terminal_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            this.UpdateUI();
        }

        private void cmSoftReset_Click(object sender, EventArgs e)
        {
            this.Port.SoftReset();
        }

        private void mnUndo_Click(object sender, EventArgs e)
        {
            this.scintilla.Undo();
        }

        private void mnRedo_Click(object sender, EventArgs e)
        {
            this.scintilla.Redo();
        }

        private void mnCut_Click(object sender, EventArgs e)
        {
            this.scintilla.Cut();
        }

        private void mnCopy_Click(object sender, EventArgs e)
        {
            this.scintilla.Copy();
        }

        private void mnPaste_Click(object sender, EventArgs e)
        {
            this.scintilla.Paste();
        }

        private void mnDelete_Click(object sender, EventArgs e)
        {
            this.scintilla.DeleteRange(this.scintilla.SelectionStart, this.scintilla.SelectionEnd);
        }

        private void mnSelectAll_Click(object sender, EventArgs e)
        {
            this.scintilla.SelectAll();
        }

        private void mnClean_Click(object sender, EventArgs e)
        {
            this.scintilla.Clean();
        }

        private void SendCommand(object sender, EventArgs e)
        {
            ToolStripMenuItem c = sender as ToolStripMenuItem;
            if (c != null && c.Tag != null)
            {
                string[] lines = c.Tag.ToString().Split(';');
                foreach (string line in lines)
                {
                    this.Port.WriteLine(line);
                }
            }
        }

        public bool ViewEOL
        {
            get { return this.scintilla.ViewEol; }
            set
            {

                this.scintilla.ViewEol = value;
                Properties.Settings.Default.TerminalShowEol = value;
                this.mnShowEol.Checked = value;
                this.cmShowEOL.Checked = value;
            }
        }

        public bool ViewWhitespace
        {
            get { return this.scintilla.ViewWhitespace == WhitespaceMode.VisibleAlways; }
            set
            {
                if (value)
                    this.scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
                else this.scintilla.ViewWhitespace = WhitespaceMode.Invisible;

                Properties.Settings.Default.TerminalShowWhitespace = value;
                this.mnShowWhitespace.Checked = value;
                this.cmShowWhitespace.Checked = value;
            }
        }

        private void mnShowEol_Click(object sender, EventArgs e)
        {
            this.ViewEOL = !this.ViewEOL;
        }

        private void mnShowWhitespace_Click(object sender, EventArgs e)
        {
            this.ViewWhitespace = !this.ViewWhitespace;
        }

        private void TerminalForm_Activated(object sender, EventArgs e)
        {
            this.scintilla.Focus();
        }

        //public void UIStateChanged(object sender, UIChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
