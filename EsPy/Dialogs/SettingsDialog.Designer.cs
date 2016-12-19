namespace EsPy.Dialogs
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbPythonPath = new System.Windows.Forms.TextBox();
            this.btnPythonPath = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ShowServer = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Python:";
            // 
            // tbPythonPath
            // 
            this.tbPythonPath.Location = new System.Drawing.Point(16, 30);
            this.tbPythonPath.Name = "tbPythonPath";
            this.tbPythonPath.Size = new System.Drawing.Size(503, 20);
            this.tbPythonPath.TabIndex = 0;
            // 
            // btnPythonPath
            // 
            this.btnPythonPath.Location = new System.Drawing.Point(525, 28);
            this.btnPythonPath.Name = "btnPythonPath";
            this.btnPythonPath.Size = new System.Drawing.Size(25, 23);
            this.btnPythonPath.TabIndex = 1;
            this.btnPythonPath.Text = "...";
            this.btnPythonPath.UseVisualStyleBackColor = true;
            this.btnPythonPath.Click += new System.EventHandler(this.btnPythonPath_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(393, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(475, 227);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // ShowServer
            // 
            this.ShowServer.AutoSize = true;
            this.ShowServer.Location = new System.Drawing.Point(16, 184);
            this.ShowServer.Name = "ShowServer";
            this.ShowServer.Size = new System.Drawing.Size(186, 17);
            this.ShowServer.TabIndex = 2;
            this.ShowServer.Text = "Show Py Server (Restart required)";
            this.ShowServer.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 262);
            this.Controls.Add(this.ShowServer);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnPythonPath);
            this.Controls.Add(this.tbPythonPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPythonPath;
        private System.Windows.Forms.Button btnPythonPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox ShowServer;
    }
}