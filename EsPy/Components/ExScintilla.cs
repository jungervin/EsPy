using EsPy.Python;
using EsPy.Python.Jedi;
using EsPy.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using static EsPy.Utility.TextHelper;

namespace EsPy.Components
{
    public class ExScintilla : Scintilla
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private const int NUMBER_MARGIN = 1;
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        const int WM_KEYDOWN = 0x100;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_LBUTTONDOWN = 0x0201;
        public string Lang = "";
        public string CommentLine = "";
        public string CommentStart = "";
        public string CommentEnd = "";

        public ExScintilla() : base()
        {
            this.InitializeComponent();
            this.CompletionEnabled = false;
            this.Completions = new List<Completion>(500);
            this.MouseDwellTime = 500;
        }

        public bool CompletionEnabled
        { get; set; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ExScintilla
            // 
            this.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.ExScintilla_CharAdded);
            this.Delete += new System.EventHandler<ScintillaNET.ModificationEventArgs>(this.ExScintilla_Delete);
            this.DwellEnd += new System.EventHandler<ScintillaNET.DwellEventArgs>(this.ExScintilla_DwellEnd);
            this.Leave += new System.EventHandler(this.ExScintilla_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExScintilla_MouseDown);
            this.MouseHover += new System.EventHandler(this.ExScintilla_MouseHover);
            this.Resize += new System.EventHandler(this.ExScintilla_Resize);
            this.ResumeLayout(false);

        }

        private CompletionForm FCompletionForm = null;
        public void HideCompletions()
        {
            if (this.FCompletionForm != null)
            {
                this.FCompletionForm.Dispose();
                this.FCompletionForm = null;
            }
        }

        public void CompletionFormDisposing(object sender, EventArgs e)
        {
            this.FCompletionForm.FormDisposing -= CompletionFormDisposing;
            this.FCompletionForm = null;
        }

        public void ShowCompletion()
        {
            //this.Capture = true;
            this.HideCompletions();
            this.FCompletionForm = new CompletionForm();
            this.FCompletionForm.FormDisposing += CompletionFormDisposing;
            this.FCompletionForm.Scintilla = this;

            int word_start_pos = Utility.TextHelper.KeywordStartPosition(this.Text, this.CurrentPosition - 1) + 1;
            int x = this.PointXFromPosition(word_start_pos);
            int y = this.PointYFromPosition(word_start_pos);

            this.FCompletionForm.Show();

            Point p = this.PointToScreen(new Point(x, y));

            this.FCompletionForm.TopLevel = true;
            this.FCompletionForm.TopMost = true;
            this.FCompletionForm.Top = p.Y + (int)(this.Font.Height * 1.2);
            this.FCompletionForm.Left = p.X;
            this.Focus();
            this.SetFilter(0);
        }

        private List<Completion> Completions
        {get; set; }

        private void SetFilter(int dx)
        {
            this.FCompletionForm.Clear();
            Line line = this.Lines[this.CurrentLine];
            int l = this.CurrentLine + dx;
            int c = this.CurrentPosition - line.Position;

            Words words = TextHelper.FindWords(line.Text, c + dx);
            
            List<Completion> list = this.Completions.Where(k => k.name.ToLower().StartsWith(words.Filter.ToLower())).ToList();

            // Todo: Add/Remove items
            this.FCompletionForm.AddRange(list);

            if (words.Filter != "" && this.FCompletionForm.Count > 0)
            {
                this.FCompletionForm.SelectedIndex = 0;
            }
            else
            {
                
            }
        }

        private void Complete()
        {
            
            if (this.FCompletionForm.SelectedItem != null)
            {
                string name = this.FCompletionForm.SelectedItem.name;
                // prevent Delete & Insert events!!)
                this.HideCompletions();

                if (name != null)
                {
                    Line line = this.Lines[this.CurrentLine];
                    int l = this.CurrentLine + 1;
                    int c = this.CurrentPosition - line.Position;

                    Words words = TextHelper.FindWords(line.Text, c);

                    this.DeleteRange(line.Position + words.Pos, words.Filter.Length);
                    this.InsertText(line.Position + words.Pos, name);
                    this.GotoPosition(line.Position + words.Pos + name.Length);                    
                }
            }
            else this.HideCompletions();
        }
         
        //protected override void WndProc(ref Message m)
        //{
        //    if (this.FCompletionForm != null)
        //    {
        //        if (m.Msg == WM_LBUTTONDOWN)
        //        {
        //            PostMessage(this.FCompletionForm.Handle, (uint) m.Msg,(int) m.WParam,(int)m.LParam);
        //            return;
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys key)
        {
            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                switch (key)
                {
                    case Keys.Control | Keys.Space:                      
                        this.FindCompletions();
                        return true;

                    case Keys.Escape:
                        this.HideCompletions();
                        return true;

                    case Keys.Enter:
                    case Keys.Tab:
                        if (this.FCompletionForm != null)
                        {
                            this.Complete();
                            return true;
                        }
                        break;

                    case Keys.Left:

                        if (this.FCompletionForm != null)
                        {
                            int p = this.CurrentPosition - 1;
                            char c = this.Text[p];
                            if (c == '.' || char.IsWhiteSpace(c))
                            {
                                this.HideCompletions();
                                
                            }
                            else
                            {
                                this.SetFilter(-1);
                            }
                        }
                        break;

                    case Keys.Right:
                        if (this.FCompletionForm != null)
                        {
                            int p = this.CurrentPosition ;
                            char c = this.Text[p];
                            if (c == '.' || char.IsWhiteSpace(c))
                            {
                                this.HideCompletions();
                            }
                            else
                            {
                                this.SetFilter(1);
                            }
                        }
                        break;

                    case Keys.Up:
                        if (this.FCompletionForm != null)
                        {
                            this.FCompletionForm.SelectPrevious();
                            return true;
                        }
                        break;

                    case Keys.Down:
                        if (this.FCompletionForm != null)
                        {
                            this.FCompletionForm.SelectNext();
                            return true;
                        }
                        break;

                    case Keys.Home:
                        if (this.FCompletionForm != null)
                        {
                            this.HideCompletions();
                        }
                        break;

                    case Keys.End:
                        if (this.FCompletionForm != null)
                        {
                            this.HideCompletions();
                        }
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, key);
        }

        private int CompletionPosition = 0;

        private void FindCompletions()
        {
            Line line = this.Lines[this.CurrentLine];
            int l = this.CurrentLine + 1;
            int c = this.CurrentPosition - line.Position;
            Words words = TextHelper.FindWords(line.Text, c);
            this.CompletionPosition = line.Position + words.Pos;
            this.FindCompletions(l, c);
        }

        private void FindCompletions(int line, int column)
        {
            if (Globals.PyClient != null && this.Lexer == Lexer.Python && this.CompletionEnabled)
            {
                this.HideCompletions();
                this.Completions.Clear();

                Script script = new Script(this.Text, line, column);

                PyRequest req = new CompletionRequest(script);
                string json = JsonConvert.SerializeObject(req);
                try
                {
                    JToken token = Globals.PyClient.DoRequest<JToken>(req);
                    if (token != null && token["completions"] is JArray)
                    {
                        JArray items = token["completions"] as JArray;
                        foreach (JToken t in items)
                        {
                            Completion comp = JsonConvert.DeserializeObject<Completion>(t.ToString());
                            if (comp != null)
                                this.Completions.Add(comp);
                        }

                        //Todo: move to Show
                        if (this.Completions.Count > 0)
                        {
                            this.ShowCompletion();
                        }
                        else
                        {
                            this.CallTipShow(this.CurrentPosition, "No suggestions");
                        }
                    }
                }
                catch
                { 
                    this.CallTipShow(this.CurrentPosition, "Try again**");
                }
            }
        }

        private string GetAttr(XmlNode xnode, string attr)
        {
            if (xnode != null && xnode.Attributes[attr] != null && xnode.Attributes[attr].Value != null)
            {
                return xnode.Attributes[attr].Value;
            }
            return "";
        }

        private void LoadStyle()
        {
            string conf = Path.Combine(Application.StartupPath, "Conf", "scistyles.xml");
            if (File.Exists(conf))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(conf);
                    XmlNode xstyles = doc.SelectSingleNode("Scintilla/LexerStyles");
                    if (xstyles != null && xstyles.HasChildNodes)
                    {
                        foreach (XmlNode xstyle in xstyles.ChildNodes)
                        {
                            if (xstyle.Name == "LexerType" && xstyle.Attributes["name"] != null && xstyle.Attributes["name"].Value == this.Lang && xstyle.HasChildNodes)
                            {
                                foreach (XmlNode xword in xstyle.ChildNodes)
                                {
                                    int id = 0;
                                    int c = 0;
                                    if (int.TryParse(this.GetAttr(xword, "styleID"), out id))
                                    {
                                        if (int.TryParse(this.GetAttr(xword, "fgColor"), System.Globalization.NumberStyles.HexNumber, null, out c))
                                            this.Styles[id].ForeColor = Color.FromArgb(c);

                                        if (int.TryParse(this.GetAttr(xword, "bgColor"), System.Globalization.NumberStyles.HexNumber, null, out c))
                                            this.Styles[id].BackColor = Color.FromArgb(c);

                                        if (this.GetAttr(xword, "fontName") != "")
                                        {
                                            this.Styles[id].Font = this.GetAttr(xword, "fontName");
                                        }

                                        if (int.TryParse(this.GetAttr(xword, "fontStyle"), out c))
                                        {
                                            this.Styles[id].Bold = (c & 1) == 1;

                                        }
                                        if (int.TryParse(this.GetAttr(xword, "fontSize"), out c))
                                        {
                                            this.Styles[id].Size = c;
                                        }

                                        if (this.GetAttr(xstyle, "keywordClass") != "")
                                        {

                                        }

                                    }
                                }
                            }

                        }
                    }
                }
                catch
                { }
                finally
                {
                    doc = null;
                }
            }
        }

        private void LoadLang()
        {
            string conf = Path.Combine(Application.StartupPath, "Conf", "scilang.xml");
            if (File.Exists(conf))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(conf);
                    XmlNode xstyles = doc.SelectSingleNode("Scintilla/Languages");
                    if (xstyles != null && xstyles.HasChildNodes)
                    {
                        foreach (XmlNode xstyle in xstyles.ChildNodes)
                        {
                            if (xstyle.Name == "Language" && xstyle.Attributes["name"] != null && xstyle.Attributes["name"].Value == this.Lang && xstyle.HasChildNodes)
                            {
                                int id = 0;

                                this.CommentLine = this.GetAttr(xstyle, "commentLine");
                                this.CommentStart = this.GetAttr(xstyle, "commentStart");
                                this.CommentEnd = this.GetAttr(xstyle, "commentEnd");

                                foreach (XmlNode xword in xstyle.ChildNodes)
                                {
                                    if (xword.Name == "Keywords")
                                    {
                                        this.SetKeywords(id++, xword.InnerText);
                                    }
                                }
                                return;
                            }
                        }
                    }
                }
                catch
                { }
                finally
                {
                    doc = null;
                }
            }
        }

        public void SetLang()
        {
            this.EolMode = Eol.CrLf;
            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";
            this.Styles[Style.Default].Size = 10;
            this.StyleClearAll(); 
            this.IndentWidth = 4;
            this.IndentationGuides = IndentView.LookForward;
           
            this.SetProperty("tab.timmy.whinge.level", "1");
            this.SetProperty("fold", "1");

            this.Margins[0].Width = 42;

            this.Margins[2].Type = MarginType.Symbol;
            this.Margins[2].Mask = Marker.MaskFolders;
            this.Margins[2].Sensitive = true;
            this.Margins[2].Width = 20;

            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                this.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                this.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            this.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            this.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            this.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            this.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            this.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            this.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            this.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            this.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            this.LoadStyle();
            this.LoadLang();
        }

        public void SetDefault()
        {
            this.EolMode = Eol.CrLf;
            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";
            this.Styles[Style.Default].Size = 10;
            this.StyleClearAll(); // i.e. Apply to all
            
          
            this.SetProperty("tab.timmy.whinge.level", "1");
            this.SetProperty("fold", "1");

            this.Margins[0].Width = 42;

            this.Margins[2].Type = MarginType.Symbol;
            this.Margins[2].Mask = Marker.MaskFolders;
            this.Margins[2].Sensitive = true;
            this.Margins[2].Width = 20;

            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                this.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                this.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            this.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            this.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            this.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            this.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            this.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            this.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            this.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            this.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        private void ExScintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            var currentPos = this.CurrentPosition;
            var wordStartPos = this.WordStartPosition(currentPos, true);

            char c = this.Text[this.CurrentPosition-1];
            // Display the autocompletion list
            var lenEntered = currentPos - wordStartPos;
            if (e.Char == ' ' && this.FCompletionForm != null)
            {
                this.HideCompletions();
            }
            else if (e.Char == '(')
            {
                Application.DoEvents();
                this.FindCompletions();

                if (this.Completions.Count == 1)
                {
                    Completion comp = this.Completions[0];
                }
            }
            else if (e.Char == '.')
            {
                Application.DoEvents();
                this.FindCompletions();
            }
            else if (this.FCompletionForm == null && lenEntered == 1)
            {
                this.FindCompletions();
            }
            else if (this.FCompletionForm != null)
            {
                this.SetFilter(0);
            }
        }

        private void ExScintilla_Delete(object sender, ModificationEventArgs e)
        {
            if (this.FCompletionForm != null)
            {
                if (e.Text.Contains('.') || e.Text.Contains("(") || e.Text.Contains(")") || this.CurrentPosition < this.CompletionPosition)
                {
                    this.HideCompletions();
                }
                else
                {
                    this.SetFilter(0);
                }
            }

        }

        private void ExScintilla_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void ExScintilla_Leave(object sender, EventArgs e)
        {
            if (this.FCompletionForm != null && this.FCompletionForm.Visible)
                this.HideCompletions();
        }

        private void ExScintilla_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.FCompletionForm != null)
                this.HideCompletions();
        }

        private void ExScintilla_Resize(object sender, EventArgs e)
        {
            if (this.FCompletionForm != null)
                this.HideCompletions();
        }

        private void ExScintilla_DwellEnd(object sender, DwellEventArgs e)
        {
            // Todo:
            //if (e.Position > 0)
            //{
            //    Line line = this.Lines[this.LineFromPosition(e.Position)];
            //    int sp = e.Position - line.Position;
            //    while (sp > 0)
            //    {
            //        if (line.Text[sp] == '.' || line.Text[sp] == '_' || char.IsLetterOrDigit(line.Text[sp]))
            //            sp--;
            //        else break;
            //    }

            //    int ep = e.Position - line.Position;
            //    while (ep < line.Text.Length)
            //    {
            //        if (line.Text[ep] == '.' || line.Text[ep] == '_' || char.IsLetterOrDigit(line.Text[ep]))
            //            ep++;
            //        else break;
            //    }
            //    if (ep > sp)
            //    {
            //        string w = line.Text.Substring(sp, ep - sp);
            //        Console.WriteLine(w);

                    
            //    }
            //}
        }
    }
}

    
