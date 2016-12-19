using EsPy.Python.Jedi;
using EsPy.Utility;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EsPy.Utility.TextHelper;

namespace EsPy.Components.Completion
{
    public class CompletionListBox : ListBox
    {

        Form Form = new Form();
        public CompletionListBox(Scintilla scintilla)
        {
            this.InitializeComponent();

            if (scintilla == null)
                throw new ArgumentNullException();

            this.Scintilla = scintilla;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.Dock = DockStyle.Fill;

            int word_start_pos = Utility.TextHelper.KeywordStartPosition(this.Scintilla.Text,this.Scintilla.CurrentPosition - 1) + 1;
            int x = this.Scintilla.PointXFromPosition(word_start_pos);
            int y = this.Scintilla.PointYFromPosition(word_start_pos);
            
            Point p = this.Scintilla.PointToScreen(new Point(x, y));
            this.Form.Owner = Globals.MainForm;
            
            this.Form.FormBorderStyle = FormBorderStyle.None;
            this.Form.TopLevel = true;this.Form.Show();
            this.Form.Top = p.Y + (int)(this.Scintilla.Font.Height * 1.2);
            this.Form.Left = p.X;
            this.Form.Height = this.Height;
            this.Form.Controls.Add(this);

            this.Form.Deactivate += Form_Deactivate;

        }

        private void Form_Deactivate(object sender, EventArgs e)
        {
            this.Form.Deactivate -= Form_Deactivate;
            this.Form.Close();
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index >= 0)
            {
                BaseDefinition item = (BaseDefinition)Items[e.Index];
                if (item.ImageIndex != -1 && this.imageList1.Images.Count > item.ImageIndex)
                {
                    Size image_size = imageList1.ImageSize;
                    this.imageList1.Draw(e.Graphics, e.Bounds.Left, e.Bounds.Top, item.ImageIndex);
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                        e.Bounds.Left + image_size.Width, e.Bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                       e.Bounds.Left, e.Bounds.Top);
                }
            }
        }

        private ImageList imageList1;

        //private ImageList FImageList;
        //public ImageList ImageList
        //{
        //    get { return FImageList; }
        //    set { FImageList = value; }
        //}



        public BaseDefinition SelectedDefinition
        { get { return this.SelectedItem as BaseDefinition; } }


        public Words Words
        { get; set; }

        private void DoFilter()
        {
            var defs = this.Definitions.Where(k => k.name.StartsWith(this.Words.Filter)).ToArray();

            this.Items.Clear();
            this.Items.AddRange(defs);

            for (int i = 0; i < this.Items.Count; i++)
            {

                if (((this.Items[i] as BaseDefinition).name.StartsWith(Words.Filter)))
                {
                    this.SelectedIndex = i;
                    break;
                }
            }
        }

        private List<BaseDefinition> FDefinitions = null;
        public List<BaseDefinition> Definitions
        {
            get { return this.FDefinitions; }
            set
            {
                this.Items.Clear();
                this.FDefinitions = value;

                // this.Items.AddRange(value.ToArray());
                this.DoFilter();
                this.Focus();// .Focused = true; //.SelectedIndex = 0;
            }
        }
        private void SelectItem(int index)
        {
            if (this.Items.Count > 0)
            {
                if (index < 0 || index > this.Items.Count - 1)
                    index = 0;

                if (index < this.Items.Count)
                    this.SelectedIndex = index;
            }
        }

        public void SelectNextItem()
        {
            if (this.Items.Count > 0)
            {
                if (this.SelectedIndex < 0)
                {
                    this.SelectedIndex = 0;
                    return;
                }
                if (this.SelectedIndex < this.Items.Count - 1)
                    this.SelectedIndex++;
            }
        }

        public void SelectPreviousItem()
        {
            if (this.Items.Count > 0)
            {
                if (this.SelectedIndex < 0)
                {
                    this.SelectedIndex = 0;
                    return;
                }
                if (this.SelectedIndex > 0)
                    this.SelectedIndex--;
            }
        }

        public Scintilla Scintilla
        { get; set; }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        protected override bool ProcessCmdKey(ref Message msg, Keys key)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                switch (key)
                {
                    case Keys.Up:
                        this.SelectPreviousItem();
                        return true;

                    case Keys.Down:
                        this.SelectNextItem();
                        return true;

                    case Keys.Escape:
                        this.Form.Close();
                        return true;

                    case Keys.Enter:
                        this.Form.Close();
                        return true;
                }

                PostMessage(this.Scintilla.Handle, msg.Msg, msg.WParam, msg.LParam);
                return true;
            }
            return base.ProcessCmdKey(ref msg, key);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompletionListBox));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "module.png");
            this.imageList1.Images.SetKeyName(1, "function.png");
            // 
            // CompletionListBox
            // 
            this.ItemHeight = 16;
            this.Size = new System.Drawing.Size(96, 96);
            this.ResumeLayout(false);

        }
        private System.ComponentModel.IContainer components;
    }
}
