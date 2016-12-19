using EsPy.Units;
using EsPy.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsPy.Dialogs
{
    public partial class PyFileManager : Form
    {
        public PyFileManager()
        {
            InitializeComponent();
        }

        public PySerial Port
        { get; set; }
   

        private List<PyFile> Files = null;
        private void Fill()
        {
            this.listView1.Items.Clear();

            if (this.Port != null && this.Port.IsOpen)
            {
                
                ListViewItem item = new ListViewItem();
                ResultStatus cwd = this.Port.Cwd();
                if (cwd.Result == ResultStatus.Statuses.Success)
                {
                    this.Path.Text = cwd.ToString().Replace("'", "");
                    if (Path.Text == "")
                        this.Path.Text = "/";
                }
                else
                {
                    Helpers.ErrorBox(cwd);
                    return;
                }

                ResultStatus files = this.Port.Ls();

                if (files.Result == ResultStatus.Statuses.Success)
                {
                    if (this.Path.Text != "/")
                    {
                        item.Text = "..";
                        item.SubItems.Add("");
                        item.ImageIndex = 0;
                        listView1.Items.Add(item);

                    }


                    this.Files = files.Data as List<PyFile>;
                    foreach (PyFile f in this.Files)
                    {
                        item = new ListViewItem();

                        item.Text = f.FileName;
                        
                        if (f.IsDir)
                        {
                            item.ImageIndex = 2;
                            item.SubItems.Add("Folder");
                        }
                        else if (f.IsFile)
                        {
                            item.ImageIndex = 3;
                            item.SubItems.Add(f.FileSize.ToString());
                        }

                        this.listView1.Items.Add(item);
                        item.Tag = f;
                    }

                    if (listView1.Items.Count > 0)
                    {
                        this.listView1.Items[0].Selected = true;
                        this.listView1.Items[0].Focused = true;
                    }
                    //this.listView1.Columns[1].Width = -2;
                    
                }
                else
                {
                    Helpers.ErrorBox(files);
                }
            }
        }
        
      
        private void PyFileManager_Load(object sender, EventArgs e)
        {
            this.Fill();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                string path = "";
                if (this.listView1.SelectedItems[0].Tag is PyFile)
                {
                    PyFile f = this.listView1.SelectedItems[0].Tag as PyFile;
                    if (f.IsDir)
                    {
                        path = f.FileName;
                    }
                }
                else
                {
                    path = this.listView1.SelectedItems[0].Text;
                }

                if (path != "")
                {
                    ResultStatus res = this.Port.Cd(path);
                    if (res.Result == ResultStatus.Statuses.Error)
                        Helpers.ErrorBox(res);
                    else
                        this.Fill();
                }

            }
        }

        private void bntMkDir_Click(object sender, EventArgs e)
        {
            InputDialog d = new InputDialog();
            d.Text = "MkDir";
            d.label1.Text = "Directory name:";
            if (d.ShowDialog() == DialogResult.OK)
            {
                ResultStatus res = this.Port.MkDir(d.textBox1.Text);
                if (res.Result == ResultStatus.Statuses.Error)
                    Helpers.ErrorBox(res);
                else
                    this.Fill();
            }
            d.Dispose();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                PyFile f = this.listView1.SelectedItems[0].Tag as PyFile;
                if (f != null)
                {
                    int i = this.listView1.SelectedItems[0].Index;
                    InputDialog d = new InputDialog();
                    d.Text = "Rename";
                    d.label1.Text = "New name:";
                    if (d.ShowDialog() == DialogResult.OK)
                    {
                        ResultStatus res = this.Port.Rename(f.FileName, d.textBox1.Text);
                        if (res.Result == ResultStatus.Statuses.Error)
                           Helpers.ErrorBox(res);
                        else
                        {
                            this.Fill();

                            foreach (ListViewItem item in this.listView1.Items)
                            {
                                item.Selected = (item.Tag is PyFile && (item.Tag as PyFile).FileName == d.textBox1.Text);
                            }
                        }
                    }
                    d.Dispose();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                PyFile f = this.listView1.SelectedItems[0].Tag as PyFile;
                if (f != null)
                {
                    ResultStatus res = null;
                    if (MessageBox.Show("Are you sure?", "Delete",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (f.IsDir)
                        {
                            res = this.Port.RmDir(f.FileName);
                        }
                        else if (f.IsFile)
                        {
                            res = this.Port.Remove(f.FileName);
                        }
                        else
                        {
                            MessageBox.Show("Unknow type!");
                            return;
                        }
                        if (res.Result == ResultStatus.Statuses.Success)
                        {
                            this.Fill();
                        }
                        else
                        {
                            Helpers.ErrorBox(res);
                        }
                    }
                    
                }
            }
        }

        //ProgressDialog ProgressDialog = null;
        //private void Port_FileProgress(object sender, FileProgressEventArgs e)
        //{
        //    this.ProgressDialog.label1.Text = e.FName + ": " + e.Bytes.ToString() + " / " + e.Size.ToString();
        //    this.ProgressDialog.progressBar1.Maximum = e.Size;
        //    this.ProgressDialog.progressBar1.Value = e.Bytes;
        //    Application.DoEvents();
        //}

        //private void Port_BeginFileProgress(object sender, FileProgressEventArgs e)
        //{
        //    if (ProgressDialog == null)
        //        this.ProgressDialog = new ProgressDialog();
        //    this.Port_FileProgress(sender, e);
        //    this.ProgressDialog.Show();
        //    Application.DoEvents();
        //}   

        //private void Port_EndFileProgress(object sender, FileProgressEventArgs e)
        //{
        //    Application.DoEvents();
        //    this.ProgressDialog.Dispose();
        //    this.ProgressDialog = null;
        //}

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
               try
                {
                    byte[] buff = File.ReadAllBytes(d.FileName);

                    ProgressDialog p = new ProgressDialog();
                    p.Port = this.Port;
                    p.Mode = ProgressDialog.Modes.Upload;
                    p.FileName = System.IO.Path.GetFileName(d.FileName);
                    p.Buffer = buff;
                    if(p.ShowDialog() == DialogResult.OK)
                        this.Fill();
                    p.Dispose();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorBox(ex);
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                if (this.listView1.SelectedItems[0].Tag is PyFile)
                {
                    PyFile f = this.listView1.SelectedItems[0].Tag as PyFile;
                    if (f.IsFile)
                    {
                        ProgressDialog p = new ProgressDialog();
                        p.Port = this.Port;
                        p.Mode = ProgressDialog.Modes.Download;
                        p.FileName = System.IO.Path.GetFileName(f.FileName);

                        if (p.ShowDialog() == DialogResult.OK)
                        {
                            SaveFileDialog d = new SaveFileDialog();
                            d.FileName = f.FileName;                         
                            if (d.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    File.WriteAllBytes(d.FileName, p.Buffer);
                                }
                                catch (Exception ex)
                                {
                                    Helpers.ErrorBox(ex);
                                }
                                d.Dispose();
                            }
                            p.Dispose();
                        }
                    }
                }
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.listView1_DoubleClick(this, null);
                e.Handled = true;
            }
        }
    }
}
