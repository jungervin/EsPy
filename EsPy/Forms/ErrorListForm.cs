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
using WeifenLuo.WinFormsUI.Docking;

namespace EsPy.Forms
{
    public partial class ErrorListForm : DockContent
    {
        public ErrorListForm()
        {
            InitializeComponent();
            this.DockAreas = DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop
               | DockAreas.Document;
                   
            this.HideOnClose = true;
        }

        public void MkList(string list, string filename)
        {
            this.listView1.Items.Clear();
            string[] lines = list.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string module = "";
            for(int i = 0; i < lines.Length-1; i++)
            {
                if (lines[i + 1] == "")
                    return;

                string line = lines[i];
                if (line.StartsWith("************* Module "))
                {
                    module = line.Remove(0, "************* Module ".Length) + ".py";
                    continue;
                }

                ErrorItem ei = new ErrorItem(filename, line);
                ListViewItem item = new ListViewItem();
                if (ei.MsgType == "E" || ei.MsgType == "F")
                    item.ImageIndex = 0;
                else item.ImageIndex = 1;
                item.SubItems.Add(ei.Desc);
                item.SubItems.Add(Path.GetFileName(ei.FileName));
                item.Tag = ei;
                this.listView1.Items.Add(item);


            }
            this.listView1.Columns[1].Width = -2; // .AutoResizeColumn(1, -2);
            //this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent );
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ErrorItem ei = (this.listView1.SelectedItems[0].Tag as ErrorItem);

                IDocument doc = Globals.MainForm.FindDocument(ei.FileName);
                if(doc == null)
                    doc =Globals.MainForm.OpenFromFile(ei.FileName, EditorForm.EditorFileFormats);

                if (doc != null)

                if (doc is EditorForm)
                {
                        //(doc as EditorForm).Show(Globals.MainForm.dockPanel1);

                        Scintilla s = (doc as EditorForm).scintilla;
                    Line line = s.Lines[ei.Line-1];
                        
                    s.GotoPosition(line.Position + ei.Column);
                        s.Focus();
                }
            }
        }
    }

    public class ErrorItem
    {
        public string FileName = "";
        public string MsgType = "";
        public int Line = 0;
        public int Column = 0;
        public string Desc = "";

        public ErrorItem(string filename, string line)
        {
            this.FileName = filename;
            this.MsgType = line.Substring(0, 1);
            int.TryParse(line.Substring(2, 3), out this.Line);
            int.TryParse(line.Substring(6, 2), out this.Column);
            this.Desc = line.Substring(10, line.Length - 10);
        }

    }
}
