using EsPy.Units;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Components
{
    public class Terminal : ExScintilla
    {
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_KEYDOWN = 0x100;
        const int WM_SYSKEYDOWN = 0x104;

        int PromptPos = 0;
        //int ActPos = 0;
        int BeforeMouse = -1;

        int HistoryPos = 0;
        public bool HistoryModified = false;
        public List<string> History = new List<string>();

        public Terminal() : base()
        {
            this.Lexer = Lexer.Python;
            this.Lang = "python";
            this.SetLang();
            this.ReadOnly = true;
        }

        private PySerial FPort = null;
        public PySerial Port
        {
            get { return this.FPort; }
            set
            {
                if (this.FPort != null)
                {
                    this.FPort.PortOpen -= FPort_PortOpen;
                    this.FPort.PortClose -= FPort_PortClose;
                    this.FPort.DataReceived -= FPort_DataReceived;
                    this.FPort.ErrorReceived -= FPort_ErrorReceived;
                    this.FPort.PortBusy -= FPort_PortBusy;
                    this.FPort.PortFree -= FPort_PortFree;
                }

                this.FPort = value;
                if (this.FPort != null)
                {
                    this.FPort.PortOpen += FPort_PortOpen;
                    this.FPort.PortClose += FPort_PortClose;
                    this.FPort.DataReceived += FPort_DataReceived;
                    this.FPort.ErrorReceived += FPort_ErrorReceived;
                    this.FPort.PortBusy += FPort_PortBusy;
                    this.FPort.PortFree += FPort_PortFree;
                }
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

        private void FPort_PortFree(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FPort_PortBusy(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Append(string text)
        {
            this.GotoPosition(this.TextLength);
            this.AddText(text);
            this.GotoPosition(this.TextLength);
            this.EmptyUndoBuffer();
        }

        public void Clean()
        {
            this.Text = "";
            if(this.Port != null && this.Port.IsOpen)
            this.Port.WriteLine("");
        }

        private void FPort_PortClose(object sender, EventArgs e)
        {
        }

        private void FPort_PortOpen(object sender, EventArgs e)
        {
        }

        private void FPort_DataReceived(object sender, string data)
        {
           
            this.UpdateTerminal(data, "");
        }

        private void FPort_ErrorReceived(object sender, string data)
        {
            // this.UpdateTerminal(data);
        }

        public delegate string PreprocessEvent(string text);
        public PreprocessEvent Preprocess = null;


        const int LineLength = 180;
        char[] Buffer = new char[LineLength]; // new StringBuilder(" ", LineLength);
        int Cp = 0;
        char LastChar = (char)0;
        char LastMode = (char)0;
        public bool Locked = false;


        //public int FindPrompPos()
        //{
        //    Line line = this.Lines[this.LineFromPosition(this.CurrentPosition)];
        //    if()
        //}

        private char C0 = '\0';
        private char C1 = '\0';
        private char C2 = '\0';
        private char C3 = '\0';
        private delegate void UpdateTerminalEvent(string data, string message);
        public void UpdateTerminal(string data, string message)
        {
            if (this.Locked)
                return;

            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateTerminalEvent(UpdateTerminal), new object[] { data, message});
            }
            else
            {
                //if (this.Preprocess != null)
                //{
                //   text = this.Preprocess(text);
                //}

                //http://www.termsys.demon.co.uk/vtansi.htm

                //if (text[0] == '\b' && this.TextLength > 0)
                //{
                //    this.DeleteRange(this.TextLength - 1, 1);
                //}

                if (message != "")
                {
                    if (message.Contains("\r\n"))
                    {
                        this.AppendText(message);
                        this.PromptPos = this.TextLength;
                    }
                    else
                    {
                        this.DeleteRange(this.PromptPos, 100);
                        this.AppendText(message);
                    }
                    //this.PromptPos = this.TextLength;
                    //this.ActPos = this.PromptPos;
                    return;
                }

                //while (this.Lines.Count > 500)
                //{
                //    this.DeleteRange(0, this.Lines[0].EndPosition);
                //}

                string res = this.Text ;
         
                for (int i = 0; i < data.Length; i++)
                {
                    char c = data[i];
                    
                    // Todo: \[K
                    //this.C0 = this.C1;
                    //this.C1 = this.C2;
                    //this.C2 = this.C3;
                    //this.C3 = c;

                      

                    if (c == '\b' && res.Length >= 1)
                    {
                        res = res.Remove(res.Length - 1, 1);
                    }
                    else if (this.LastChar == 27)
                    {
                        if (c == '[')
                        {
                            LastMode = c;
                            continue;
                        }

                        if (LastMode == '[' && c == 'K')
                        {
                            LastMode = (char)0;
                            continue;
                        }
                    }
                    else if (c != 27)
                    {
                        res += data[i];
                    }

                    this.LastChar = data[i];
                }

                //this.AppendText(text);
                this.Text = res;
                this.GotoPosition(this.TextLength);
                //this.ActPos = this.TextLength;
                Line line = this.Lines[this.CurrentLine];
                if (line.Text.StartsWith(SerialPort.PROMPT))
                    this.PromptPos = line.Position + SerialPort.PROMPT.Length;
                else if (line.Text.StartsWith("... "))
                    this.PromptPos = this.TextLength;
                //else //if (line.Text.StartsWith("=== "))
                //    this.PromptPos = this.TextLength;

                this.PromptPos = Math.Max(this.PromptPos, this.TextLength);


                this.EmptyUndoBuffer();
                this.BeforeMouse = -1;
            }
        }
       
        private void AddToHistory()
        {
            string text = this.Lines[this.CurrentLine].Text;

            if (text.StartsWith(PySerial.PROMPT))
                text = text.Substring(PySerial.PROMPT.Length, text.Length - PySerial.PROMPT.Length);
            else if (text.StartsWith(PySerial.DOTS))
                text = text.Substring(PySerial.DOTS.Length, text.Length - PySerial.DOTS.Length);
            else return;

            text = text.Trim();

            if (text.Length > 0 && (this.History.Count == 0 || (this.History.Count > 0 && this.History.Last() != text)))
            {
                this.History.Add(text);
                this.HistoryPos = this.History.Count;
                this.HistoryModified = true;
                while(this.History.Count > 100)
                {
                    this.History.RemoveAt(0);
                }
            }
        }

        public void SaveHistory(string fname)
        {
            File.WriteAllText(fname, String.Join("\r\n", this.History));
            this.HistoryModified = false;
        }

        public void LoadHistory(string fname)
        {
            if (File.Exists(fname))
            {
                this.History.Clear();
                this.History.AddRange(File.ReadAllLines(fname));
                this.HistoryPos = this.History.Count;
                this.HistoryModified = false;
            }
        }

        private string GetAndRemoveLine(bool history)
        {
            Line line = this.Lines[this.CurrentLine];
            string text = this.GetTextRange(this.PromptPos, this.TextLength - this.PromptPos);
            //string text = this.GetTextRange(this.PromptPos, this.TextLength - this.PromptPos);
            //text = this.GetTextRange(line.Position + SerialPort.PROMPT.Length, this.TextLength - line.Position);
           
            if (history)
            {
                this.AddToHistory();
            }
            line = this.Lines[this.CurrentLine];
            this.DeleteRange(this.PromptPos, this.TextLength);
            return text;
        }

        protected override void WndProc(ref Message m)
        {           
            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN)
            {
                if (this.BeforeMouse == -1)
                    this.BeforeMouse = this.CurrentPosition;
            }
            base.WndProc(ref m);
        }

        private void Paste(string text)
        {
            //this.InsertText(this.CurrentPosition, text);
            //this.GotoPosition(this.CurrentPosition + text.Length);
            //return;

            //Line line = this.Lines[this.CurrentLine];
            //string  = this.GetTextRange(this.PromptPos, this.TextLength - this.PromptPos);

            string[] lines = text.Replace("\r", "").Split('\n');
            //foreach (string line in lines)
            //{
            //    this.Port.WriteLine(line);
            //}

            for (int i = 0; i < lines.Length - 1; i++)
            {
                this.Port.WriteLine(lines[i]);
            }

            this.Port.Write(lines[lines.Length - 1]);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Line line = null;

            if (this.BeforeMouse >= 0)
            {
               this.GotoPosition (this.BeforeMouse);
                this.BeforeMouse = -1;
            }

            //if (this.CurrentPosition < this.ActPos)
            //    this.CurrentPosition = this.ActPos;

            if (this.CurrentPosition < this.PromptPos)
                this.CurrentPosition = this.PromptPos;

            // this.ActPos = this.CurrentPosition;

            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
               // if (this.Port is PySerial)
                {
                    //PythonSerial port = this.Port as PythonSerial;

                    switch (keyData)
                    {
                        case Keys.Control | Keys.V:
                            if (Clipboard.ContainsText())
                            {
                                this.Paste(Clipboard.GetText());
                                //this.Port.Clean();
                                //this.Port.PasteMode();
                                //this.Port.ReadAllLines();
                                //byte[] buff = Encoding.UTF8.GetBytes(Clipboard.GetText().Replace("\r", ""));
                                //this.Port.Write(buff, 0, buff.Length);
                                //this.Port.SoftReset();

                                 

                            }
                            return true;

                            break;

                        case Keys.Control | Keys.A:
                            this.Port.EnterRawMode();                            
                            return true;

                        case Keys.Control | Keys.B:
                            //this.Port.Write(new byte[] { 2 });
                            this.Port.LeaveRawMode();
                            return true;

                        case Keys.Control | Keys.C:
                            //this.Port.Write(new byte[] { 3 });
                            this.Port.Interrupt();
                            return true;

                        case Keys.Control | Keys.D:
                            //this.Port.Write(new byte[] { 4 });
                            this.Port.SoftReset();
                            return true;

                        case Keys.Control | Keys.E:
                            //this.Port.Write(new byte[] { 5 });
                            this.Port.PasteMode();
                            return true;

                        //case Keys.Control | Keys.Tab:
                        //    this.Port.WriteLine(this.GetLine());
                        //    this.Port.Write(new byte[] { 9 });
                        //    return true;

                        case  Keys.Tab:
                            string l = this.GetAndRemoveLine(false);
                            this.Port.Write(l);
                            this.Port.Write(9);
                            return true;
                            //break;

                        case Keys.Down:
                            this.HistoryPos++;
                            if (this.HistoryPos > this.History.Count)
                                this.HistoryPos = this.History.Count;

                            if (this.HistoryPos <= 0)
                                this.HistoryPos = 1;

                            if (this.HistoryPos <= this.History.Count - 1)
                            {

                                this.DeleteRange(this.PromptPos, this.TextLength);
                                this.Append(this.History[this.HistoryPos]);

                            }
                            else
                            {
                                line = this.Lines[this.CurrentLine];
                                this.DeleteRange(this.PromptPos, this.TextLength);
                            }
                            return true;

                        case Keys.Up:
                            this.HistoryPos--;
                            if (this.HistoryPos < 0)
                                this.HistoryPos = 0;

                            if (this.HistoryPos >= this.History.Count)
                                this.HistoryPos = this.History.Count - 1;

                            if (this.HistoryPos >= 0)
                            {
                                this.DeleteRange(this.PromptPos, this.TextLength);
                                this.Append(this.History[this.HistoryPos]);
                            }
                            return true;

                        case Keys.Left:
                            if (this.PromptPos >= this.CurrentPosition)
                                return true;
                            break;

                        case Keys.Home:
                            this.CurrentPosition = this.PromptPos;
                            return true;

                        //case Keys.End:
                        //    return true;

                        case Keys.PageDown:
                            return true;

                        case Keys.PageUp:
                            return true;

                        case Keys.Back:
                            if (this.PromptPos == this.CurrentPosition)
                            {
                                this.Port.Write(8);
                                return true;
                            }
                            //else if()
                            break;

                        case Keys.Enter:
                            this.Port.WriteLine(this.GetAndRemoveLine(true));
                            return true;
                        //default:
                        //    if()

                        //    break;
                    }
                }
            }
            //KeysConverter kc = new KeysConverter();
            
            //string keyChar = kc.ConvertToString(keyData);
            //if(keyData != Keys.ShiftKey)
            //    this.Port.Write(keyChar.ToLower());
            //else 
           
            bool res = base.ProcessCmdKey(ref msg, keyData);
            //this.ActPos = this.CurrentPosition;
            return res;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Terminal
            // 
            this.IndentationGuides = ScintillaNET.IndentView.Real;
            this.IndentWidth = 4;
            this.Lexer = ScintillaNET.Lexer.Python;
            this.ResumeLayout(false);

        }
    }
}
