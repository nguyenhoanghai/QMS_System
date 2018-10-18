namespace QMS_System
{
    partial class frmVideoTemplate
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVideoTemplate));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridVideo = new DevExpress.XtraGrid.GridControl();
            this.gridViewVideo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEdit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ColDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteCounter = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridChild = new DevExpress.XtraGrid.GridControl();
            this.gridViewChild = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repLKVideo = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDeleteChild = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetTemplate = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetDetailGrid = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLKVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDeleteChild)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnResetTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.groupControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnResetDetailGrid);
            this.splitContainer1.Panel2.Controls.Add(this.groupControl2);
            this.splitContainer1.Size = new System.Drawing.Size(938, 450);
            this.splitContainer1.SplitterDistance = 533;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.gridVideo);
            this.groupControl1.Location = new System.Drawing.Point(3, 53);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(527, 394);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "Danh sách mẫu phát Video";
            // 
            // gridVideo
            // 
            this.gridVideo.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridVideo.Location = new System.Drawing.Point(2, 20);
            this.gridVideo.MainView = this.gridViewVideo;
            this.gridVideo.Name = "gridVideo";
            this.gridVideo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteCounter,
            this.repbtnDetail,
            this.repositoryItemCheckEdit1});
            this.gridVideo.Size = new System.Drawing.Size(523, 372);
            this.gridVideo.TabIndex = 2;
            this.gridVideo.UseEmbeddedNavigator = true;
            this.gridVideo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewVideo});
            // 
            // gridViewVideo
            // 
            this.gridViewVideo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn6,
            this.colEdit,
            this.ColDelete});
            this.gridViewVideo.GridControl = this.gridVideo;
            this.gridViewVideo.Name = "gridViewVideo";
            this.gridViewVideo.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewVideo.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewVideo.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewVideo.OptionsView.RowAutoHeight = true;
            this.gridViewVideo.OptionsView.ShowAutoFilterRow = true;
            this.gridViewVideo.OptionsView.ShowGroupPanel = false;
            this.gridViewVideo.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewVideo_CustomRowCellEdit);
            this.gridViewVideo.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewVideo_CellValueChanged);
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
            this.gridColumn1.Caption = "Tên mẫu";
            this.gridColumn1.FieldName = "TemplateName";
            this.gridColumn1.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 220;
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
            this.gridColumn5.Caption = "Trạng thái";
            this.gridColumn5.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn5.FieldName = "IsActive";
            this.gridColumn5.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            this.gridColumn5.Width = 64;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
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
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 172;
            // 
            // colEdit
            // 
            this.colEdit.ColumnEdit = this.repbtnDetail;
            this.colEdit.Name = "colEdit";
            this.colEdit.Visible = true;
            this.colEdit.VisibleIndex = 3;
            this.colEdit.Width = 27;
            // 
            // repbtnDetail
            // 
            this.repbtnDetail.AutoHeight = false;
            this.repbtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.list, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "Cấu hình âm thanh quầy", null, null, true)});
            this.repbtnDetail.Name = "repbtnDetail";
            this.repbtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtnDetail.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtnDetail_ButtonClick);
            // 
            // ColDelete
            // 
            this.ColDelete.ColumnEdit = this.repbtn_deleteCounter;
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.Visible = true;
            this.ColDelete.VisibleIndex = 4;
            this.ColDelete.Width = 22;
            // 
            // repbtn_deleteCounter
            // 
            this.repbtn_deleteCounter.AutoHeight = false;
            this.repbtn_deleteCounter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, true)});
            this.repbtn_deleteCounter.Name = "repbtn_deleteCounter";
            this.repbtn_deleteCounter.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteCounter.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deleteCounter_ButtonClick);
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.gridChild);
            this.groupControl2.Location = new System.Drawing.Point(3, 53);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(395, 394);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "Danh sách video theo mẫu";
            // 
            // gridChild
            // 
            this.gridChild.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridChild.Location = new System.Drawing.Point(2, 20);
            this.gridChild.MainView = this.gridViewChild;
            this.gridChild.Name = "gridChild";
            this.gridChild.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtnDeleteChild,
            this.repLKVideo,
            this.repositoryItemSpinEdit1});
            this.gridChild.Size = new System.Drawing.Size(391, 372);
            this.gridChild.TabIndex = 1;
            this.gridChild.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewChild});
            // 
            // gridViewChild
            // 
            this.gridViewChild.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn14});
            this.gridViewChild.GridControl = this.gridChild;
            this.gridViewChild.Name = "gridViewChild";
            this.gridViewChild.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewChild.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewChild.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewChild.OptionsView.RowAutoHeight = true;
            this.gridViewChild.OptionsView.ShowAutoFilterRow = true;
            this.gridViewChild.OptionsView.ShowGroupPanel = false;
            this.gridViewChild.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewChild_CustomRowCellEdit);
            this.gridViewChild.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewChild_CellValueChanged);
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "gridColumn2";
            this.gridColumn9.FieldName = "Id";
            this.gridColumn9.Name = "gridColumn9";
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
            this.gridColumn10.Caption = "Video";
            this.gridColumn10.ColumnEdit = this.repLKVideo;
            this.gridColumn10.FieldName = "VideoId";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 291;
            // 
            // repLKVideo
            // 
            this.repLKVideo.AutoHeight = false;
            this.repLKVideo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repLKVideo.Name = "repLKVideo";
            this.repLKVideo.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn11.AppearanceCell.Options.UseFont = true;
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn11.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn11.AppearanceHeader.Options.UseFont = true;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn11.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn11.Caption = "STT";
            this.gridColumn11.ColumnEdit = this.repositoryItemSpinEdit1;
            this.gridColumn11.FieldName = "Index";
            this.gridColumn11.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 0;
            this.gridColumn11.Width = 48;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.IsFloatValue = false;
            this.repositoryItemSpinEdit1.Mask.EditMask = "N00";
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtnDeleteChild;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 2;
            this.gridColumn14.Width = 34;
            // 
            // repbtnDeleteChild
            // 
            this.repbtnDeleteChild.AutoHeight = false;
            this.repbtnDeleteChild.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtnDeleteChild.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, null, true)});
            this.repbtnDeleteChild.Name = "repbtnDeleteChild";
            this.repbtnDeleteChild.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtnDeleteChild.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtnDeleteChild_ButtonClick);
            // 
            // btnResetTemplate
            // 
            this.btnResetTemplate.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetTemplate.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetTemplate.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetTemplate.Location = new System.Drawing.Point(9, 9);
            this.btnResetTemplate.Name = "btnResetTemplate";
            this.btnResetTemplate.Size = new System.Drawing.Size(132, 38);
            this.btnResetTemplate.TabIndex = 6;
            this.btnResetTemplate.Text = "  Làm mới  lưới mẫu";
            this.btnResetTemplate.Click += new System.EventHandler(this.btnResetTemplate_Click);
            // 
            // btnResetDetailGrid
            // 
            this.btnResetDetailGrid.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetDetailGrid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetDetailGrid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetDetailGrid.Location = new System.Drawing.Point(8, 9);
            this.btnResetDetailGrid.Name = "btnResetDetailGrid";
            this.btnResetDetailGrid.Size = new System.Drawing.Size(153, 38);
            this.btnResetDetailGrid.TabIndex = 7;
            this.btnResetDetailGrid.Text = "  Làm mới  lưới chi tiết";
            this.btnResetDetailGrid.Click += new System.EventHandler(this.btnResetDetailGrid_Click);
            // 
            // frmVideoTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmVideoTemplate";
            this.Text = "Mẫu phát Video";
            this.Load += new System.EventHandler(this.frmVideoTemplate_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLKVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDeleteChild)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gridVideo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewVideo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn colEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDetail;
        private DevExpress.XtraGrid.Columns.GridColumn ColDelete;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteCounter;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridChild;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewChild;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDeleteChild;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repLKVideo;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.SimpleButton btnResetTemplate;
        private DevExpress.XtraEditors.SimpleButton btnResetDetailGrid;
    }
}