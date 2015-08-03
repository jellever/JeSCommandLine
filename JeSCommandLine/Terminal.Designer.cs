namespace JeSCommandLine
{
    partial class JeSTerminal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JeSTerminal));
            this.terminalControl1 = new JeSCommandLine.TerminalControl();
            this.SuspendLayout();
            // 
            // terminalControl1
            // 
            this.terminalControl1.BackColor = System.Drawing.Color.Black;
            this.terminalControl1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.terminalControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalControl1.Font = new System.Drawing.Font("DejaVu Sans Mono", 9.75F);
            this.terminalControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.terminalControl1.Location = new System.Drawing.Point(0, 0);
            this.terminalControl1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.terminalControl1.Name = "terminalControl1";
            this.terminalControl1.Size = new System.Drawing.Size(876, 391);
            this.terminalControl1.TabIndex = 0;
            this.terminalControl1.Text = "";
            // 
            // JeSTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(876, 391);
            this.Controls.Add(this.terminalControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JeSTerminal";
            this.Text = "Multi-Tab Command Line";
            this.Load += new System.EventHandler(this.MainUI_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TerminalControl terminalControl1;

    }
}

