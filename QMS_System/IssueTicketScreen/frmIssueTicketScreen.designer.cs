namespace QMS_System.IssueTicketScreen
{
    partial class frmIssueTicketScreen
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuSetupInterface = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHideInterface = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetupInterface,
            this.menuHideInterface,
            this.menuExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuSetupInterface
            // 
            this.menuSetupInterface.Name = "menuSetupInterface";
            this.menuSetupInterface.Size = new System.Drawing.Size(108, 20);
            this.menuSetupInterface.Text = "Cài đặt giao diện";
            this.menuSetupInterface.Click += new System.EventHandler(this.menuSetupInterface_Click);
            // 
            // menuHideInterface
            // 
            this.menuHideInterface.Name = "menuHideInterface";
            this.menuHideInterface.Size = new System.Drawing.Size(86, 20);
            this.menuHideInterface.Text = "Ẩn giao diện";
            this.menuHideInterface.Click += new System.EventHandler(this.menuHideInterface_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(50, 20);
            this.menuExit.Text = "Thoát";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // frmIssueTicketScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmIssueTicketScreen";
            this.Text = "Màn hình cấp vé";
            this.Load += new System.EventHandler(this.frmIssueTicketScreen_Load);
            this.ClientSizeChanged += new System.EventHandler(this.frmIssueTicketScreen_ClientSizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmIssueTicketScreen_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuSetupInterface;
        private System.Windows.Forms.ToolStripMenuItem menuHideInterface;
        private System.Windows.Forms.ToolStripMenuItem menuExit;

    }
}

