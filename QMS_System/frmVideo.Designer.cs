namespace QMS_System
{
    partial class frmVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVideo));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gridChild = new DevExpress.XtraGrid.GridControl();
            this.gridViewChild = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repbtnDeleteChild = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridLookUpLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnReset = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDeleteChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // gridChild
            // 
            this.gridChild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridChild.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridChild.Location = new System.Drawing.Point(4, 49);
            this.gridChild.MainView = this.gridViewChild;
            this.gridChild.Name = "gridChild";
            this.gridChild.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repbtnDeleteChild,
            this.gridLookUpLanguage});
            this.gridChild.Size = new System.Drawing.Size(485, 379);
            this.gridChild.TabIndex = 2;
            this.gridChild.UseEmbeddedNavigator = true;
            this.gridChild.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewChild});
            // 
            // gridViewChild
            // 
            this.gridViewChild.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
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
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "gridColumn2";
            this.gridColumn9.FieldName = "Id";
            this.gridColumn9.Name = "gridColumn9";
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
            this.gridColumn11.Caption = "Tên tệp video";
            this.gridColumn11.FieldName = "FileName";
            this.gridColumn11.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 0;
            this.gridColumn11.Width = 127;
            // 
            // gridColumn14
            // 
            this.gridColumn14.ColumnEdit = this.repbtnDeleteChild;
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 1;
            this.gridColumn14.Width = 22;
            // 
            // repbtnDeleteChild
            // 
            this.repbtnDeleteChild.AutoHeight = false;
            this.repbtnDeleteChild.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repbtnDeleteChild.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.repbtnDeleteChild.Name = "repbtnDeleteChild";
            this.repbtnDeleteChild.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // gridLookUpLanguage
            // 
            this.gridLookUpLanguage.AutoHeight = false;
            this.gridLookUpLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpLanguage.Name = "gridLookUpLanguage";
            this.gridLookUpLanguage.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // btnReset
            // 
            this.btnReset.Image = global::QMS_System.Properties.Resources.if_refresh22_216527;
            this.btnReset.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnReset.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnReset.Location = new System.Drawing.Point(4, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(132, 38);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "  Làm mới  lưới";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::QMS_System.Properties.Resources.if_plus_sign_173078;
            this.btnAdd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnAdd.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnAdd.Location = new System.Drawing.Point(142, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(129, 38);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Thêm tệp video";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 430);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.gridChild);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmVideo";
            this.Text = "Danh sách tệp video";
            this.Load += new System.EventHandler(this.frmVideo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repbtnDeleteChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraGrid.GridControl gridChild;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewChild;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridLookUpLanguage;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repbtnDeleteChild;
        private DevExpress.XtraEditors.SimpleButton btnReset;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
    }
}