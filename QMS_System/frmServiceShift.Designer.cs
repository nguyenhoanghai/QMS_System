namespace QMS_System
{
    partial class frmServiceShift
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServiceShift));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridServiceShift = new DevExpress.XtraGrid.GridControl();
            this.gridViewServiceShift = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetService = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpService = new DevExpress.XtraEditors.LookUpEdit();
            this.btnResetGrid = new DevExpress.XtraEditors.SimpleButton();
            this.gridLookUpShift = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridServiceShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewServiceShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpService.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // gridServiceShift
            // 
            this.gridServiceShift.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridServiceShift.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridServiceShift.Location = new System.Drawing.Point(4, 36);
            this.gridServiceShift.MainView = this.gridViewServiceShift;
            this.gridServiceShift.Name = "gridServiceShift";
            this.gridServiceShift.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtnDelete,
            this.gridLookUpShift});
            this.gridServiceShift.Size = new System.Drawing.Size(663, 352);
            this.gridServiceShift.TabIndex = 45;
            this.gridServiceShift.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewServiceShift});
            // 
            // gridViewServiceShift
            // 
            this.gridViewServiceShift.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn14});
            this.gridViewServiceShift.GridControl = this.gridServiceShift;
            this.gridViewServiceShift.Name = "gridViewServiceShift";
            this.gridViewServiceShift.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewServiceShift.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewServiceShift.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewServiceShift.OptionsView.RowAutoHeight = true;
            this.gridViewServiceShift.OptionsView.ShowAutoFilterRow = true;
            this.gridViewServiceShift.OptionsView.ShowGroupPanel = false;
            this.gridViewServiceShift.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewServiceShift_CellValueChanged);
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
            this.gridColumn9.Caption = "Thời gian cấp phiếu (Ca)";
            this.gridColumn9.ColumnEdit = this.gridLookUpShift;
            this.gridColumn9.FieldName = "ShiftId";
            this.gridColumn9.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 509;
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
            this.gridColumn10.Width = 126;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtnDelete;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 2;
            this.gridColumn14.Width = 71;
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
            // btnResetService
            // 
            this.btnResetService.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnResetService.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnResetService.Appearance.Options.UseBackColor = true;
            this.btnResetService.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetService.Location = new System.Drawing.Point(282, 6);
            this.btnResetService.Name = "btnResetService";
            this.btnResetService.Size = new System.Drawing.Size(27, 24);
            this.btnResetService.TabIndex = 44;
            this.btnResetService.Click += new System.EventHandler(this.btnResetService_Click);
            // 
            // lookUpService
            // 
            this.lookUpService.Location = new System.Drawing.Point(4, 6);
            this.lookUpService.Name = "lookUpService";
            this.lookUpService.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpService.Properties.Appearance.Options.UseFont = true;
            this.lookUpService.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpService.Properties.NullText = "Chọn dịch vụ ...";
            this.lookUpService.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpService.Size = new System.Drawing.Size(272, 22);
            this.lookUpService.TabIndex = 43;
            this.lookUpService.EditValueChanged += new System.EventHandler(this.lookUpService_EditValueChanged);
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetGrid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetGrid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetGrid.Location = new System.Drawing.Point(315, 6);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(280, 24);
            this.btnResetGrid.TabIndex = 42;
            this.btnResetGrid.Text = "  Làm mới danh sách thời gian cấp phiếu dịch vụ";
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // gridLookUpShift
            // 
            this.gridLookUpShift.AutoHeight = false;
            this.gridLookUpShift.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpShift.Name = "gridLookUpShift";
            this.gridLookUpShift.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // frmServiceShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 393);
            this.Controls.Add(this.gridServiceShift);
            this.Controls.Add(this.btnResetService);
            this.Controls.Add(this.lookUpService);
            this.Controls.Add(this.btnResetGrid);
            this.Name = "frmServiceShift";
            this.Text = "Thời Gian Cấp Phiếu Dịch Vụ";
            this.Load += new System.EventHandler(this.frmServiceShift_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridServiceShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewServiceShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpService.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridServiceShift;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewServiceShift;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDelete;
        private DevExpress.XtraEditors.SimpleButton btnResetService;
        private DevExpress.XtraEditors.LookUpEdit lookUpService;
        private DevExpress.XtraEditors.SimpleButton btnResetGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridLookUpShift;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;

    }
}