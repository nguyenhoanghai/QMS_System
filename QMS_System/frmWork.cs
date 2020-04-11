using QMS_System.Data;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmWork : Form
    {
        int workTypeId = 0, workId = 0, workDetailId = 0;
        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        public frmWork()
        {
            InitializeComponent();
        }

        private void frmServiceDetail_Load(object sender, EventArgs e)
        {
            loadWorkType();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: loadWorkType(); break;
                case 1: loadWork(); break;
                case 2:
                    LoadWorkDetail();
                    cbWork.DataSource = null;
                    cbWork.DataSource = BLLWork.Instance.GetLookUp(frmMain_ver3.connectString);
                    cbWork.ValueMember = "Id";
                    cbWork.DisplayMember = "Name";

                    cbWorkType.DataSource = null;
                    cbWorkType.DataSource = BLLWorkType.Instance.GetLookUp(frmMain_ver3.connectString);
                    cbWorkType.ValueMember = "Id";
                    cbWorkType.DisplayMember = "Name";
                    break;
            }
        }

        #region WorkType
        private void loadWorkType()
        {
            gridWorkType.DataSource = null;
            gridWorkType.DataSource = BLLWorkType.Instance.Gets(frmMain_ver3.connectString);
        }

        private void btRefreshWT_Click(object sender, EventArgs e)
        {
            loadWorkType();
        }

        private void btAdd_WT_Click(object sender, EventArgs e)
        {
            btCancel_WT.PerformClick();
            btSave_WT.Enabled = true;
            btAdd_WT.Enabled = false;
        }

        private void btSave_WT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmaWT.Text))
            {
                MessageBox.Show("Vui lòng nhập mã .!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmaWT.Focus();
            }
            else if (string.IsNullOrEmpty(txtnameWT.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại công việc.! ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtnameWT.Focus();
            }
            else
            {
                var rs = BLLWorkType.Instance.Insert(frmMain_ver3.connectString, new Q_WorkType() { Id = workTypeId, Code = txtmaWT.Text, Name = txtnameWT.Text });
                if (rs.IsSuccess)
                {
                    workTypeId = 0;
                    btCancel_WT.PerformClick();
                    loadWorkType();
                }
                else
                    MessageBox.Show(rs.sms, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_WT_Click(object sender, EventArgs e)
        {
            txtmaWT.Text = "";
            txtnameWT.Text = "";
            btAdd_WT.Enabled = true;
            btSave_WT.Enabled = false;
        }

        private void repEdit_WT_Click(object sender, EventArgs e)
        {
            workTypeId = int.Parse(gridViewWorkType.GetRowCellValue(gridViewWorkType.FocusedRowHandle, "Id").ToString());
            txtmaWT.Text = gridViewWorkType.GetRowCellValue(gridViewWorkType.FocusedRowHandle, "Code").ToString();
            txtnameWT.Text = gridViewWorkType.GetRowCellValue(gridViewWorkType.FocusedRowHandle, "Name").ToString();
            btAdd_WT.Enabled = false;
            btSave_WT.Enabled = true;
        }

        private void repDelete_WT_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewWorkType.GetRowCellValue(gridViewWorkType.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLWorkType.Instance.Delete(frmMain_ver3.connectString, Id);
                loadWorkType();
            }
        }
         
        #endregion

        #region Work
        private void loadWork()
        {
            gridWork.DataSource = null;
            gridWork.DataSource = BLLWork.Instance.Gets(frmMain_ver3.connectString);
        }

        private void btSaveW_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaW.Text))
            {
                MessageBox.Show("Vui lòng nhập mã .!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaW.Focus();
            }
            else if (string.IsNullOrEmpty(txtNameW.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại công việc.! ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNameW.Focus();
            }
            else
            {
                var rs = BLLWork.Instance.Insert(frmMain_ver3.connectString, new Q_Works() { Id = workId, Code = txtMaW.Text, Name = txtNameW.Text });
                if (rs.IsSuccess)
                {
                    workId = 0;
                    btCancelW.PerformClick();
                    loadWork();
                }
                else
                    MessageBox.Show(rs.sms, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancelW_Click(object sender, EventArgs e)
        {
            txtMaW.Text = "";
            txtNameW.Text = "";
            btAddW.Enabled = true;
            btSaveW.Enabled = false;
        }

        private void repDeleteW_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewWork.GetRowCellValue(gridViewWork.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLWork.Instance.Delete(frmMain_ver3.connectString, Id);
                loadWork();
            }
        }

        private void repEditW_Click(object sender, EventArgs e)
        {
            workId = int.Parse(gridViewWork.GetRowCellValue(gridViewWork.FocusedRowHandle, "Id").ToString());
            txtMaW.Text = gridViewWork.GetRowCellValue(gridViewWork.FocusedRowHandle, "Code").ToString();
            txtNameW.Text = gridViewWork.GetRowCellValue(gridViewWork.FocusedRowHandle, "Name").ToString();
            btAddW.Enabled = false;
            btSaveW.Enabled = true;
        }

        private void btAddW_Click(object sender, EventArgs e)
        {
            btCancelW.PerformClick();
            btSaveW.Enabled = true;
            btAddW.Enabled = false;
        }

        private void btRefreshW_Click(object sender, EventArgs e)
        {
            loadWork();
        }

        #endregion

        #region Work detail
        private void LoadWorkDetail()
        {
            gridWorkDetail.DataSource = null;
            gridWorkDetail.DataSource = BLLWorkDetail.Instance.Gets(frmMain_ver3.connectString);
        }

        private void btAddWD_Click(object sender, EventArgs e)
        {
            btCancelWD.PerformClick();
            btSaveWD.Enabled = true;
            btAddWD.Enabled = false;
        }

        private void btRefreshWD_Click(object sender, EventArgs e)
        {
            LoadWorkDetail();
        }

        private void repDeleteWD_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridViewWorkDetail.GetRowCellValue(gridViewWorkDetail.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLWorkDetail.Instance.Delete(frmMain_ver3.connectString, Id);
                LoadWorkDetail();
            }
        }

        private void repEditWD_Click(object sender, EventArgs e)
        {
            try
            {
                workDetailId = int.Parse(gridViewWorkDetail.GetRowCellValue(gridViewWorkDetail.FocusedRowHandle, "Id").ToString());
                cbWork.Text = gridViewWorkDetail.GetRowCellValue(gridViewWorkDetail.FocusedRowHandle, "WorkName").ToString();
                cbWorkType.Text = gridViewWorkDetail.GetRowCellValue(gridViewWorkDetail.FocusedRowHandle, "WorkTypeName").ToString();
                dtTime.Value = DateTime.Parse(gridViewWorkDetail.GetRowCellValue(gridViewWorkDetail.FocusedRowHandle, "TimeProcess").ToString());
                btAddWD.Enabled = false;
                btSaveWD.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }


        private void btSaveWD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbWorkType.Text))
            {
                MessageBox.Show("Vui lòng chọn loại công việc.!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbWorkType.Focus();
            }
            else if (string.IsNullOrEmpty(cbWork.Text))
            {
                MessageBox.Show("Vui lòng chọn công việc.! ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbWork.Focus();
            }
            else
            {
                try
                {
                    var wId = (ModelSelectItem)cbWork.SelectedItem;
                    var wtId = (WorkTypeModel)cbWorkType.SelectedItem;
                    var rs = BLLWorkDetail.Instance.Insert(frmMain_ver3.connectString, new Q_WorkDetail() { Id = workDetailId, WorkId = wId.Id, WorkTypeId = wtId.Id, TimeProcess = dtTime.Value });
                    if (rs.IsSuccess)
                    {
                        workDetailId = 0;
                        btCancelWD.PerformClick();
                        LoadWorkDetail();
                    }
                    else
                        MessageBox.Show(rs.sms, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                { }
            }
        }

        private void btCancelWD_Click(object sender, EventArgs e)
        {
            cbWork.Text = "";
            cbWorkType.Text = "";
            dtTime.Value = date;
            btAddWD.Enabled = true;
            btSaveWD.Enabled = false;
        }
        #endregion
    }
}
