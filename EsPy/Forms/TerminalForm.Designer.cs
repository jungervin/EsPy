namespace EsPy.Forms
{
    partial class TerminalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

         

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnClean = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnShowEol = new System.Windows.Forms.ToolStripMenuItem();
            this.mnShowWhitespace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.osToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmSoftReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmClean = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmShowEOL = new System.Windows.Forms.ToolStripMenuItem();
            this.cmShowWhitespace = new System.Windows.Forms.ToolStripMenuItem();
            this.scintilla = new EsPy.Components.Terminal();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnEdit,
            this.mnView});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(664, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // mnEdit
            // 
            this.mnEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnUndo,
            this.mnRedo,
            this.toolStripMenuItem2,
            this.mnCut,
            this.mnCopy,
            this.mnPaste,
            this.mnDelete,
            this.mnClean,
            this.toolStripMenuItem3,
            this.mnSelectAll,
            this.toolStripMenuItem7});
            this.mnEdit.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.mnEdit.Name = "mnEdit";
            this.mnEdit.Size = new System.Drawing.Size(39, 20);
            this.mnEdit.Text = "&Edit";
            // 
            // mnUndo
            // 
            this.mnUndo.Image = ((System.Drawing.Image)(resources.GetObject("mnUndo.Image")));
            this.mnUndo.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnUndo.MergeIndex = 0;
            this.mnUndo.Name = "mnUndo";
            this.mnUndo.ShortcutKeyDisplayString = "Ctrl+Z";
            this.mnUndo.Size = new System.Drawing.Size(144, 22);
            this.mnUndo.Text = "Undo";
            // 
            // mnRedo
            // 
            this.mnRedo.Image = ((System.Drawing.Image)(resources.GetObject("mnRedo.Image")));
            this.mnRedo.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnRedo.MergeIndex = 1;
            this.mnRedo.Name = "mnRedo";
            this.mnRedo.ShortcutKeyDisplayString = "Ctrl+Y";
            this.mnRedo.Size = new System.Drawing.Size(144, 22);
            this.mnRedo.Text = "Redo";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripMenuItem2.MergeIndex = 2;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(141, 6);
            // 
            // mnCut
            // 
            this.mnCut.Image = ((System.Drawing.Image)(resources.GetObject("mnCut.Image")));
            this.mnCut.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnCut.MergeIndex = 3;
            this.mnCut.Name = "mnCut";
            this.mnCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.mnCut.Size = new System.Drawing.Size(144, 22);
            this.mnCut.Text = "Cut";
            // 
            // mnCopy
            // 
            this.mnCopy.Image = ((System.Drawing.Image)(resources.GetObject("mnCopy.Image")));
            this.mnCopy.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnCopy.MergeIndex = 4;
            this.mnCopy.Name = "mnCopy";
            this.mnCopy.ShortcutKeyDisplayString = "";
            this.mnCopy.Size = new System.Drawing.Size(144, 22);
            this.mnCopy.Text = "Copy";
            // 
            // mnPaste
            // 
            this.mnPaste.Enabled = false;
            this.mnPaste.Image = ((System.Drawing.Image)(resources.GetObject("mnPaste.Image")));
            this.mnPaste.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnPaste.MergeIndex = 5;
            this.mnPaste.Name = "mnPaste";
            this.mnPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.mnPaste.Size = new System.Drawing.Size(144, 22);
            this.mnPaste.Text = "Paste";
            // 
            // mnDelete
            // 
            this.mnDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnDelete.MergeIndex = 6;
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.ShortcutKeyDisplayString = "Del";
            this.mnDelete.Size = new System.Drawing.Size(144, 22);
            this.mnDelete.Text = "Delete";
            // 
            // mnClean
            // 
            this.mnClean.Image = ((System.Drawing.Image)(resources.GetObject("mnClean.Image")));
            this.mnClean.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnClean.MergeIndex = 7;
            this.mnClean.Name = "mnClean";
            this.mnClean.Size = new System.Drawing.Size(144, 22);
            this.mnClean.Text = "Clean";
            this.mnClean.Click += new System.EventHandler(this.mnClean_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripMenuItem3.MergeIndex = 8;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(141, 6);
            // 
            // mnSelectAll
            // 
            this.mnSelectAll.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnSelectAll.MergeIndex = 9;
            this.mnSelectAll.Name = "mnSelectAll";
            this.mnSelectAll.ShortcutKeyDisplayString = "";
            this.mnSelectAll.Size = new System.Drawing.Size(144, 22);
            this.mnSelectAll.Text = "Select All";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripMenuItem7.MergeIndex = 10;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(141, 6);
            // 
            // mnView
            // 
            this.mnView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.mnShowEol,
            this.mnShowWhitespace});
            this.mnView.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.mnView.Name = "mnView";
            this.mnView.Size = new System.Drawing.Size(44, 20);
            this.mnView.Text = "&View";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 6);
            // 
            // mnShowEol
            // 
            this.mnShowEol.Name = "mnShowEol";
            this.mnShowEol.Size = new System.Drawing.Size(167, 22);
            this.mnShowEol.Text = "Show EOL";
            this.mnShowEol.Click += new System.EventHandler(this.mnShowEol_Click);
            // 
            // mnShowWhitespace
            // 
            this.mnShowWhitespace.Name = "mnShowWhitespace";
            this.mnShowWhitespace.Size = new System.Drawing.Size(167, 22);
            this.mnShowWhitespace.Text = "Show Whitespace";
            this.mnShowWhitespace.Click += new System.EventHandler(this.mnShowWhitespace_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(664, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // osToolStripMenuItem
            // 
            this.osToolStripMenuItem.Name = "osToolStripMenuItem";
            this.osToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.osToolStripMenuItem.Text = "os";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmSoftReset,
            this.toolStripSeparator5,
            this.cmUndo,
            this.cmRedo,
            this.toolStripSeparator4,
            this.cmCut,
            this.cmCopy,
            this.cmPaste,
            this.cmDelete,
            this.cmClean,
            this.toolStripSeparator1,
            this.cmSelectAll,
            this.toolStripSeparator3,
            this.advancedToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 248);
            // 
            // cmSoftReset
            // 
            this.cmSoftReset.Enabled = false;
            this.cmSoftReset.Name = "cmSoftReset";
            this.cmSoftReset.Size = new System.Drawing.Size(144, 22);
            this.cmSoftReset.Text = "Soft reset";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(141, 6);
            // 
            // cmUndo
            // 
            this.cmUndo.Enabled = false;
            this.cmUndo.Image = ((System.Drawing.Image)(resources.GetObject("cmUndo.Image")));
            this.cmUndo.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmUndo.MergeIndex = 0;
            this.cmUndo.Name = "cmUndo";
            this.cmUndo.ShortcutKeyDisplayString = "Ctrl+Z";
            this.cmUndo.Size = new System.Drawing.Size(144, 22);
            this.cmUndo.Text = "Undo";
            this.cmUndo.Click += new System.EventHandler(this.mnUndo_Click);
            // 
            // cmRedo
            // 
            this.cmRedo.Enabled = false;
            this.cmRedo.Image = ((System.Drawing.Image)(resources.GetObject("cmRedo.Image")));
            this.cmRedo.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmRedo.MergeIndex = 1;
            this.cmRedo.Name = "cmRedo";
            this.cmRedo.ShortcutKeyDisplayString = "Ctrl+Y";
            this.cmRedo.Size = new System.Drawing.Size(144, 22);
            this.cmRedo.Text = "Redo";
            this.cmRedo.Click += new System.EventHandler(this.mnRedo_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator4.MergeIndex = 2;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // cmCut
            // 
            this.cmCut.Enabled = false;
            this.cmCut.Image = ((System.Drawing.Image)(resources.GetObject("cmCut.Image")));
            this.cmCut.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmCut.MergeIndex = 3;
            this.cmCut.Name = "cmCut";
            this.cmCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.cmCut.Size = new System.Drawing.Size(144, 22);
            this.cmCut.Text = "Cut";
            this.cmCut.Click += new System.EventHandler(this.mnCut_Click);
            // 
            // cmCopy
            // 
            this.cmCopy.Enabled = false;
            this.cmCopy.Image = ((System.Drawing.Image)(resources.GetObject("cmCopy.Image")));
            this.cmCopy.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmCopy.MergeIndex = 4;
            this.cmCopy.Name = "cmCopy";
            this.cmCopy.ShortcutKeyDisplayString = "";
            this.cmCopy.Size = new System.Drawing.Size(144, 22);
            this.cmCopy.Text = "Copy";
            this.cmCopy.Click += new System.EventHandler(this.mnCopy_Click);
            // 
            // cmPaste
            // 
            this.cmPaste.Enabled = false;
            this.cmPaste.Image = ((System.Drawing.Image)(resources.GetObject("cmPaste.Image")));
            this.cmPaste.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmPaste.MergeIndex = 5;
            this.cmPaste.Name = "cmPaste";
            this.cmPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.cmPaste.Size = new System.Drawing.Size(144, 22);
            this.cmPaste.Text = "Paste";
            this.cmPaste.Click += new System.EventHandler(this.mnPaste_Click);
            // 
            // cmDelete
            // 
            this.cmDelete.Enabled = false;
            this.cmDelete.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmDelete.MergeIndex = 6;
            this.cmDelete.Name = "cmDelete";
            this.cmDelete.ShortcutKeyDisplayString = "Del";
            this.cmDelete.Size = new System.Drawing.Size(144, 22);
            this.cmDelete.Text = "Delete";
            this.cmDelete.Click += new System.EventHandler(this.mnDelete_Click);
            // 
            // cmClean
            // 
            this.cmClean.Image = global::EsPy.Properties.Resources.editclear;
            this.cmClean.Name = "cmClean";
            this.cmClean.Size = new System.Drawing.Size(144, 22);
            this.cmClean.Text = "Clean";
            this.cmClean.Click += new System.EventHandler(this.mnClean_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // cmSelectAll
            // 
            this.cmSelectAll.Enabled = false;
            this.cmSelectAll.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.cmSelectAll.MergeIndex = 8;
            this.cmSelectAll.Name = "cmSelectAll";
            this.cmSelectAll.ShortcutKeyDisplayString = "";
            this.cmSelectAll.Size = new System.Drawing.Size(144, 22);
            this.cmSelectAll.Text = "Select All";
            this.cmSelectAll.Click += new System.EventHandler(this.mnSelectAll_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmShowEOL,
            this.cmShowWhitespace});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // cmShowEOL
            // 
            this.cmShowEOL.Name = "cmShowEOL";
            this.cmShowEOL.Size = new System.Drawing.Size(167, 22);
            this.cmShowEOL.Text = "Show EOL";
            this.cmShowEOL.Click += new System.EventHandler(this.mnShowEol_Click);
            // 
            // cmShowWhitespace
            // 
            this.cmShowWhitespace.Name = "cmShowWhitespace";
            this.cmShowWhitespace.Size = new System.Drawing.Size(167, 22);
            this.cmShowWhitespace.Text = "Show Whitespace";
            this.cmShowWhitespace.Click += new System.EventHandler(this.mnShowWhitespace_Click);
            // 
            // scintilla
            // 
            this.scintilla.AutoCAutoHide = false;
            this.scintilla.AutoCChooseSingle = true;
            this.scintilla.AutomaticFold = ((ScintillaNET.AutomaticFold)(((ScintillaNET.AutomaticFold.Show | ScintillaNET.AutomaticFold.Click) 
            | ScintillaNET.AutomaticFold.Change)));
            this.scintilla.CompletionEnabled = false;
            this.scintilla.ContextMenuStrip = this.contextMenuStrip1;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.EolMode = ScintillaNET.Eol.Cr;
            this.scintilla.IndentationGuides = ScintillaNET.IndentView.LookForward;
            this.scintilla.IndentWidth = 4;
            this.scintilla.Lexer = ScintillaNET.Lexer.Python;
            this.scintilla.Location = new System.Drawing.Point(0, 25);
            this.scintilla.MouseDwellTime = 500;
            this.scintilla.Name = "scintilla";
            this.scintilla.Port = null;
            this.scintilla.ReadOnly = true;
            this.scintilla.Size = new System.Drawing.Size(664, 237);
            this.scintilla.TabIndex = 2;
            this.scintilla.ViewWhitespace = ScintillaNET.WhitespaceMode.VisibleAlways;
            this.scintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.terminal_UpdateUI);
            // 
            // TerminalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 262);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TerminalForm";
            this.Text = "Terminal";
            this.Activated += new System.EventHandler(this.TerminalForm_Activated);
            this.Load += new System.EventHandler(this.TerminalForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        public Components.Terminal scintilla;
        private System.Windows.Forms.ToolStripMenuItem mnEdit;
        private System.Windows.Forms.ToolStripMenuItem mnUndo;
        private System.Windows.Forms.ToolStripMenuItem mnRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnCut;
        private System.Windows.Forms.ToolStripMenuItem mnCopy;
        private System.Windows.Forms.ToolStripMenuItem mnPaste;
        private System.Windows.Forms.ToolStripMenuItem mnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem mnClean;
        private System.Windows.Forms.ToolStripMenuItem osToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnView;
        private System.Windows.Forms.ToolStripMenuItem mnShowEol;
        private System.Windows.Forms.ToolStripMenuItem mnShowWhitespace;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        protected System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmSoftReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem cmUndo;
        private System.Windows.Forms.ToolStripMenuItem cmRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmCut;
        private System.Windows.Forms.ToolStripMenuItem cmCopy;
        private System.Windows.Forms.ToolStripMenuItem cmPaste;
        private System.Windows.Forms.ToolStripMenuItem cmDelete;
        private System.Windows.Forms.ToolStripMenuItem cmClean;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmShowEOL;
        private System.Windows.Forms.ToolStripMenuItem cmShowWhitespace;
    }
}