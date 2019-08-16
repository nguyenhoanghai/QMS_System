namespace QMS_System
{
    partial class frmServiceLimit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServiceLimit));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnReGrid = new DevExpress.XtraEditors.SimpleButton();
            this.btnReUser = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridService = new DevExpress.XtraGrid.GridControl();
            this.gridViewService = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repLKService = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteService = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lkUser = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnReService = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLKService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkUser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReGrid
            // 
            this.btnReGrid.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnReGrid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnReGrid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnReGrid.Location = new System.Drawing.Point(13, 152);
            this.btnReGrid.Margin = new System.Windows.Forms.Padding(4);
            this.btnReGrid.Name = "btnReGrid";
            this.btnReGrid.Size = new System.Drawing.Size(249, 30);
            this.btnReGrid.TabIndex = 48;
            this.btnReGrid.Text = "  Làm mới lưới";
            // 
            // btnReUser
            // 
            this.btnReUser.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnReUser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnReUser.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnReUser.Location = new System.Drawing.Point(13, 76);
            this.btnReUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnReUser.Name = "btnReUser";
            this.btnReUser.Size = new System.Drawing.Size(249, 30);
            this.btnReUser.TabIndex = 47;
            this.btnReUser.Text = "  Làm mới danh sách nhân viên";
            this.btnReUser.Click += new System.EventHandler(this.btnReUser_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.gridService);
            this.groupControl1.Location = new System.Drawing.Point(270, 13);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(807, 346);
            this.groupControl1.TabIndex = 49;
            this.groupControl1.Text = "Danh mục giới hạn lấy phiếu theo dịch vụ của nhân viên";
            // 
            // gridService
            // 
            this.gridService.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridService.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridService.Location = new System.Drawing.Point(2, 25);
            this.gridService.MainView = this.gridViewService;
            this.gridService.Margin = new System.Windows.Forms.Padding(4);
            this.gridService.Name = "gridService";
            this.gridService.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteService,
            this.repLKService});
            this.gridService.Size = new System.Drawing.Size(803, 319);
            this.gridService.TabIndex = 1;
            this.gridService.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewService});
            // 
            // gridViewService
            // 
            this.gridViewService.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn7});
            this.gridViewService.GridControl = this.gridService;
            this.gridViewService.Name = "gridViewService";
            this.gridViewService.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewService.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewService.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewService.OptionsView.RowAutoHeight = true;
            this.gridViewService.OptionsView.ShowAutoFilterRow = true;
            this.gridViewService.OptionsView.ShowGroupPanel = false;
            this.gridViewService.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewService_CellValueChanged);
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
            this.gridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn1.Caption = "Dịch vụ";
            this.gridColumn1.ColumnEdit = this.repLKService;
            this.gridColumn1.FieldName = "ServiceId";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 176;
            // 
            // repLKService
            // 
            this.repLKService.AutoHeight = false;
            this.repLKService.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repLKService.Name = "repLKService";
            this.repLKService.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn3.Caption = "SL giới hạn";
            this.gridColumn3.FieldName = "Quantity";
            this.gridColumn3.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 110;
            // 
            // gridColumn7
            // 
            this.gridColumn7.ColumnEdit = this.repbtn_deleteService;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 20;
            // 
            // repbtn_deleteService
            // 
            this.repbtn_deleteService.AutoHeight = false;
            this.repbtn_deleteService.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtn_deleteService.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deleteService.Name = "repbtn_deleteService";
            this.repbtn_deleteService.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteService.Click += new System.EventHandler(this.repbtn_deleteService_Click);
            // 
            // lkUser
            // 
            this.lkUser.Location = new System.Drawing.Point(13, 37);
            this.lkUser.Margin = new System.Windows.Forms.Padding(4);
            this.lkUser.Name = "lkUser";
            this.lkUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lkUser.Properties.Appearance.Options.UseFont = true;
            this.lkUser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkUser.Properties.NullText = "Chọn nhân viên ...";
            this.lkUser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lkUser.Size = new System.Drawing.Size(249, 28);
            this.lkUser.TabIndex = 52;
            this.lkUser.EditValueChanged += new System.EventHandler(this.lkUser_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControl1.Location = new System.Drawing.Point(13, 13);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(81, 19);
            this.labelControl1.TabIndex = 50;
            this.labelControl1.Text = "Nhân viên";
            // 
            // btnReService
            // 
            this.btnReService.Image = global::QMS_System.Properties.Resources.if_view_refresh_15329;
            this.btnReService.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnReService.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnReService.Location = new System.Drawing.Point(13, 114);
            this.btnReService.Margin = new System.Windows.Forms.Padding(4);
            this.btnReService.Name = "btnReService";
            this.btnReService.Size = new System.Drawing.Size(249, 30);
            this.btnReService.TabIndex = 53;
            this.btnReService.Text = "  Làm mới danh sách dịch vụ";
            this.btnReService.Click += new System.EventHandler(this.btnReService_Click);
            // 
            // frmServiceLimit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 370);
            this.Controls.Add(this.btnReService);
            this.Controls.Add(this.lkUser);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnReGrid);
            this.Controls.Add(this.btnReUser);
            this.Name = "frmServiceLimit";
            this.Text = "Giới hạn lấy phiếu theo dịch vụ";
            this.Load += new System.EventHandler(this.frmServiceLimit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLKService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkUser.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnReGrid;
        private DevExpress.XtraEditors.SimpleButton btnReUser;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gridService;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewService;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteService;
        private DevExpress.XtraEditors.LookUpEdit lkUser;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repLKService;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton btnReService;
    }
}