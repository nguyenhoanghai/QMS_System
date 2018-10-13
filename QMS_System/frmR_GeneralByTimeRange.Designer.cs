namespace QMS_System
{
    partial class frmR_GeneralByTimeRange
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
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.dtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.dtToDate = new DevExpress.XtraEditors.DateEdit();
            this.lblToDate = new DevExpress.XtraEditors.LabelControl();
            this.lblFromDate = new DevExpress.XtraEditors.LabelControl();
            this.lookUpSelect = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.btnExportToExcel = new DevExpress.XtraEditors.SimpleButton();
            this.gridGeneralByTimeRange = new DevExpress.XtraGrid.GridControl();
            this.gridViewGeneralByTimeRange = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MajorName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteCounter = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lookUpSound = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repCheckedComboBoxSound = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSelect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGeneralByTimeRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewGeneralByTimeRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckedComboBoxSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.dtFromDate);
            this.groupControl2.Controls.Add(this.dtToDate);
            this.groupControl2.Controls.Add(this.lblToDate);
            this.groupControl2.Controls.Add(this.lblFromDate);
            this.groupControl2.Controls.Add(this.lookUpSelect);
            this.groupControl2.Controls.Add(this.btnSearch);
            this.groupControl2.Controls.Add(this.radioGroup1);
            this.groupControl2.Controls.Add(this.btnExportToExcel);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1086, 70);
            this.groupControl2.TabIndex = 15;
            this.groupControl2.Text = "Tìm kiếm theo";
            // 
            // dtFromDate
            // 
            this.dtFromDate.EditValue = new System.DateTime(2018, 3, 14, 11, 24, 20, 0);
            this.dtFromDate.Location = new System.Drawing.Point(476, 34);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFromDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm tt";
            this.dtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm tt";
            this.dtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm tt";
            this.dtFromDate.Size = new System.Drawing.Size(136, 20);
            this.dtFromDate.TabIndex = 26;
            // 
            // dtToDate
            // 
            this.dtToDate.EditValue = new System.DateTime(2018, 3, 14, 11, 24, 20, 0);
            this.dtToDate.Location = new System.Drawing.Point(670, 34);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtToDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm tt";
            this.dtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtToDate.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm tt";
            this.dtToDate.Size = new System.Drawing.Size(129, 20);
            this.dtToDate.TabIndex = 18;
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(617, 37);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(47, 13);
            this.lblToDate.TabIndex = 25;
            this.lblToDate.Text = "Đến ngày";
            // 
            // lblFromDate
            // 
            this.lblFromDate.Location = new System.Drawing.Point(430, 37);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(40, 13);
            this.lblFromDate.TabIndex = 23;
            this.lblFromDate.Text = "Từ ngày";
            // 
            // lookUpSelect
            // 
            this.lookUpSelect.EditValue = "";
            this.lookUpSelect.Location = new System.Drawing.Point(238, 33);
            this.lookUpSelect.Name = "lookUpSelect";
            this.lookUpSelect.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.lookUpSelect.Properties.Appearance.Options.UseFont = true;
            this.lookUpSelect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpSelect.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lookUpSelect.Properties.View = this.gridLookUpEdit1View;
            this.lookUpSelect.Size = new System.Drawing.Size(187, 22);
            this.lookUpSelect.TabIndex = 11;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Appearance.Options.UseTextOptions = true;
            this.btnSearch.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnSearch.Image = global::QMS_System.Properties.Resources.search_icon_20;
            this.btnSearch.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSearch.Location = new System.Drawing.Point(823, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 28);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = " Tìm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radioGroup1
            // 
            this.radioGroup1.EditValue = ((short)(1));
            this.radioGroup1.Location = new System.Drawing.Point(5, 30);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Columns = 3;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "Nhân viên"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(2)), "Nghiệp vụ"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(3)), "Dịch vụ")});
            this.radioGroup1.Size = new System.Drawing.Size(227, 27);
            this.radioGroup1.TabIndex = 17;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.btnExportToExcel.Appearance.Options.UseFont = true;
            this.btnExportToExcel.Appearance.Options.UseTextOptions = true;
            this.btnExportToExcel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnExportToExcel.Image = global::QMS_System.Properties.Resources.excel2_icon_20;
            this.btnExportToExcel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnExportToExcel.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnExportToExcel.Location = new System.Drawing.Point(916, 29);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(139, 28);
            this.btnExportToExcel.TabIndex = 13;
            this.btnExportToExcel.Text = " Xuất File Excel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // gridGeneralByTimeRange
            // 
            this.gridGeneralByTimeRange.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridGeneralByTimeRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGeneralByTimeRange.Location = new System.Drawing.Point(0, 70);
            this.gridGeneralByTimeRange.MainView = this.gridViewGeneralByTimeRange;
            this.gridGeneralByTimeRange.Name = "gridGeneralByTimeRange";
            this.gridGeneralByTimeRange.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteCounter,
            this.lookUpSound,
            this.repCheckedComboBoxSound});
            this.gridGeneralByTimeRange.Size = new System.Drawing.Size(1086, 434);
            this.gridGeneralByTimeRange.TabIndex = 16;
            this.gridGeneralByTimeRange.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewGeneralByTimeRange,
            this.gridView1});
            // 
            // gridViewGeneralByTimeRange
            // 
            this.gridViewGeneralByTimeRange.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn1,
            this.UserName,
            this.MajorName,
            this.ServiceName,
            this.gridColumn9});
            this.gridViewGeneralByTimeRange.GridControl = this.gridGeneralByTimeRange;
            this.gridViewGeneralByTimeRange.Name = "gridViewGeneralByTimeRange";
            this.gridViewGeneralByTimeRange.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewGeneralByTimeRange.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewGeneralByTimeRange.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewGeneralByTimeRange.OptionsView.RowAutoHeight = true;
            this.gridViewGeneralByTimeRange.OptionsView.ShowAutoFilterRow = true;
            this.gridViewGeneralByTimeRange.OptionsView.ShowGroupPanel = false;
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
            this.gridColumn1.Caption = "STT";
            this.gridColumn1.FieldName = "Index";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 49;
            // 
            // UserName
            // 
            this.UserName.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.UserName.AppearanceCell.Options.UseFont = true;
            this.UserName.AppearanceCell.Options.UseTextOptions = true;
            this.UserName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UserName.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.UserName.AppearanceHeader.Options.UseFont = true;
            this.UserName.AppearanceHeader.Options.UseTextOptions = true;
            this.UserName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UserName.Caption = "Đối tượng";
            this.UserName.FieldName = "Name";
            this.UserName.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.UserName.Name = "UserName";
            this.UserName.OptionsColumn.AllowEdit = false;
            this.UserName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.UserName.Visible = true;
            this.UserName.VisibleIndex = 1;
            this.UserName.Width = 171;
            // 
            // MajorName
            // 
            this.MajorName.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.MajorName.AppearanceCell.Options.UseFont = true;
            this.MajorName.AppearanceCell.Options.UseTextOptions = true;
            this.MajorName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MajorName.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.MajorName.AppearanceHeader.Options.UseFont = true;
            this.MajorName.AppearanceHeader.Options.UseTextOptions = true;
            this.MajorName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MajorName.Caption = "Số lượt giao dịch";
            this.MajorName.FieldName = "TotalTransaction";
            this.MajorName.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.MajorName.Name = "MajorName";
            this.MajorName.OptionsColumn.AllowEdit = false;
            this.MajorName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.MajorName.Visible = true;
            this.MajorName.VisibleIndex = 2;
            this.MajorName.Width = 141;
            // 
            // ServiceName
            // 
            this.ServiceName.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.ServiceName.AppearanceCell.Options.UseFont = true;
            this.ServiceName.AppearanceCell.Options.UseTextOptions = true;
            this.ServiceName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ServiceName.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.ServiceName.AppearanceHeader.Options.UseFont = true;
            this.ServiceName.AppearanceHeader.Options.UseTextOptions = true;
            this.ServiceName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ServiceName.Caption = "Tổng thời gian giao dịch (phút)";
            this.ServiceName.FieldName = "TotalTransTime";
            this.ServiceName.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.OptionsColumn.AllowEdit = false;
            this.ServiceName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.ServiceName.Visible = true;
            this.ServiceName.VisibleIndex = 3;
            this.ServiceName.Width = 206;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn9.AppearanceHeader.Options.UseFont = true;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "Thời gian giao dịch trung bình (phút/gd)";
            this.gridColumn9.FieldName = "AverageTimePerTrans";
            this.gridColumn9.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 4;
            this.gridColumn9.Width = 244;
            // 
            // repbtn_deleteCounter
            // 
            this.repbtn_deleteCounter.AutoHeight = false;
            this.repbtn_deleteCounter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deleteCounter.Name = "repbtn_deleteCounter";
            this.repbtn_deleteCounter.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // lookUpSound
            // 
            this.lookUpSound.AutoHeight = false;
            this.lookUpSound.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpSound.Name = "lookUpSound";
            this.lookUpSound.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // repCheckedComboBoxSound
            // 
            this.repCheckedComboBoxSound.AutoHeight = false;
            this.repCheckedComboBoxSound.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCheckedComboBoxSound.Name = "repCheckedComboBoxSound";
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridGeneralByTimeRange;
            this.gridView1.Name = "gridView1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmR_GeneralByTimeRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 504);
            this.Controls.Add(this.gridGeneralByTimeRange);
            this.Controls.Add(this.groupControl2);
            this.Name = "frmR_GeneralByTimeRange";
            this.Text = "Báo cáo tổng hợp theo khoảng thời gian";
            this.Load += new System.EventHandler(this.frmR_GeneralByTimeRange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSelect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGeneralByTimeRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewGeneralByTimeRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckedComboBoxSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GridLookUpEdit lookUpSelect;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.SimpleButton btnExportToExcel;
        private DevExpress.XtraEditors.LabelControl lblToDate;
        private DevExpress.XtraEditors.LabelControl lblFromDate;
        private DevExpress.XtraEditors.DateEdit dtToDate;
        private DevExpress.XtraEditors.DateEdit dtFromDate;
        private DevExpress.XtraGrid.GridControl gridGeneralByTimeRange;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewGeneralByTimeRange;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn UserName;
        private DevExpress.XtraGrid.Columns.GridColumn MajorName;
        private DevExpress.XtraGrid.Columns.GridColumn ServiceName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteCounter;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpSound;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repCheckedComboBoxSound;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Timer timer1;
    }
}