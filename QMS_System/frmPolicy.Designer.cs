namespace QMS_System
{
    partial class frmPolicy
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridPolicy = new DevExpress.XtraGrid.GridControl();
            this.gridViewPolicy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEdit_IsActived = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deletePolicy = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetMajor = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridPolicy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPolicy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit_IsActived)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deletePolicy)).BeginInit();
            this.SuspendLayout();
            // 
            // gridPolicy
            // 
            this.gridPolicy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPolicy.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridPolicy.Location = new System.Drawing.Point(2, 50);
            this.gridPolicy.MainView = this.gridViewPolicy;
            this.gridPolicy.Name = "gridPolicy";
            this.gridPolicy.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deletePolicy,
            this.chkEdit_IsActived});
            this.gridPolicy.Size = new System.Drawing.Size(765, 281);
            this.gridPolicy.TabIndex = 1;
            this.gridPolicy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPolicy});
            // 
            // gridViewPolicy
            // 
            this.gridViewPolicy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn7});
            this.gridViewPolicy.GridControl = this.gridPolicy;
            this.gridViewPolicy.Name = "gridViewPolicy";
            this.gridViewPolicy.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewPolicy.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewPolicy.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewPolicy.OptionsView.RowAutoHeight = true;
            this.gridViewPolicy.OptionsView.ShowAutoFilterRow = true;
            this.gridViewPolicy.OptionsView.ShowGroupPanel = false;
            this.gridViewPolicy.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewPolicy_CellValueChanged);
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
            this.gridColumn1.Caption = "Tên chính sách";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 255;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Hiện hành";
            this.gridColumn3.ColumnEdit = this.chkEdit_IsActived;
            this.gridColumn3.FieldName = "IsActived";
            this.gridColumn3.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 105;
            // 
            // chkEdit_IsActived
            // 
            this.chkEdit_IsActived.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.chkEdit_IsActived.Appearance.Options.UseForeColor = true;
            this.chkEdit_IsActived.AutoHeight = false;
            this.chkEdit_IsActived.Name = "chkEdit_IsActived";
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
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 207;
            // 
            // gridColumn7
            // 
            this.gridColumn7.ColumnEdit = this.repbtn_deletePolicy;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            this.gridColumn7.Width = 54;
            // 
            // repbtn_deletePolicy
            // 
            this.repbtn_deletePolicy.AutoHeight = false;
            this.repbtn_deletePolicy.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deletePolicy.Name = "repbtn_deletePolicy";
            this.repbtn_deletePolicy.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deletePolicy.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deletePolicy_ButtonClick);
            // 
            // btnResetMajor
            // 
            this.btnResetMajor.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetMajor.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetMajor.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetMajor.Location = new System.Drawing.Point(5, 6);
            this.btnResetMajor.Name = "btnResetMajor";
            this.btnResetMajor.Size = new System.Drawing.Size(193, 38);
            this.btnResetMajor.TabIndex = 8;
            this.btnResetMajor.Text = "  Làm mới  danh sách chính sách";
            this.btnResetMajor.Click += new System.EventHandler(this.btnResetMajor_Click);
            // 
            // frmPolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 333);
            this.Controls.Add(this.gridPolicy);
            this.Controls.Add(this.btnResetMajor);
            this.Name = "frmPolicy";
            this.Text = "Chính Sách";
            this.Load += new System.EventHandler(this.frmPolicy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridPolicy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPolicy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEdit_IsActived)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deletePolicy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridPolicy;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPolicy;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEdit_IsActived;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deletePolicy;
        private DevExpress.XtraEditors.SimpleButton btnResetMajor;
    }
}