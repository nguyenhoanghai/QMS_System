namespace QMS_System
{
    partial class frmLanguage
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridLanguage = new DevExpress.XtraGrid.GridControl();
            this.gridViewLanguage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetMajor = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteLanguage)).BeginInit();
            this.SuspendLayout();
            // 
            // gridLanguage
            // 
            this.gridLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLanguage.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridLanguage.Location = new System.Drawing.Point(4, 49);
            this.gridLanguage.MainView = this.gridViewLanguage;
            this.gridLanguage.Name = "gridLanguage";
            this.gridLanguage.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteLanguage});
            this.gridLanguage.Size = new System.Drawing.Size(717, 211);
            this.gridLanguage.TabIndex = 1;
            this.gridLanguage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewLanguage});
            // 
            // gridViewLanguage
            // 
            this.gridViewLanguage.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn1,
            this.gridColumn6,
            this.gridColumn7});
            this.gridViewLanguage.GridControl = this.gridLanguage;
            this.gridViewLanguage.Name = "gridViewLanguage";
            this.gridViewLanguage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewLanguage.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewLanguage.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewLanguage.OptionsView.RowAutoHeight = true;
            this.gridViewLanguage.OptionsView.ShowAutoFilterRow = true;
            this.gridViewLanguage.OptionsView.ShowGroupPanel = false;
            this.gridViewLanguage.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewLanguage_CellValueChanged);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn2";
            this.gridColumn2.FieldName = "Id";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn1.Caption = "Tên ngôn ngữ";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 175;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn6.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn6.Caption = "Diễn giải";
            this.gridColumn6.FieldName = "Note";
            this.gridColumn6.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 149;
            // 
            // gridColumn7
            // 
            this.gridColumn7.ColumnEdit = this.repbtn_deleteLanguage;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 42;
            // 
            // repbtn_deleteLanguage
            // 
            this.repbtn_deleteLanguage.AutoHeight = false;
            this.repbtn_deleteLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.repbtn_deleteLanguage.Name = "repbtn_deleteLanguage";
            this.repbtn_deleteLanguage.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteLanguage.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deleteLanguage_ButtonClick);
            // 
            // btnResetMajor
            // 
            this.btnResetMajor.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetMajor.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetMajor.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetMajor.Location = new System.Drawing.Point(4, 5);
            this.btnResetMajor.Name = "btnResetMajor";
            this.btnResetMajor.Size = new System.Drawing.Size(188, 38);
            this.btnResetMajor.TabIndex = 9;
            this.btnResetMajor.Text = " Làm mới danh sách ngôn ngữ";
            this.btnResetMajor.Click += new System.EventHandler(this.btnResetMajor_Click);
            // 
            // frmLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 261);
            this.Controls.Add(this.btnResetMajor);
            this.Controls.Add(this.gridLanguage);
            this.Name = "frmLanguage";
            this.Text = "Ngôn ngữ";
            this.Load += new System.EventHandler(this.frmLanguage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteLanguage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridLanguage;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteLanguage;
        private DevExpress.XtraEditors.SimpleButton btnResetMajor;
    }
}