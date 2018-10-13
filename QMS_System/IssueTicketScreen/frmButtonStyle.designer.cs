namespace QMS_System.IssueTicketScreen
{
    partial class frmButtonStyle
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
            this.label2 = new System.Windows.Forms.Label();
            this.UpDownButtonWidth = new System.Windows.Forms.NumericUpDown();
            this.UpDownButtonHeight = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnFontStyle = new System.Windows.Forms.Button();
            this.btnButtonBackColor = new System.Windows.Forms.Button();
            this.btnForeColor = new System.Windows.Forms.Button();
            this.UpDownButtonSpace = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSampleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonSpace)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chiều rộng";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Chiều cao";
            // 
            // UpDownButtonWidth
            // 
            this.UpDownButtonWidth.Location = new System.Drawing.Point(15, 27);
            this.UpDownButtonWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.UpDownButtonWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownButtonWidth.Name = "UpDownButtonWidth";
            this.UpDownButtonWidth.Size = new System.Drawing.Size(111, 20);
            this.UpDownButtonWidth.TabIndex = 2;
            this.UpDownButtonWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownButtonWidth.ValueChanged += new System.EventHandler(this.UpDownButtonWidth_ValueChanged);
            // 
            // UpDownButtonHeight
            // 
            this.UpDownButtonHeight.Location = new System.Drawing.Point(15, 69);
            this.UpDownButtonHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.UpDownButtonHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownButtonHeight.Name = "UpDownButtonHeight";
            this.UpDownButtonHeight.Size = new System.Drawing.Size(111, 20);
            this.UpDownButtonHeight.TabIndex = 3;
            this.UpDownButtonHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownButtonHeight.ValueChanged += new System.EventHandler(this.UpDownButtonHeight_ValueChanged);
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(123, 251);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(104, 42);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Lưu lại";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(243, 251);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(104, 42);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnFontStyle
            // 
            this.btnFontStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFontStyle.Location = new System.Drawing.Point(15, 136);
            this.btnFontStyle.Name = "btnFontStyle";
            this.btnFontStyle.Size = new System.Drawing.Size(111, 26);
            this.btnFontStyle.TabIndex = 6;
            this.btnFontStyle.Text = "Kiểu chữ";
            this.btnFontStyle.UseVisualStyleBackColor = true;
            this.btnFontStyle.Click += new System.EventHandler(this.btnFontStyle_Click);
            // 
            // btnButtonBackColor
            // 
            this.btnButtonBackColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnButtonBackColor.Location = new System.Drawing.Point(15, 200);
            this.btnButtonBackColor.Name = "btnButtonBackColor";
            this.btnButtonBackColor.Size = new System.Drawing.Size(111, 26);
            this.btnButtonBackColor.TabIndex = 7;
            this.btnButtonBackColor.Text = "Màu nút";
            this.btnButtonBackColor.UseVisualStyleBackColor = true;
            this.btnButtonBackColor.Click += new System.EventHandler(this.btnButtonBackColor_Click);
            // 
            // btnForeColor
            // 
            this.btnForeColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForeColor.Location = new System.Drawing.Point(15, 168);
            this.btnForeColor.Name = "btnForeColor";
            this.btnForeColor.Size = new System.Drawing.Size(111, 26);
            this.btnForeColor.TabIndex = 8;
            this.btnForeColor.Text = "Màu chữ";
            this.btnForeColor.UseVisualStyleBackColor = true;
            this.btnForeColor.Click += new System.EventHandler(this.btnForeColor_Click);
            // 
            // UpDownButtonSpace
            // 
            this.UpDownButtonSpace.Location = new System.Drawing.Point(15, 110);
            this.UpDownButtonSpace.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.UpDownButtonSpace.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownButtonSpace.Name = "UpDownButtonSpace";
            this.UpDownButtonSpace.Size = new System.Drawing.Size(111, 20);
            this.UpDownButtonSpace.TabIndex = 10;
            this.UpDownButtonSpace.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Khoảng cách";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.btnSampleButton);
            this.panel1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.panel1.Location = new System.Drawing.Point(148, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 214);
            this.panel1.TabIndex = 11;
            this.panel1.UseWaitCursor = true;
            // 
            // btnSampleButton
            // 
            this.btnSampleButton.Location = new System.Drawing.Point(95, 85);
            this.btnSampleButton.Name = "btnSampleButton";
            this.btnSampleButton.Size = new System.Drawing.Size(157, 42);
            this.btnSampleButton.TabIndex = 0;
            this.btnSampleButton.Text = "Nút dịch vụ";
            this.btnSampleButton.UseVisualStyleBackColor = true;
            this.btnSampleButton.UseWaitCursor = true;
            // 
            // frmButtonStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 315);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.UpDownButtonSpace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnForeColor);
            this.Controls.Add(this.btnButtonBackColor);
            this.Controls.Add(this.btnFontStyle);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.UpDownButtonHeight);
            this.Controls.Add(this.UpDownButtonWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmButtonStyle";
            this.Text = "Định dạng nút";
            this.Load += new System.EventHandler(this.frmButtonStyle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownButtonSpace)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown UpDownButtonWidth;
        private System.Windows.Forms.NumericUpDown UpDownButtonHeight;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnFontStyle;
        private System.Windows.Forms.Button btnButtonBackColor;
        private System.Windows.Forms.Button btnForeColor;
        private System.Windows.Forms.NumericUpDown UpDownButtonSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSampleButton;
    }
}