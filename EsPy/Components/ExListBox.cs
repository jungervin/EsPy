using EsPy.Python.Jedi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Components
{
    public class ExListBox : ListBox
    {
        private ImageList imageList1;
        private System.ComponentModel.IContainer components;

        public class ExListBoxItem
        {
            public string Text = "";
            public int ImageIndex = -1;

            public override string ToString()
            {
                return this.Text;
            }
            
        }

        public ExListBox()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index >= 0)
            {
                ExListBoxItem item = (ExListBoxItem)Items[e.Index];
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ResumeLayout(false);

        }

        //protected override bool ShowWithoutActivation
        //{
        //    get { return true; }
        //}

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
    }
}
