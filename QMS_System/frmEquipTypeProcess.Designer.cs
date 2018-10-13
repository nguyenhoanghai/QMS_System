namespace QMS_System
{
    partial class frmEquipTypeProcess
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
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridEquipTypeProcess = new DevExpress.XtraGrid.GridControl();
            this.gridViewEquipTypeProcess = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpEquipType = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpProcess = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_delete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetProcess = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetEquipType = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetEquipTypeProcess = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEquipTypeProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEquipTypeProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEquipType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.gridEquipTypeProcess);
            this.groupControl2.Location = new System.Drawing.Point(0, 46);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(893, 313);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Danh sách tiến trình loại thiết bị";
            // 
            // gridEquipTypeProcess
            // 
            this.gridEquipTypeProcess.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridEquipTypeProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEquipTypeProcess.Location = new System.Drawing.Point(2, 20);
            this.gridEquipTypeProcess.MainView = this.gridViewEquipTypeProcess;
            this.gridEquipTypeProcess.Name = "gridEquipTypeProcess";
            this.gridEquipTypeProcess.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookUpEquipType,
            this.lookUpProcess,
            this.repbtn_delete});
            this.gridEquipTypeProcess.Size = new System.Drawing.Size(889, 291);
            this.gridEquipTypeProcess.TabIndex = 1;
            this.gridEquipTypeProcess.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEquipTypeProcess});
            // 
            // gridViewEquipTypeProcess
            // 
            this.gridViewEquipTypeProcess.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn14});
            this.gridViewEquipTypeProcess.GridControl = this.gridEquipTypeProcess;
            this.gridViewEquipTypeProcess.Name = "gridViewEquipTypeProcess";
            this.gridViewEquipTypeProcess.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewEquipTypeProcess.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewEquipTypeProcess.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewEquipTypeProcess.OptionsView.RowAutoHeight = true;
            this.gridViewEquipTypeProcess.OptionsView.ShowAutoFilterRow = true;
            this.gridViewEquipTypeProcess.OptionsView.ShowGroupPanel = false;
            this.gridViewEquipTypeProcess.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewEquipTypeProcess_CellValueChanged);
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn2";
            this.gridColumn8.FieldName = "Id";
            this.gridColumn8.Name = "gridColumn8";
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
            this.gridColumn3.Caption = "Loại thiết bị";
            this.gridColumn3.ColumnEdit = this.lookUpEquipType;
            this.gridColumn3.FieldName = "EquipTypeId";
            this.gridColumn3.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 205;
            // 
            // lookUpEquipType
            // 
            this.lookUpEquipType.AutoHeight = false;
            this.lookUpEquipType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEquipType.Name = "lookUpEquipType";
            this.lookUpEquipType.View = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Tiến trình";
            this.gridColumn5.ColumnEdit = this.lookUpProcess;
            this.gridColumn5.FieldName = "ProcessId";
            this.gridColumn5.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            this.gridColumn5.Width = 209;
            // 
            // lookUpProcess
            // 
            this.lookUpProcess.AutoHeight = false;
            this.lookUpProcess.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpProcess.Name = "lookUpProcess";
            this.lookUpProcess.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
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
            this.gridColumn1.Caption = "Bước";
            this.gridColumn1.FieldName = "Step";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            this.gridColumn1.Width = 132;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Ưu tiên";
            this.gridColumn2.FieldName = "Priority";
            this.gridColumn2.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 134;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Đếm";
            this.gridColumn4.FieldName = "Count";
            this.gridColumn4.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            this.gridColumn4.Width = 165;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtn_delete;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            this.gridColumn14.Width = 26;
            // 
            // repbtn_delete
            // 
            this.repbtn_delete.AutoHeight = false;
            this.repbtn_delete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_delete.Name = "repbtn_delete";
            this.repbtn_delete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_delete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_delete_ButtonClick);
            // 
            // btnResetProcess
            // 
            this.btnResetProcess.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetProcess.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetProcess.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetProcess.Location = new System.Drawing.Point(224, 4);
            this.btnResetProcess.Name = "btnResetProcess";
            this.btnResetProcess.Size = new System.Drawing.Size(213, 38);
            this.btnResetProcess.TabIndex = 7;
            this.btnResetProcess.Text = "  Làm mới  danh sách tiến trình";
            this.btnResetProcess.Click += new System.EventHandler(this.btnResetProcess_Click);
            // 
            // btnResetEquipType
            // 
            this.btnResetEquipType.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetEquipType.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetEquipType.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetEquipType.Location = new System.Drawing.Point(5, 4);
            this.btnResetEquipType.Name = "btnResetEquipType";
            this.btnResetEquipType.Size = new System.Drawing.Size(213, 38);
            this.btnResetEquipType.TabIndex = 8;
            this.btnResetEquipType.Text = "  Làm mới  danh sách loại thiết bị";
            this.btnResetEquipType.Click += new System.EventHandler(this.btnResetEquipType_Click);
            // 
            // btnResetEquipTypeProcess
            // 
            this.btnResetEquipTypeProcess.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetEquipTypeProcess.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetEquipTypeProcess.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetEquipTypeProcess.Location = new System.Drawing.Point(443, 4);
            this.btnResetEquipTypeProcess.Name = "btnResetEquipTypeProcess";
            this.btnResetEquipTypeProcess.Size = new System.Drawing.Size(248, 38);
            this.btnResetEquipTypeProcess.TabIndex = 9;
            this.btnResetEquipTypeProcess.Text = "  Làm mới  danh sách tiến trình loại thiết bị";
            this.btnResetEquipTypeProcess.Click += new System.EventHandler(this.btnResetEquipTypeProcess_Click);
            // 
            // frmEquipTypeProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 359);
            this.Controls.Add(this.btnResetEquipTypeProcess);
            this.Controls.Add(this.btnResetEquipType);
            this.Controls.Add(this.btnResetProcess);
            this.Controls.Add(this.groupControl2);
            this.Name = "frmEquipTypeProcess";
            this.Text = "Tiến trình xử lý loại thiết bị";
            this.Load += new System.EventHandler(this.frmEquipTypeProcess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEquipTypeProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEquipTypeProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEquipType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_delete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridEquipTypeProcess;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewEquipTypeProcess;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpEquipType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpProcess;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_delete;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton btnResetProcess;
        private DevExpress.XtraEditors.SimpleButton btnResetEquipType;
        private DevExpress.XtraEditors.SimpleButton btnResetEquipTypeProcess;
    }
}