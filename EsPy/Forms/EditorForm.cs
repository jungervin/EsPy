using EsPy.Components;

using EsPy.Dialogs;
//using EsPy.Python;
//using EsPy.Python.Jedi;

using EsPy.Units;
using EsPy.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static EsPy.Utility.TextHelper;

namespace EsPy.Forms
{
    public partial class EditorForm : DockContent, IDocument, IPort //, INotifyPropertyChanged
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private EditorForm()
        {
            InitializeComponent();
            this.Text = "new";
            this.HideOnClose = false;
            this.DockAreas = DockAreas.Document;
            this.EOLVisible = Properties.Settings.Default.EolVisible;
            this.WhitespaceVisible = Properties.Settings.Default.WhitespaceVisible;
            
            
        }

        public EditorForm(MainForm main_form) : this()
        {
            this.Port = main_form.Port;
        }

        //public Lexer Lexer
        //{
        //    get {

        //        try
        //        {
        //            return this.scintilla.Lexer;
        //        }
        //        catch(Exception e)
        //        {
        //            Helpers.WarningBox("LEXER => " + e.Message);
        //        }

        //        return Lexer.Python;
                
        //    }
        //    set
        //    {
        //        this.scintilla.Lexer = value;
        //    }
        //}

        private bool IsRunnable
        {
            get
            {
                return this.Port != null &&
                    this.Port.IsOpen &&
                    !this.Port.Busy &&
                    this.FileName != null &&
                    Path.GetExtension(this.FileName) == ".py";
            }
        }

        private PySerial FPort = null;
        public PySerial Port
        {
            get { return this.FPort; }
            set
            {
                if (this.FPort != null)
                {
                    this.FPort.PortOpen -= Port_PortOpen;
                    this.FPort.PortClose -= Port_PortClose;
                    this.FPort.PortBusy -= Port_PortBusy;
                    this.FPort.PortFree -= Port_PortFree;
                }

                this.FPort = value;

                if (this.FPort != null)
                {
                    this.FPort.PortOpen += Port_PortOpen;
                    this.FPort.PortClose += Port_PortClose;
                    this.FPort.PortBusy += Port_PortBusy;
                    this.FPort.PortFree += Port_PortFree;
                }
                this.UpdateUI();
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
                this.btnUpload.Enabled = !busy;
                this.btnRun.Enabled = !busy && this.IsRunnable;
            }
        }

        private void Port_PortFree(object sender, EventArgs e)
        {
            this.UpdateBusy(false);
        }

        private void Port_PortBusy(object sender, EventArgs e)
        {
            this.UpdateBusy(true);
        }

        private void Port_PortClose(object sender, EventArgs e)
        {
            this.btnUpload.Enabled =
                 this.btnRun.Enabled = false;
        }

        private void Port_PortOpen(object sender, EventArgs e)
        {
            this.UpdateUI();
        }

        protected override string GetPersistString()
        {
            if (this.FileName != null)
                return String.Format("{0},{1},{2}",
                    GetType().ToString(),
                    this.FileName,
                    this.scintilla.CurrentPosition
                    );

            return GetType().ToString();
        }

        public bool CanPaste
        { get; set; }

        public ToolStrip ToolStrip
        { get { return this.toolStrip1; } }

        public ToolStrip MenuStrip
        { get { return this.menuStrip1; } }

        public void LoadFromFile(string fname)
        {
            this.FileName = fname;
            this.scintilla.Text = File.ReadAllText(fname);
            this.scintilla.GotoPosition(this.scintilla.TextLength);
            this.scintilla.SetSavePoint();
            this.scintilla.EmptyUndoBuffer();
        }

        public string Source
        { get { return this.scintilla.Text; } }

        public bool Modified
        { get { return this.scintilla.Modified; } }

        public bool CanSave
        { get { return true; } }

        public static FileFormats EditorFileFormats
        {
            get
            {
                FileFormats ff = new FileFormats();
                ff.Add(FileFormat.Python);
                ff.Add(FileFormat.Html);
                ff.Add(FileFormat.Css);
                ff.Add(FileFormat.Js);
                ff.Add(FileFormat.Json);
                ff.Add(FileFormat.Xml);
                ff.Add(FileFormat.Txt);
                ff.Add(FileFormat.All);
                ff.DefaultExt = FileFormat.Python.DefaultExt;
                return ff;
            }
        }

        private string FFileName = null;
        public string FileName
        {
            get { return FFileName; }
            set
            {

                this.FFileName = value;
                this.ToolTipText = value;
                this.Text = Path.GetFileName(this.FFileName);
                string ext = Path.GetExtension(this.FFileName).ToLower();

                FileFormat ff = EditorForm.EditorFileFormats.Find(ext);
                if (ff != null)
                {
                    this.scintilla.Lexer = ff.Lexer;
                    //this.scintilla.LexerLanguage = ff.Language.ToLower() == "html" ? "hypertext" : ff.Language.ToLower();
                    this.scintilla.Lang = ff.Language.ToLower();
                    this.scintilla.SetLang();
                }
                else
                {
                    this.scintilla.Lexer = Lexer.Null;
                    this.scintilla.LexerLanguage = "";
                }

                this.mnComment.Enabled =
                    this.mnUncommet.Enabled =
                    this.cmComment.Enabled =
                    this.cmUncomment.Enabled = this.scintilla.CommentLine != "" || (this.scintilla.CommentStart != "" && this.scintilla.CommentEnd != null);
            }
        }

        private string ShortFileName
        { get { return Path.GetFileName(this.FileName); } }

        public void SaveToFile(string fname)
        {
            File.WriteAllText(this.FileName, this.scintilla.Text);
            this.scintilla.SetSavePoint();
        }

        public DialogResult Save()
        {
            if (this.FileName == null)
            {
                return this.SaveAs();
            }
            else
            {
                this.SaveToFile(this.FileName);
                //this.scintilla.SetSavePoint();
                this.UpdateUI();
            }
            return DialogResult.OK;
        }

        public DialogResult SaveAs()
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = EditorForm.EditorFileFormats.Filters;
            d.DefaultExt = Path.GetExtension(this.FileName);
            d.FilterIndex = EditorForm.EditorFileFormats.FindIndex("." + d.DefaultExt);
            d.FileName = this.ShortFileName;
            DialogResult res = d.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.FileName = d.FileName;
                this.SaveToFile(this.FileName);
                this.scintilla.SetSavePoint();
                this.UpdateUI();
            }
            return res;
        }

        private void mnSave_Click(object sender, EventArgs e)
        {
            //if (Helpers.QuestionBox("Would you like to save changes?") == DialogResult.Yes)
            {
                this.Save();
            }
        }

        private void mnSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string text = "";

            if (this.scintilla.SelectionEnd - this.scintilla.SelectionStart > 0)
            {
                int sl = this.scintilla.LineFromPosition(this.scintilla.SelectionStart);
                int el = this.scintilla.LineFromPosition(this.scintilla.SelectionEnd);
                for (int i = sl; i <= el; i++)
                {
                    text += this.scintilla.Lines[i].Text;
                }
            }
            else
            {
                text = this.scintilla.Text;
            }

            this.Port.Interrupt();
            this.Port.Interrupt();
            this.Port.PasteMode();
            this.Port.ReadAllLines();
            byte[] buff = Encoding.UTF8.GetBytes(text.Replace("\r", ""));
            this.Port.Write(buff, 0, buff.Length);
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

        private void mnFind_Click(object sender, EventArgs e)
        {

        }

        private void mnReplace_Click(object sender, EventArgs e)
        {

        }

        private void EditorForm_Load(object sender, EventArgs e)
        {

        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.scintilla.Modified)
                {
                    DialogResult res = MessageBox.Show("Would you like to save the changes?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (res == DialogResult.Yes)
                    {
                        if (this.Save() != DialogResult.OK)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                this.Port = null;
            }
        }

        public void UpdateUI()
        {
            this.mnUpload.Enabled =
            this.btnUpload.Enabled = this.Port != null
                && this.Port.IsOpen && !this.Port.Busy;

            this.mnRun.Enabled =
                this.btnRun.Enabled = this.IsRunnable;

            this.scintilla.Margins[0].Width = this.scintilla.TextWidth(Style.LineNumber, " " + (this.scintilla.Lines.Count + 1).ToString());

            this.mnSave.Enabled =
              this.btnSave.Enabled = this.scintilla.Modified;

            this.mnSelectAll.Enabled =
                this.cmSelectAll.Enabled = this.scintilla.TextLength > 0;

            this.mnFind.Enabled =
                this.cmFind.Enabled =
                this.mnReplace.Enabled =
                this.cmReplace.Enabled = false;

            this.mnUndo.Enabled =
                this.cmUndo.Enabled =
                this.btnUndo.Enabled = this.scintilla.CanUndo;

            this.mnRedo.Enabled =
                this.cmRedo.Enabled =
                this.btnRedo.Enabled = this.scintilla.CanRedo;

            this.btnPaste.Enabled =
                  this.mnPaste.Enabled =
                  this.cmPaste.Enabled = this.CanPaste;

            this.mnCut.Enabled =
                this.mnCopy.Enabled =
                this.mnDelete.Enabled =
                this.cmCut.Enabled =
                this.cmCopy.Enabled =
                this.cmDelete.Enabled =
                this.btnCut.Enabled =
                this.btnCopy.Enabled =
                this.scintilla.SelectionEnd > this.scintilla.SelectionStart;
        }

        private void UpdateUI(object sender, UpdateUIEventArgs e)
        {
            this.UpdateUI();
        }

        private void mnShowEOL_Click(object sender, EventArgs e)
        {
            bool visible = !this.EOLVisible;
            Properties.Settings.Default.EolVisible = visible;
            foreach (Form form in Application.OpenForms)
            {
                if (form is EditorForm)
                    (form as EditorForm).EOLVisible = visible;
            }
        }

        private void mnShowWhitespace_Click(object sender, EventArgs e)
        {
            bool visible = !this.WhitespaceVisible;
            Properties.Settings.Default.WhitespaceVisible = visible;
            foreach (Form form in Application.OpenForms)
            {
                if (form is EditorForm)
                    (form as EditorForm).WhitespaceVisible = visible;
            }
        }

        public bool EOLVisible
        {
            get { return this.scintilla.ViewEol; }
            set
            {
                this.scintilla.ViewEol = value;

                this.mnShowEol.Checked = value;
                this.cmShowEOL.Checked = value;
            }
        }

        public bool WhitespaceVisible
        {
            get { return this.scintilla.ViewWhitespace == WhitespaceMode.VisibleAlways; }
            set
            {
                if (value)
                    this.scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
                else
                    this.scintilla.ViewWhitespace = WhitespaceMode.Invisible;

                this.mnShowWhitespace.Checked = value;
                this.cmShowWhitespace.Checked = value;
            }
        }

        private void mnComment_Click(object sender, EventArgs e)
        {
            int ss = this.scintilla.SelectionStart;
            int se = this.scintilla.SelectionEnd;
            bool selected = se > ss;

            string cs = this.scintilla.CommentLine;
            string ce = "";
            if (cs == "")
            {
                cs = this.scintilla.CommentStart;
                ce = this.scintilla.CommentEnd;
            }

            int ls = this.scintilla.LineFromPosition(ss);
            int le = this.scintilla.LineFromPosition(se);
            for (int i = le; i >= ls; i--)
            {
                Line line = this.scintilla.Lines[i];
                int end = line.EndPosition;

                int j = line.Position;
                for (; j < line.EndPosition; j++)
                {
                    if (scintilla.Text[j] == '\r' || this.scintilla.Text[j] == '\n')
                        break;
                }
                if (j > line.Position)
                {
                    this.scintilla.InsertText(j, ce);
                    this.scintilla.InsertText(line.Position, cs);
                    se += ce.Length + cs.Length;
                }

            }

            if (selected)
            {
                this.scintilla.SelectionStart = ss;
                this.scintilla.SelectionEnd = se;
                this.scintilla.CurrentPosition = se;
            }
            else this.scintilla.GotoPosition(se);
        }

        private void mnUncommet_Click(object sender, EventArgs e)
        {
            int ss = this.scintilla.SelectionStart;
            int se = this.scintilla.SelectionEnd;
            bool selected = se > ss;

            int ls = this.scintilla.LineFromPosition(ss);
            int le = this.scintilla.LineFromPosition(se);

            string cs = this.scintilla.CommentLine;
            string ce = "";
            if (cs == "")
            {
                cs = this.scintilla.CommentStart;
                ce = this.scintilla.CommentEnd;
            }

            for (int i = ls; i <= le; i++)
            {
                Line line = this.scintilla.Lines[i];

                int ep = line.EndPosition - ce.Length;
                for (; ep > line.Position; ep--)
                {
                    if (scintilla.Text.Substring(ep, ce.Length) == ce)
                    {
                        scintilla.DeleteRange(ep, ce.Length);
                        se -= ce.Length;
                        break;
                    }
                }

                for (int sp = line.Position; sp < line.EndPosition - cs.Length; sp++)
                {
                    if (scintilla.Text.Substring(sp, cs.Length) == cs)
                    {
                        scintilla.DeleteRange(sp, cs.Length);
                        se -= cs.Length;

                    }
                }
            }
            this.scintilla.SelectionStart = ss;
            this.scintilla.SelectionEnd = se;
            this.scintilla.CurrentPosition = se;
        }

        public void ActiveContentChanged()
        {
            this.scintilla.Focus();
        }

        private void scintilla_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                this.scintilla.Focus();
        }

        private void scintilla_SavePointLeft(object sender, EventArgs e)
        {
            this.Text = this.ShortFileName + "*";
        }

        private void scintilla_SavePointReached(object sender, EventArgs e)
        {
            this.Text = this.ShortFileName;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Globals.Terminal.Locked = true;
            this.Port.Sync(true);
            this.Port.Clean();

            //UploadConfirmation dd = new UploadConfirmation();
            //dd.Port = this.Port;
            //dd.ShowDialog();

            //this.Port.Clean();
            //this.Port.Sync(false);
            //Globals.Terminal.Locked = false;

            //return;

            ProgressDialog d = new ProgressDialog();
            d.Port = this.Port;
            d.Mode = ProgressDialog.Modes.Upload;
            d.FileName = this.ShortFileName;
            d.Buffer = Encoding.UTF8.GetBytes(this.scintilla.Text);
            d.ShowDialog();
            d.Dispose();

            this.Port.Clean();
            this.Port.Sync(false);
            Globals.Terminal.Locked = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (this.Port != null)
            {
                string text = this.scintilla.SelectedText;
                if (text == "")
                    text = this.scintilla.GetWordFromPosition(this.scintilla.CurrentPosition);

                this.Port.Dir(text);
            }
        }

        private void mnInterrupt_Click(object sender, EventArgs e)
        {
            if (this.Port != null)
            {
                this.Port.Interrupt();
            }
        }


        //private Line CurrentLine
        //{ get { return this.scintilla.Lines[this.scintilla.CurrentLine]; } }
        //private string GetWholeWord(int pos)
        //{
        //    string res = "";
        //    for (int i = this.CurrentLine.Position; i < this.scintilla.CurrentLine; i++)
        //    {
        //        char c = this.CurrentLine.Text[i];
        //        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '.' || c == '_')
        //        {
        //            res += c;
        //        }
        //    }
        //    return res;
        //}

        //private ToolTip FTooltip = null;
        //private ToolTip Tooltip
        //{ get { return this.FTooltip; }
        //    set
        //    {
        //        if (this.FTooltip != null)
        //        {
        //            this.FTooltip.Dispose();
        //            this.FTooltip = null;
        //        }

        //        this.FTooltip = value;
        //        if (this.FTooltip != null)
        //        {
        //            //this.FTooltip.Cap
        //        }
        //    }
        //}

        //private CompletionForm FCompletionForm = null;
        //private CompletionForm CompletionForm
        //{ get { return this.FCompletionForm; }

        //    set
        //    {
        //        if (this.FCompletionForm != null)
        //        {
        //            this.FCompletionForm.Dispose();
        //            this.FCompletionForm = null;
        //        }

        //        this.FCompletionForm = value;                
        //    }
        //}


        //public void ShowCompletions(List<BaseDefinition> list, Words words)
        //{

        //    //if (this.CompletionForm != null)
        //    //{
        //    //    this.CompletionForm.Dispose();
        //    //    this.CompletionForm = null;
        //    //}
        //    //this.CompletionForm = new CompletionListBox(this.scintilla);
        //    //this.CompletionForm.Words = words;
        //    //this.CompletionForm.Definitions = list;
        //    //this.CompletionForm.Show();
        //    ////AutoCompletion ac = new AutoCompletion(this.scintilla);
        //    ////ac.Show();
        //}

        //private CompletionListBox CompletionForm = null;
        //protected override bool ProcessCmdKey(ref Message msg, Keys key)
        //{
        //    const int WM_KEYDOWN = 0x100;
        //    const int WM_SYSKEYDOWN = 0x104;

        //    if (this.scintilla.CallTipActive && this.CompletionForm == null)
        //    {
        //        this.scintilla.CallTipCancel();

        //    }

        //    if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
        //    {
        //        if (key == (Keys.Control | Keys.Space))
        //        {
        //            this.Completions("");
        //            return true;
        //        }
        //    }

        //    return base.ProcessCmdKey(ref msg, key);
        //    //if (this.Port != null)
        //    {
        //        if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
        //        {
        //            if (key == (Keys.Control | Keys.Space))
        //            {
        //                this.Completions("");
        //                return true;
        //            }

        //            switch (key)
        //            {

        //                case Keys.Up:
        //                    if (this.CompletionForm.Visible)
        //                    {
        //                        this.CompletionForm.SelectPreviousItem();
        //                        return true;
        //                    }
        //                    break;

        //                case Keys.Down:
        //                    if (this.CompletionForm.Visible)
        //                    {
        //                        this.CompletionForm.SelectNextItem();
        //                        return true;
        //                    }
        //                    break;

        //                case Keys.Enter:
        //                case Keys.Tab:
        //                    BaseDefinition def = this.CompletionForm.SelectedDefinition;
        //                    string insert = "";
        //                    if (def is Completion)
        //                    {
        //                        insert = (def as Completion).complete;
        //                    }
        //                    this.scintilla.InsertText(this.scintilla.CurrentPosition, insert);
        //                    this.scintilla.GotoPosition(this.scintilla.CurrentPosition + insert.Length);
        //                    this.CompletionForm.Hide();
        //                    return true;

        //                case Keys.Escape:
        //                    this.CompletionForm.Hide();
        //                    return true;
        //            }
        //        }
        //    }

        //    return base.ProcessCmdKey(ref msg, key);
        //}



        //private void Completions(string pattern)
        //{
        //    Line line = this.scintilla.Lines[this.scintilla.CurrentLine];
        //    int l = this.scintilla.CurrentLine + 1;
        //    int c = this.scintilla.CurrentPosition - line.Position;

        //    Words words = TextHelper.FindWords(line.Text, c-1);
        //    c = Math.Min(words.Pos+1, c);

        //    Console.WriteLine($"WORD: {words.Word} FILTER: {words.Filter} COLUMN:{words.Pos}");

        //    Script script = new Script(this.scintilla.Text, l, c);

        //    PyRequest req = new CompletionRequest(script);
        //    string json = JsonConvert.SerializeObject(req);
        //    try
        //    {
        //        string res = Globals.PyClient.GetJedi(json);
        //        File.WriteAllText("log.txt", res);

        //        JToken token = JObject.Parse(res);

        //        if (token != null && token["completions"] is JArray)
        //        {
        //            JArray items = token["completions"] as JArray;
        //            List<BaseDefinition> list = new List<BaseDefinition>();

        //            foreach (JToken t in items)
        //            {
        //                BaseDefinition def = JsonConvert.DeserializeObject<Completion>(t.ToString());

        //                if (def != null)
        //                    list.Add(def);
        //            }

        //            if (list.Count > 0)
        //            {
        //                this.ShowCompletions(list, words);

        //                //this.scintilla.AutoCComplete();

        //                //string words = "";
        //                //string sep = "";

        //                //foreach (BaseDefinition def in list)
        //                //{
        //                //    words += sep + def.name;
        //                //    sep = " ";
        //                //}

        //                //this.scintilla.AutoCShow(1, words);

        //            }
        //            else
        //            {
        //                //this.scintilla.AutoC
        //                //this.scintilla.AutoCMaxHeight = 12;
        //                //this.scintilla.AutoCMaxWidth = 48;
        //                //this.scintilla.CallTipSetPosition(true);
        //                this.scintilla.CallTipShow(this.scintilla.CurrentPosition, "No suggestions");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helpers.ErrorBox(ex);
        //    }
        //}

        ////private void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        ////{
        ////    var currentPos = scintilla.CurrentPosition;
        ////    var wordStartPos = scintilla.WordStartPosition(currentPos, true);

        ////    var lenEntered = currentPos - wordStartPos;
        ////    if (lenEntered > 0 && this.CompletionForm == null)
        ////    {
        ////        if(this.CompletionForm == null)
        ////            this.Completions("");
        ////    }

        ////}


        //public void UIStateChanged(object sender, UIChangedEventArgs e)
        //{
        //    this.scintilla.HideCompletions();
        //}

        //private void scintilla_MouseHover(object sender, EventArgs e)
        //{
        //    this.scintilla.CallTipCancel();

        //    //string text = this.scintilla.C
        //    //this.scintilla.CallTipShow(this.scintilla.CurrentPosition, "Loading");
        //}

        //private void scintilla_Click(object sender, EventArgs e)
        //{
        //    if (this.CompletionForm != null)
        //    {
        //        this.CompletionForm.Dispose();
        //        this.CompletionForm = null;
        //    }
        //}
    }
}
