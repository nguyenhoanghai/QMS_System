namespace QMS_System
{
    partial class frmUserMajor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserMajor));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnResetGrid = new DevExpress.XtraEditors.SimpleButton();
            this.lkUser = new DevExpress.XtraEditors.LookUpEdit();
            this.btnResetUser = new DevExpress.XtraEditors.SimpleButton();
            this.gridUserMajor = new DevExpress.XtraGrid.GridControl();
            this.gridViewUserMajor = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridLookUpMajor = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lkUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).BeginInit();
            this.SuspendLayout();
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetGrid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetGrid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetGrid.Location = new System.Drawing.Point(315, 7);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(229, 24);
            this.btnResetGrid.TabIndex = 10;
            this.btnResetGrid.Text = "  Làm mới danh sách nhân viên nghiệp vụ";
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // lkUser
            // 
            this.lkUser.Location = new System.Drawing.Point(4, 7);
            this.lkUser.Name = "lkUser";
            this.lkUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.lkUser.Properties.Appearance.Options.UseFont = true;
            this.lkUser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkUser.Properties.NullText = "Chọn nhân viên ...";
            this.lkUser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lkUser.Size = new System.Drawing.Size(272, 24);
            this.lkUser.TabIndex = 39;
            this.lkUser.EditValueChanged += new System.EventHandler(this.lkUser_EditValueChanged);
            // 
            // btnResetUser
            // 
            this.btnResetUser.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnResetUser.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnResetUser.Appearance.Options.UseBackColor = true;
            this.btnResetUser.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetUser.Location = new System.Drawing.Point(282, 7);
            this.btnResetUser.Name = "btnResetUser";
            this.btnResetUser.Size = new System.Drawing.Size(27, 24);
            this.btnResetUser.TabIndex = 40;
            this.btnResetUser.Click += new System.EventHandler(this.btnResetUser_Click);
            // 
            // gridUserMajor
            // 
            this.gridUserMajor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridUserMajor.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridUserMajor.Location = new System.Drawing.Point(4, 37);
            this.gridUserMajor.MainView = this.gridViewUserMajor;
            this.gridUserMajor.Name = "gridUserMajor";
            this.gridUserMajor.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtnDelete,
            this.gridLookUpMajor});
            this.gridUserMajor.Size = new System.Drawing.Size(602, 295);
            this.gridUserMajor.TabIndex = 41;
            this.gridUserMajor.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewUserMajor});
            // 
            // gridViewUserMajor
            // 
            this.gridViewUserMajor.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn14});
            this.gridViewUserMajor.GridControl = this.gridUserMajor;
            this.gridViewUserMajor.Name = "gridViewUserMajor";
            this.gridViewUserMajor.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewUserMajor.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewUserMajor.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewUserMajor.OptionsView.RowAutoHeight = true;
            this.gridViewUserMajor.OptionsView.ShowAutoFilterRow = true;
            this.gridViewUserMajor.OptionsView.ShowGroupPanel = false;
            this.gridViewUserMajor.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewUserMajor_CellValueChanged);
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn2";
            this.gridColumn8.FieldName = "Id";
            this.gridColumn8.Name = "gridColumn8";
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn9.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn9.AppearanceHeader.Options.UseFont = true;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn9.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn9.Caption = "Nghiệp vụ";
            this.gridColumn9.ColumnEdit = this.gridLookUpMajor;
            this.gridColumn9.FieldName = "MajorId";
            this.gridColumn9.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 291;
            // 
            // gridLookUpMajor
            // 
            this.gridLookUpMajor.AutoHeight = false;
            this.gridLookUpMajor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpMajor.Name = "gridLookUpMajor";
            this.gridLookUpMajor.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn10.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn10.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn10.Caption = "STT";
            this.gridColumn10.FieldName = "Index";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 125;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtnDelete;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 2;
            this.gridColumn14.Width = 39;
            // 
            // repbtnDelete
            // 
            this.repbtnDelete.AutoHeight = false;
            this.repbtnDelete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtnDelete.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtnDelete.Name = "repbtnDelete";
            this.repbtnDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtnDelete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtnDelete_ButtonClick);
            // 
            // frmUserMajor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 336);
            this.Controls.Add(this.gridUserMajor);
            this.Controls.Add(this.btnResetUser);
            this.Controls.Add(this.lkUser);
            this.Controls.Add(this.btnResetGrid);
            this.Name = "frmUserMajor";
            this.Text = "Đăng ký nghiệp vụ nhân viên";
            this.Load += new System.EventHandler(this.frmUserMajor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lkUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserMajor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewUserMajor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpMajor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnResetGrid;
        private DevExpress.XtraEditors.LookUpEdit lkUser;
        private DevExpress.XtraEditors.SimpleButton btnResetUser;
        private DevExpress.XtraGrid.GridControl gridUserMajor;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewUserMajor;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridLookUpMajor;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDelete;
    }
}