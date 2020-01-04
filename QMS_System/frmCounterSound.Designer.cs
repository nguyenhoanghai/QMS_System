namespace QMS_System
{
    partial class frmCounterSound
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
            this.gridCounterSound = new DevExpress.XtraGrid.GridControl();
            this.gridViewCounterSound = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpCounter = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpSound = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtn_deleteCounterSound = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnResetCounter = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetCounterSound = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetSound = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCounterSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCounterSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounterSound)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.gridCounterSound);
            this.groupControl2.Location = new System.Drawing.Point(1, 49);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(694, 356);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Danh mục Âm thanh Quầy";
            // 
            // gridCounterSound
            // 
            this.gridCounterSound.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridCounterSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCounterSound.Location = new System.Drawing.Point(2, 20);
            this.gridCounterSound.MainView = this.gridViewCounterSound;
            this.gridCounterSound.Name = "gridCounterSound";
            this.gridCounterSound.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lookUpCounter,
            this.lookUpSound,
            this.repbtn_deleteCounterSound});
            this.gridCounterSound.Size = new System.Drawing.Size(690, 334);
            this.gridCounterSound.TabIndex = 1;
            this.gridCounterSound.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCounterSound});
            // 
            // gridViewCounterSound
            // 
            this.gridViewCounterSound.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn14});
            this.gridViewCounterSound.GridControl = this.gridCounterSound;
            this.gridViewCounterSound.Name = "gridViewCounterSound";
            this.gridViewCounterSound.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewCounterSound.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewCounterSound.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewCounterSound.OptionsView.RowAutoHeight = true;
            this.gridViewCounterSound.OptionsView.ShowAutoFilterRow = true;
            this.gridViewCounterSound.OptionsView.ShowGroupPanel = false;
            this.gridViewCounterSound.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewCounterSound_CellValueChanged);
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
            this.gridColumn10.Caption = "Quầy";
            this.gridColumn10.ColumnEdit = this.lookUpCounter;
            this.gridColumn10.FieldName = "CounterId";
            this.gridColumn10.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 288;
            // 
            // lookUpCounter
            // 
            this.lookUpCounter.AutoHeight = false;
            this.lookUpCounter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpCounter.Name = "lookUpCounter";
            this.lookUpCounter.View = this.repositoryItemGridLookUpEdit1View;
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
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Arial", 9F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Âm thanh";
            this.gridColumn3.ColumnEdit = this.lookUpSound;
            this.gridColumn3.FieldName = "SoundId";
            this.gridColumn3.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 294;
            // 
            // lookUpSound
            // 
            this.lookUpSound.AutoHeight = false;
            this.lookUpSound.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpSound.Name = "lookUpSound";
            this.lookUpSound.View = this.repositoryItemGridLookUpEdit2View;
            // 
            // repositoryItemGridLookUpEdit2View
            // 
            this.repositoryItemGridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit2View.Name = "repositoryItemGridLookUpEdit2View";
            this.repositoryItemGridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
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
            this.gridColumn1.Caption = "Diễn giải";
            this.gridColumn1.FieldName = "Note";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            this.gridColumn1.Width = 355;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtn_deleteCounterSound;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 3;
            this.gridColumn14.Width = 70;
            // 
            // repbtn_deleteCounterSound
            // 
            this.repbtn_deleteCounterSound.AutoHeight = false;
            this.repbtn_deleteCounterSound.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::QMS_System.Properties.Resources.delete, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repbtn_deleteCounterSound.Name = "repbtn_deleteCounterSound";
            this.repbtn_deleteCounterSound.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repbtn_deleteCounterSound.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repbtn_deleteCounterSound_ButtonClick);
            // 
            // btnResetCounter
            // 
            this.btnResetCounter.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetCounter.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetCounter.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetCounter.Location = new System.Drawing.Point(231, 6);
            this.btnResetCounter.Name = "btnResetCounter";
            this.btnResetCounter.Size = new System.Drawing.Size(176, 38);
            this.btnResetCounter.TabIndex = 6;
            this.btnResetCounter.Text = "  Làm mới  danh sách Quầy";
            this.btnResetCounter.Click += new System.EventHandler(this.btnResetCounter_Click);
            // 
            // btnResetCounterSound
            // 
            this.btnResetCounterSound.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetCounterSound.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetCounterSound.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetCounterSound.Location = new System.Drawing.Point(6, 6);
            this.btnResetCounterSound.Name = "btnResetCounterSound";
            this.btnResetCounterSound.Size = new System.Drawing.Size(219, 38);
            this.btnResetCounterSound.TabIndex = 5;
            this.btnResetCounterSound.Text = "  Làm mới danh sách Âm thanh Quầy";
            this.btnResetCounterSound.Click += new System.EventHandler(this.btnResetCounterSound_Click);
            // 
            // btnResetSound
            // 
            this.btnResetSound.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnResetSound.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetSound.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnResetSound.Location = new System.Drawing.Point(413, 6);
            this.btnResetSound.Name = "btnResetSound";
            this.btnResetSound.Size = new System.Drawing.Size(196, 38);
            this.btnResetSound.TabIndex = 7;
            this.btnResetSound.Text = "  Làm mới  danh sách Âm thanh";
            this.btnResetSound.Click += new System.EventHandler(this.btnResetSound_Click);
            // 
            // frmCounterSound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 409);
            this.Controls.Add(this.btnResetSound);
            this.Controls.Add(this.btnResetCounter);
            this.Controls.Add(this.btnResetCounterSound);
            this.Controls.Add(this.groupControl2);
            this.Name = "frmCounterSound";
            this.Text = "Âm thanh Quầy";
            this.Load += new System.EventHandler(this.frmCounterSound_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCounterSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCounterSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtn_deleteCounterSound)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridCounterSound;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCounterSound;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpCounter;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit lookUpSound;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit2View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtn_deleteCounterSound;
        private DevExpress.XtraEditors.SimpleButton btnResetCounter;
        private DevExpress.XtraEditors.SimpleButton btnResetCounterSound;
        private DevExpress.XtraEditors.SimpleButton btnResetSound;
    }
}