namespace QMS_System.IssueTicketScreen
{
    partial class frmNumOfColumn
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
            this.UpDownNumOfColumn = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumOfColumn)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Số cột :";
            // 
            // UpDownNumOfColumn
            // 
            this.UpDownNumOfColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpDownNumOfColumn.Location = new System.Drawing.Point(108, 43);
            this.UpDownNumOfColumn.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UpDownNumOfColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownNumOfColumn.Name = "UpDownNumOfColumn";
            this.UpDownNumOfColumn.Size = new System.Drawing.Size(84, 21);
            this.UpDownNumOfColumn.TabIndex = 1;
            this.UpDownNumOfColumn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(48, 115);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 32);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Áp dụng";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(141, 115);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmNumOfColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 182);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.UpDownNumOfColumn);
            this.Controls.Add(this.label1);
            this.Name = "frmNumOfColumn";
            this.Text = "Số cột";
            this.Load += new System.EventHandler(this.frmNumOfColumn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumOfColumn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown UpDownNumOfColumn;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnExit;
    }
}