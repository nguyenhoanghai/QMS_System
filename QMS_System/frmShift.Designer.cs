namespace QMS_System
{
    partial class frmShift
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShift));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnResetShift = new DevExpress.XtraEditors.SimpleButton();
            this.gridShift = new DevExpress.XtraGrid.GridControl();
            this.gridViewShift = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTimeStart = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTimeEnd = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteShift = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnResetShift
            // 
            this.btnResetShift.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetShift.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetShift.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetShift.Location = new System.Drawing.Point(6, 4);
            this.btnResetShift.Name = "btnResetShift";
            this.btnResetShift.Size = new System.Drawing.Size(227, 38);
            this.btnResetShift.TabIndex = 18;
            this.btnResetShift.Text = "  Làm mới  danh sách ca làm việc";
            this.btnResetShift.Click += new System.EventHandler(this.btnResetShift_Click);
            // 
            // gridShift
            // 
            this.gridShift.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridShift.Location = new System.Drawing.Point(2, 20);
            this.gridShift.MainView = this.gridViewShift;
            this.gridShift.Name = "gridShift";
            this.gridShift.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteShift,
            this.repTimeStart,
            this.repTimeEnd});
            this.gridShift.Size = new System.Drawing.Size(577, 270);
            this.gridShift.TabIndex = 1;
            this.gridShift.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewShift});
            // 
            // gridViewShift
            // 
            this.gridViewShift.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn13,
            this.gridColumn14});
            this.gridViewShift.GridControl = this.gridShift;
            this.gridViewShift.Name = "gridViewShift";
            this.gridViewShift.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewShift.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewShift.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewShift.OptionsView.RowAutoHeight = true;
            this.gridViewShift.OptionsView.ShowAutoFilterRow = true;
            this.gridViewShift.OptionsView.ShowGroupPanel = false;
            this.gridViewShift.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewShift_CellValueChanged);
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn2";
            this.gridColumn8.FieldName = "Id";
            this.gridColumn8.Name = "gridColumn8";
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
            this.gridColumn10.Caption = "Tên ca làm việc";
            this.gridColumn10.FieldName = "Name";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 322;
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
            this.gridColumn1.Caption = "Thời gian bắt đầu";
            this.gridColumn1.ColumnEdit = this.repTimeStart;
            this.gridColumn1.FieldName = "Start";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 257;
            // 
            // repTimeStart
            // 
            this.repTimeStart.AutoHeight = false;
            this.repTimeStart.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repTimeStart.DisplayFormat.FormatString = "HH:mm:ss tt";
            this.repTimeStart.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repTimeStart.EditFormat.FormatString = "HH:mm:ss tt";
            this.repTimeStart.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repTimeStart.Name = "repTimeStart";
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
            this.gridColumn2.Caption = "Thời gian kết thúc";
            this.gridColumn2.ColumnEdit = this.repTimeEnd;
            this.gridColumn2.FieldName = "End";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 237;
            // 
            // repTimeEnd
            // 
            this.repTimeEnd.AutoHeight = false;
            this.repTimeEnd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repTimeEnd.DisplayFormat.FormatString = "HH:mm:ss tt";
            this.repTimeEnd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repTimeEnd.EditFormat.FormatString = "HH:mm:ss tt";
            this.repTimeEnd.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repTimeEnd.Name = "repTimeEnd";
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn13.AppearanceCell.Options.UseFont = true;
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn13.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn13.AppearanceHeader.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn13.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn13.Caption = "Diễn giải";
            this.gridColumn13.FieldName = "Note";
            this.gridColumn13.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 3;
            this.gridColumn13.Width = 325;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtn_deleteShift;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 4;
            // 
            // repbtn_deleteShift
            // 
            this.repbtn_deleteShift.AutoHeight = false;
            this.repbtn_deleteShift.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtn_deleteShift.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deleteShift.Name = "repbtn_deleteShift";
            this.repbtn_deleteShift.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteShift.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deleteShift_ButtonClick);
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.gridShift);
            this.groupControl2.Location = new System.Drawing.Point(0, 48);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(581, 292);
            this.groupControl2.TabIndex = 16;
            this.groupControl2.Text = "Danh mục ca làm việc";
            // 
            // frmShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 344);
            this.Controls.Add(this.btnResetShift);
            this.Controls.Add(this.groupControl2);
            this.Name = "frmShift";
            this.Text = "Ca Làm Việc";
            this.Load += new System.EventHandler(this.frmShift_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnResetShift;
        private DevExpress.XtraGrid.GridControl gridShift;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewShift;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repTimeStart;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repTimeEnd;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteShift;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}