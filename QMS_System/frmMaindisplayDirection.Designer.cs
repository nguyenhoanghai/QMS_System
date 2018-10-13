namespace QMS_System
{
    partial class frmMaindisplayDirection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaindisplayDirection));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridMaindisplayDirection = new DevExpress.XtraGrid.GridControl();
            this.gridViewMaindisplayDirection = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridLookUpEquipment = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toggleSwitchDirection = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetCounter = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpCounter = new DevExpress.XtraEditors.LookUpEdit();
            this.btnResetGrid = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridMaindisplayDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMaindisplayDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEquipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpCounter.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridMaindisplayDirection
            // 
            this.gridMaindisplayDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMaindisplayDirection.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridMaindisplayDirection.Location = new System.Drawing.Point(5, 47);
            this.gridMaindisplayDirection.MainView = this.gridViewMaindisplayDirection;
            this.gridMaindisplayDirection.Name = "gridMaindisplayDirection";
            this.gridMaindisplayDirection.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtnDelete,
            this.toggleSwitchDirection,
            this.gridLookUpEquipment});
            this.gridMaindisplayDirection.Size = new System.Drawing.Size(950, 340);
            this.gridMaindisplayDirection.TabIndex = 49;
            this.gridMaindisplayDirection.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMaindisplayDirection});
            // 
            // gridViewMaindisplayDirection
            // 
            this.gridViewMaindisplayDirection.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn1,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn3,
            this.gridColumn14});
            this.gridViewMaindisplayDirection.GridControl = this.gridMaindisplayDirection;
            this.gridViewMaindisplayDirection.Name = "gridViewMaindisplayDirection";
            this.gridViewMaindisplayDirection.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMaindisplayDirection.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMaindisplayDirection.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewMaindisplayDirection.OptionsView.RowAutoHeight = true;
            this.gridViewMaindisplayDirection.OptionsView.ShowAutoFilterRow = true;
            this.gridViewMaindisplayDirection.OptionsView.ShowGroupPanel = false;
            this.gridViewMaindisplayDirection.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewMaindisplayDirection_CellValueChanged);
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn2";
            this.gridColumn8.FieldName = "Id";
            this.gridColumn8.Name = "gridColumn8";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "STT";
            this.gridColumn1.FieldName = "Index";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 69;
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
            this.gridColumn9.Caption = "Thiết bị";
            this.gridColumn9.ColumnEdit = this.gridLookUpEquipment;
            this.gridColumn9.FieldName = "EquipmentId";
            this.gridColumn9.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 253;
            // 
            // gridLookUpEquipment
            // 
            this.gridLookUpEquipment.AutoHeight = false;
            this.gridLookUpEquipment.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEquipment.Name = "gridLookUpEquipment";
            this.gridLookUpEquipment.View = this.repositoryItemGridLookUpEdit1View;
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
            this.gridColumn10.Caption = "Hướng đi";
            this.gridColumn10.ColumnEdit = this.toggleSwitchDirection;
            this.gridColumn10.FieldName = "Direction";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 150;
            // 
            // toggleSwitchDirection
            // 
            this.toggleSwitchDirection.AutoHeight = false;
            this.toggleSwitchDirection.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.toggleSwitchDirection.Name = "toggleSwitchDirection";
            this.toggleSwitchDirection.OffText = "Trái";
            this.toggleSwitchDirection.OnText = "Phải";
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
            this.gridColumn3.Caption = "Diễn giải";
            this.gridColumn3.FieldName = "Note";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 320;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtnDelete;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 4;
            this.gridColumn14.Width = 140;
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
            // btnResetCounter
            // 
            this.btnResetCounter.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnResetCounter.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnResetCounter.Appearance.Options.UseBackColor = true;
            this.btnResetCounter.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetCounter.Location = new System.Drawing.Point(285, 12);
            this.btnResetCounter.Name = "btnResetCounter";
            this.btnResetCounter.Size = new System.Drawing.Size(27, 24);
            this.btnResetCounter.TabIndex = 48;
            this.btnResetCounter.Click += new System.EventHandler(this.btnResetCounter_Click);
            // 
            // lookUpCounter
            // 
            this.lookUpCounter.Location = new System.Drawing.Point(5, 12);
            this.lookUpCounter.Name = "lookUpCounter";
            this.lookUpCounter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpCounter.Properties.Appearance.Options.UseFont = true;
            this.lookUpCounter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpCounter.Properties.NullText = "Chọn Quầy ...";
            this.lookUpCounter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpCounter.Size = new System.Drawing.Size(272, 22);
            this.lookUpCounter.TabIndex = 47;
            this.lookUpCounter.EditValueChanged += new System.EventHandler(this.lookUpCounter_EditValueChanged);
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnResetGrid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetGrid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetGrid.Location = new System.Drawing.Point(319, 12);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(280, 24);
            this.btnResetGrid.TabIndex = 46;
            this.btnResetGrid.Text = "  Làm mới danh sách hướng đi  Main Display";
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // frmMaindisplayDirection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 393);
            this.Controls.Add(this.gridMaindisplayDirection);
            this.Controls.Add(this.btnResetCounter);
            this.Controls.Add(this.lookUpCounter);
            this.Controls.Add(this.btnResetGrid);
            this.Name = "frmMaindisplayDirection";
            this.Text = "Hướng đi Main Display";
            this.Load += new System.EventHandler(this.frmMaindisplayDirection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMaindisplayDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMaindisplayDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEquipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpCounter.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridMaindisplayDirection;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMaindisplayDirection;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch toggleSwitchDirection;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDelete;
        private DevExpress.XtraEditors.SimpleButton btnResetCounter;
        private DevExpress.XtraEditors.LookUpEdit lookUpCounter;
        private DevExpress.XtraEditors.SimpleButton btnResetGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridLookUpEquipment;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;


    }
}