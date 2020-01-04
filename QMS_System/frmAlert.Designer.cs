namespace QMS_System
{
    partial class frmAlert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlert));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridAlert = new DevExpress.XtraGrid.GridControl();
            this.gridViewAlert = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpSound = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTimeStart = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTimeEnd = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteAlert = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetAlert = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetSound = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteAlert)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.gridAlert);
            this.groupControl2.Location = new System.Drawing.Point(-1, 50);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(670, 292);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Danh mục câu hướng dẫn";
            // 
            // gridAlert
            // 
            this.gridAlert.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAlert.Location = new System.Drawing.Point(2, 20);
            this.gridAlert.MainView = this.gridViewAlert;
            this.gridAlert.Name = "gridAlert";
            this.gridAlert.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtn_deleteAlert,
            this.lookUpSound,
            this.repTimeStart,
            this.repTimeEnd});
            this.gridAlert.Size = new System.Drawing.Size(666, 270);
            this.gridAlert.TabIndex = 1;
            this.gridAlert.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAlert});
            // 
            // gridViewAlert
            // 
            this.gridViewAlert.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn13,
            this.gridColumn14});
            this.gridViewAlert.GridControl = this.gridAlert;
            this.gridViewAlert.Name = "gridViewAlert";
            this.gridViewAlert.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewAlert.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewAlert.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewAlert.OptionsView.RowAutoHeight = true;
            this.gridViewAlert.OptionsView.ShowAutoFilterRow = true;
            this.gridViewAlert.OptionsView.ShowGroupPanel = false;
            this.gridViewAlert.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewAlert_CellValueChanged);
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
            this.gridColumn10.Caption = "Âm thanh";
            this.gridColumn10.ColumnEdit = this.lookUpSound;
            this.gridColumn10.FieldName = "SoundId";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 322;
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
            this.gridColumn14.ColumnEdit = this.repbtn_deleteAlert;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 4;
            // 
            // repbtn_deleteAlert
            // 
            this.repbtn_deleteAlert.AutoHeight = false;
            this.repbtn_deleteAlert.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtn_deleteAlert.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deleteAlert.Name = "repbtn_deleteAlert";
            this.repbtn_deleteAlert.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteAlert.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deleteAlert_ButtonClick);
            // 
            // btnResetAlert
            // 
            this.btnResetAlert.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetAlert.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetAlert.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetAlert.Location = new System.Drawing.Point(227, 6);
            this.btnResetAlert.Name = "btnResetAlert";
            this.btnResetAlert.Size = new System.Drawing.Size(227, 38);
            this.btnResetAlert.TabIndex = 15;
            this.btnResetAlert.Text = "  Làm mới  danh sách câu hướng dẫn";
            this.btnResetAlert.Click += new System.EventHandler(this.btnResetAlert_Click);
            // 
            // btnResetSound
            // 
            this.btnResetSound.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetSound.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetSound.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetSound.Location = new System.Drawing.Point(8, 6);
            this.btnResetSound.Name = "btnResetSound";
            this.btnResetSound.Size = new System.Drawing.Size(213, 38);
            this.btnResetSound.TabIndex = 14;
            this.btnResetSound.Text = "  Làm mới  danh sách âm thanh";
            this.btnResetSound.Click += new System.EventHandler(this.btnResetSound_Click);
            // 
            // frmAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 344);
            this.Controls.Add(this.btnResetAlert);
            this.Controls.Add(this.btnResetSound);
            this.Controls.Add(this.groupControl2);
            this.Name = "frmAlert";
            this.Text = "Câu hướng dẫn";
            this.Load += new System.EventHandler(this.frmAlert_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTimeEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteAlert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridAlert;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpSound;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteAlert;
        private DevExpress.XtraEditors.SimpleButton btnResetAlert;
        private DevExpress.XtraEditors.SimpleButton btnResetSound;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repTimeStart;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repTimeEnd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAlert;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
    }
}