namespace QMS_System.IssueTicketScreen
{
    partial class frmSetupInterface
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnButtonStyle = new System.Windows.Forms.Button();
            this.btnNumOfColumn = new System.Windows.Forms.Button();
            this.btnSetBackGroundImg = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(504, 319);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPage1.Controls.Add(this.btnButtonStyle);
            this.tabPage1.Controls.Add(this.btnNumOfColumn);
            this.tabPage1.Controls.Add(this.btnSetBackGroundImg);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(496, 290);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Giao diện";
            // 
            // btnButtonStyle
            // 
            this.btnButtonStyle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnButtonStyle.Location = new System.Drawing.Point(159, 191);
            this.btnButtonStyle.Name = "btnButtonStyle";
            this.btnButtonStyle.Size = new System.Drawing.Size(152, 41);
            this.btnButtonStyle.TabIndex = 2;
            this.btnButtonStyle.Text = "Định dạng nút";
            this.btnButtonStyle.UseVisualStyleBackColor = true;
            this.btnButtonStyle.Click += new System.EventHandler(this.btnButtonStyle_Click);
            // 
            // btnNumOfColumn
            // 
            this.btnNumOfColumn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNumOfColumn.Location = new System.Drawing.Point(159, 122);
            this.btnNumOfColumn.Name = "btnNumOfColumn";
            this.btnNumOfColumn.Size = new System.Drawing.Size(152, 41);
            this.btnNumOfColumn.TabIndex = 1;
            this.btnNumOfColumn.Text = "Số cột hiển thị";
            this.btnNumOfColumn.UseVisualStyleBackColor = true;
            this.btnNumOfColumn.Click += new System.EventHandler(this.btnNumOfColumn_Click);
            // 
            // btnSetBackGroundImg
            // 
            this.btnSetBackGroundImg.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetBackGroundImg.Location = new System.Drawing.Point(159, 54);
            this.btnSetBackGroundImg.Name = "btnSetBackGroundImg";
            this.btnSetBackGroundImg.Size = new System.Drawing.Size(152, 41);
            this.btnSetBackGroundImg.TabIndex = 0;
            this.btnSetBackGroundImg.Text = "Chỉnh ảnh nền";
            this.btnSetBackGroundImg.UseVisualStyleBackColor = true;
            this.btnSetBackGroundImg.Click += new System.EventHandler(this.btnSetBackGroundImg_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(496, 290);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nút dịch vụ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Danh sách nút hiện có :";
            // 
            // frmSetupInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 319);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmSetupInterface";
            this.Text = "Cài đặt giao diện";
            this.Load += new System.EventHandler(this.frmSetupInterface_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnButtonStyle;
        private System.Windows.Forms.Button btnNumOfColumn;
        private System.Windows.Forms.Button btnSetBackGroundImg;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
    }
}