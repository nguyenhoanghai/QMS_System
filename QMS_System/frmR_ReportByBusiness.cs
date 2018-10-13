using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace QMS_System
{
    public partial class frmR_ReportByBusiness : Form
    {
        public frmR_ReportByBusiness()
        {
            InitializeComponent();
        }

        private void frmR_ReportByBusiness_Load(object sender, EventArgs e)
        {
            GetLookUp();
            gridReportByBusiness.Visible = false;
            txtQuantity.Visible = false;
            lblQuantity.Visible = false;
        }
        
        private void GetLookUp()
        {
            cbSelect.Properties.Items.Add("Tổng khách hàng chờ nội bộ");
            cbSelect.Properties.Items.Add("Tổng khách hàng đang giao dịch");
            cbSelect.Properties.Items.Add("Khách hàng chờ giao dịch lâu nhất");
            cbSelect.Properties.Items.Add("Khách hàng giao dịch lâu nhất");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            try
            {
                int index = cbSelect.SelectedIndex;
                if (index == -1)
                {
                    MessageBox.Show("Vui lòng chọn thông tin cần báo cáo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    switch (index)
                    {
                        case 0:
                            {
                                var list = BLLR_ReportByBusiness.Instance.GetWaitingTransInternal();
                                gridReportByBusiness.DataSource = null;
                                SetVisibleIndex();  // thiết lập thứ tự các column
                                gridReportByBusiness.DataSource = list;
                                gridReportByBusiness.Visible = true;
                                //gridViewDIDByUser.PopulateColumns();
                                for (int i = 0; i < gridViewReportByBusiness.Columns.Count; i++)
                                {
                                    if (gridViewReportByBusiness.Columns[i].FieldName != "Id")
                                        gridViewReportByBusiness.Columns[i].Visible = true;
                                }
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TicketNumber").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("PrintTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("ProcessTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("EndProcessTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("MajorName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("CounterName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("EquipmentName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("StatusCode").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalTransTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalInProgressTrans").Visible = false;

                                lblCurrentDate.Visible = true;
                                lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString();
                                //lblTotalTransaction.Text = "Tổng số lượt khách hàng giao dịch: " + list.Count.ToString();
                                break;
                            }
                        case 1:
                            {
                                var list = BLLR_ReportByBusiness.Instance.GetInProgressTrans();
                                gridReportByBusiness.DataSource = null;
                                SetVisibleIndex();  // thiết lập thứ tự các column
                                gridReportByBusiness.DataSource = list;
                                gridReportByBusiness.Visible = true;
                                //gridViewDIDByUser.PopulateColumns();
                                for (int i = 0; i < gridViewReportByBusiness.Columns.Count; i++)
                                {
                                    if (gridViewReportByBusiness.Columns[i].FieldName != "Id")
                                        gridViewReportByBusiness.Columns[i].Visible = true;
                                }
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TicketNumber").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("PrintTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("ProcessTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("EndProcessTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("MajorName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("CounterName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("EquipmentName").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("StatusCode").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalTransTime").Visible = false;
                                gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTransInternal").Visible = false;

                                lblCurrentDate.Visible = true;
                                lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString();
                                //lblTotalTransaction.Text = "Tổng số lượt khách hàng giao dịch: " + list.Count.ToString();

                                break;
                            }
                        case 2:
                            {

                                int num;
                                bool isNumeric = int.TryParse(txtQuantity.Text, out num);
                                if (txtQuantity.Text == string.Empty)
                                {
                                    MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtQuantity.Focus();
                                }
                                else if (isNumeric == false)
                                {
                                    MessageBox.Show("Số lượng phải là số. Xin nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //txtQuantity.Text = "";
                                    txtQuantity.Focus();
                                }
                                else
                                {
                                    var list = BLLR_ReportByBusiness.Instance.GetLongestWaitingBusiness(num);
                                    gridReportByBusiness.DataSource = null;
                                    SetVisibleIndex();  // thiết lập thứ tự các column
                                    gridReportByBusiness.DataSource = list;
                                    gridReportByBusiness.Visible = true;
                                    //gridViewDIDByUser.PopulateColumns();
                                    for (int i = 0; i < gridViewReportByBusiness.Columns.Count; i++)
                                    {
                                        if (gridViewReportByBusiness.Columns[i].FieldName != "Id")
                                            gridViewReportByBusiness.Columns[i].Visible = true;
                                    }
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("CounterName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("EquipmentName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("ServiceName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("EndProcessTime").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalTransTime").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("StatusCode").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalInProgressTrans").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTransInternal").Visible = false;

                                    //gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").AppearanceCell.ForeColor = Color.Red;
                                    //gridViewReportByBusiness.Columns.ColumnByFieldName("MajorName").AppearanceCell.ForeColor = Color.Black;
                                    lblCurrentDate.Visible = true;
                                    lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString();
                                    //lblTotalTransaction.Text = "Tổng số lượt khách hàng giao dịch: " + list.Count.ToString();

                                }
                                break;
                            }

                        case 3:
                            {
                                int num;
                                bool isNumeric = int.TryParse(txtQuantity.Text, out num);
                                if (txtQuantity.Text == string.Empty)
                                {
                                    MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtQuantity.Focus();
                                }
                                else if (isNumeric == false)
                                {
                                    MessageBox.Show("Số lượng phải là số. Xin nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //txtQuantity.Text = "";
                                    txtQuantity.Focus();
                                }
                                else
                                {
                                    var list = BLLR_ReportByBusiness.Instance.GetLongestTransBusiness(num);
                                    gridReportByBusiness.DataSource = null;
                                    SetVisibleIndex();
                                    gridReportByBusiness.DataSource = list;
                                    gridReportByBusiness.Visible = true;
                                    for (int i = 0; i < gridViewReportByBusiness.Columns.Count; i++)
                                    {
                                        if (gridViewReportByBusiness.Columns[i].FieldName != "Id")
                                            gridViewReportByBusiness.Columns[i].Visible = true;
                                    }
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("EquipmentName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("ServiceName").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("PrintTime").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("StatusCode").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTime").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalInProgressTrans").Visible = false;
                                    gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTransInternal").Visible = false;

                                    //gridViewReportByBusiness.Columns.ColumnByFieldName("MajorName").AppearanceCell.ForeColor = Color.Red;
                                    //gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").AppearanceCell.ForeColor = Color.Black;
                                    lblCurrentDate.Visible = true;
                                    lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString();
                                    //lblTotalTransaction.Text = "Tổng số lượt khách hàng giao dịch: " + list.Count.ToString();
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            
        }

        private void SetVisibleIndex()
        {
            gridViewReportByBusiness.Columns.ColumnByFieldName("Id").VisibleIndex = -1;
            gridViewReportByBusiness.Columns.ColumnByFieldName("Index").VisibleIndex = 0;
            gridViewReportByBusiness.Columns.ColumnByFieldName("TicketNumber").VisibleIndex = 1;
            gridViewReportByBusiness.Columns.ColumnByFieldName("MajorName").VisibleIndex = 2;
            gridViewReportByBusiness.Columns.ColumnByFieldName("PrintTime").VisibleIndex = 3;
            gridViewReportByBusiness.Columns.ColumnByFieldName("ProcessTime").VisibleIndex = 4;
            gridViewReportByBusiness.Columns.ColumnByFieldName("EndProcessTime").VisibleIndex = 5;
            gridViewReportByBusiness.Columns.ColumnByFieldName("TotalTransTime").VisibleIndex = 6;
            gridViewReportByBusiness.Columns.ColumnByFieldName("UserName").VisibleIndex = 7;
            gridViewReportByBusiness.Columns.ColumnByFieldName("CounterName").VisibleIndex = 8;
            gridViewReportByBusiness.Columns.ColumnByFieldName("EquipmentName").VisibleIndex = 9;
            gridViewReportByBusiness.Columns.ColumnByFieldName("ServiceName").VisibleIndex = 10;
            gridViewReportByBusiness.Columns.ColumnByFieldName("StatusCode").VisibleIndex = 11;
            gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTime").VisibleIndex = 12;
            gridViewReportByBusiness.Columns.ColumnByFieldName("TotalInProgressTrans").VisibleIndex = 13;
            gridViewReportByBusiness.Columns.ColumnByFieldName("TotalWaitingTransInternal").VisibleIndex = 14;

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cbSelect.SelectedIndex;
                if (index == -1)
                {
                    MessageBox.Show("Vui lòng chọn thông tin cần báo cáo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    switch (index)
                    {
                        case 0:
                            {
                                var listObjs = BLLR_ReportByBusiness.Instance.GetWaitingTransInternal();

                                var excelPackage = new ExcelPackage();
                                excelPackage.Workbook.Properties.Author = "GPRO";
                                excelPackage.Workbook.Properties.Title = "Báo Cáo SL Khách Hàng Chờ Nội Bộ";
                                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                                sheet.Name = "Sheet1";
                                sheet.Cells.Style.Font.Size = 12;
                                sheet.Cells.Style.Font.Name = "Times New Roman";
                                sheet.Cells.AutoFitColumns();
                                sheet.Cells[1, 2].Value = "DANH SÁCH KHÁCH HÀNG CHỜ NỘI BỘ";
                                sheet.Cells[1, 2].Style.Font.Size = 14;
                                sheet.Cells[1, 2, 1, 4].Merge = true;
                                sheet.Cells[1, 2, 1, 4].Style.Font.Bold = true;
                                sheet.Cells[1, 2].Style.WrapText = true;
                                sheet.Cells[1, 2, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[1, 2, 1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                sheet.Cells[1, 2, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[1, 2, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                sheet.Cells[1, 2, 1, 4].Style.Font.Color.SetColor(Color.White);

                                sheet.Row(1).Height = 25;
                                sheet.Row(3).Height = 20;
                                sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                sheet.Row(3).Style.Font.Bold = true;

                                sheet.Cells[3, 2, 3, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[3, 2, 3, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                sheet.Cells[3, 2, 3, 4].Style.Font.Color.SetColor(Color.White);

                                sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                                //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                sheet.Cells[2, 2].Style.Font.Bold = true;
                                sheet.Cells[2, 2, 2, 4].Merge = true;
                                int rowIndex = 3;

                                sheet.Cells[rowIndex, 2].Value = "STT";
                                sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[rowIndex, 3].Value = "Dịch vụ";
                                sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[rowIndex, 4].Value = "Số khách hàng chờ nội bộ";
                                sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                rowIndex++;
                                if (listObjs.Count > 0)
                                {
                                    foreach (var item in listObjs)
                                    {
                                        sheet.Cells[rowIndex, 2].Value = item.Index;
                                        sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        sheet.Cells[rowIndex, 3].Value = item.ServiceName;
                                        sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        sheet.Cells[rowIndex, 4].Value = item.TotalWaitingTransInternal;
                                        sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                        rowIndex++;
                                    }
                                }

                                sheet.Column(2).Width = 10;
                                sheet.Column(3).Width = 50;
                                sheet.Column(4).Width = 50;

                                for (int i = 2; i <= 4; i++)
                                {
                                    sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }

                                sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                SaveFileDialog dialog = new SaveFileDialog();
                                dialog.DefaultExt = "xlsx";
                                dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                                dialog.InitialDirectory = @"C:\";
                                dialog.Title = "Save File As";
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    string filename = dialog.FileName;
                                    //if (File.Exists(filename))
                                    //    File.Delete(filename);

                                    //Create excel file on physical disk    
                                    FileStream objFileStrm = File.Create(filename);
                                    objFileStrm.Close();

                                    //Write content to excel file    
                                    File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                                    if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        if (File.Exists(filename))
                                            System.Diagnostics.Process.Start(filename);
                                        else
                                        {
                                            MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                break;
                            }
                        case 1:
                            {

                                var listObjs = BLLR_ReportByBusiness.Instance.GetInProgressTrans();

                                var excelPackage = new ExcelPackage();
                                excelPackage.Workbook.Properties.Author = "GPRO";
                                excelPackage.Workbook.Properties.Title = "Báo Cáo SL Khách Hàng Đang Giao Dịch";
                                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                                sheet.Name = "Sheet1";
                                sheet.Cells.Style.Font.Size = 12;
                                sheet.Cells.Style.Font.Name = "Times New Roman";
                                sheet.Cells.AutoFitColumns();
                                sheet.Cells[1, 2].Value = "DANH SÁCH KHÁCH HÀNG ĐANG GIAO DỊCH";
                                sheet.Cells[1, 2].Style.Font.Size = 14;
                                sheet.Cells[1, 2, 1, 4].Merge = true;
                                sheet.Cells[1, 2, 1, 4].Style.Font.Bold = true;
                                sheet.Cells[1, 2].Style.WrapText = true;
                                sheet.Cells[1, 2, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[1, 2, 1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                sheet.Cells[1, 2, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[1, 2, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                sheet.Cells[1, 2, 1, 4].Style.Font.Color.SetColor(Color.White);

                                sheet.Row(1).Height = 25;
                                sheet.Row(3).Height = 20;
                                sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                sheet.Row(3).Style.Font.Bold = true;

                                sheet.Cells[3, 2, 3, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[3, 2, 3, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                sheet.Cells[3, 2, 3, 4].Style.Font.Color.SetColor(Color.White);

                                sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                                //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                sheet.Cells[2, 2].Style.Font.Bold = true;
                                sheet.Cells[2, 2, 2, 4].Merge = true;
                                int rowIndex = 3;

                                sheet.Cells[rowIndex, 2].Value = "STT";
                                sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[rowIndex, 3].Value = "Dịch vụ";
                                sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet.Cells[rowIndex, 4].Value = "Số khách hàng đang giao dịch";
                                sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                rowIndex++;
                                if (listObjs.Count > 0)
                                {
                                    foreach (var item in listObjs)
                                    {
                                        sheet.Cells[rowIndex, 2].Value = item.Index;
                                        sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        sheet.Cells[rowIndex, 3].Value = item.ServiceName;
                                        sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                        sheet.Cells[rowIndex, 4].Value = item.TotalInProgressTrans;
                                        sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                        rowIndex++;
                                    }
                                }

                                sheet.Column(2).Width = 10;
                                sheet.Column(3).Width = 50;
                                sheet.Column(4).Width = 50;

                                for (int i = 2; i <= 4; i++)
                                {
                                    sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                                sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                SaveFileDialog dialog = new SaveFileDialog();
                                dialog.DefaultExt = "xlsx";
                                dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                                dialog.InitialDirectory = @"C:\";
                                dialog.Title = "Save File As";
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    string filename = dialog.FileName;
                                    //if (File.Exists(filename))
                                    //    File.Delete(filename);

                                    //Create excel file on physical disk    
                                    FileStream objFileStrm = File.Create(filename);
                                    objFileStrm.Close();

                                    //Write content to excel file    
                                    File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                                    if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        if (File.Exists(filename))
                                            System.Diagnostics.Process.Start(filename);
                                        else
                                        {
                                            MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                break;
                            }
                        case 2:
                            {
                                int num;
                                bool isNumeric = int.TryParse(txtQuantity.Text, out num);
                                if (txtQuantity.Text == string.Empty)
                                {
                                    MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtQuantity.Focus();
                                }
                                else if (isNumeric == false)
                                {
                                    MessageBox.Show("Số lượng phải là số. Xin nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //txtQuantity.Text = "";
                                    txtQuantity.Focus();
                                }
                                else
                                {
                                    var listObjs = BLLR_ReportByBusiness.Instance.GetLongestWaitingBusiness(num);
                                    var excelPackage = new ExcelPackage();
                                    excelPackage.Workbook.Properties.Author = "GPRO";
                                    excelPackage.Workbook.Properties.Title = "Báo Cáo Khách Hàng Có Thời Gian Chờ Lâu Nhất";
                                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                                    sheet.Name = "Sheet1";
                                    sheet.Cells.Style.Font.Size = 12;
                                    sheet.Cells.Style.Font.Name = "Times New Roman";
                                    sheet.Cells.AutoFitColumns();
                                    sheet.Cells[1, 2].Value = "DANH SÁCH KHÁCH HÀNG CÓ THỜI GIAN CHỜ LÂU NHẤT";
                                    sheet.Cells[1, 2].Style.Font.Size = 14;
                                    sheet.Cells[1, 2, 1, 7].Merge = true;
                                    sheet.Cells[1, 2, 1, 7].Style.Font.Bold = true;
                                    sheet.Cells[1, 2].Style.WrapText = true;
                                    sheet.Cells[1, 2, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[1, 2, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    sheet.Cells[1, 2, 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[1, 2, 1, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                    sheet.Cells[1, 2, 1, 7].Style.Font.Color.SetColor(Color.White);

                                    sheet.Row(1).Height = 25;
                                    sheet.Row(3).Height = 20;
                                    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    sheet.Row(3).Style.Font.Bold = true;

                                    sheet.Cells[3, 2, 3, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[3, 2, 3, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                    sheet.Cells[3, 2, 3, 7].Style.Font.Color.SetColor(Color.White);

                                    sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                                    //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    sheet.Cells[2, 2].Style.Font.Bold = true;
                                    sheet.Cells[2, 2, 2, 7].Merge = true;
                                    int rowIndex = 3;

                                    sheet.Cells[rowIndex, 2].Value = "STT";
                                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 3].Value = "Số phiếu";
                                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 4].Value = "Nghiệp vụ";
                                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 5].Value = "Giờ lấy phiếu";
                                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 6].Value = "Giờ giao dịch";
                                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 7].Value = "Thời gian chờ";
                                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    rowIndex++;
                                    if (listObjs.Count > 0)
                                    {
                                        foreach (var item in listObjs)
                                        {
                                            sheet.Cells[rowIndex, 2].Value = item.Index;
                                            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 3].Value = item.TicketNumber;
                                            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 4].Value = item.MajorName;
                                            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            //sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
                                            sheet.Cells[rowIndex, 5].Value = String.Format("{0:HH:mm:ss}", item.PrintTime);
                                            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 6].Value = String.Format("{0:HH:mm:ss}", item.ProcessTime);
                                            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 7].Value = item.TotalWaitingTime;
                                            sheet.Cells[rowIndex, 7].Style.Numberformat.Format = "HH:mm:ss";  // vi la kieu TimeSpan nen dung dinh dang
                                            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                            rowIndex++;
                                        }
                                    }

                                    sheet.Cells.AutoFitColumns(5);
                                    sheet.Column(2).Width = 10;
                                    sheet.Column(3).Width = 20;
                                    sheet.Column(4).Width = 30;
                                    sheet.Column(5).Width = 30;
                                    sheet.Column(6).Width = 30;
                                    sheet.Column(7).Width = 30;
                                    for (int i = 2; i <= 7; i++)
                                    {
                                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    SaveFileDialog dialog = new SaveFileDialog();
                                    dialog.DefaultExt = "xlsx";
                                    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                                    dialog.InitialDirectory = @"C:\";
                                    dialog.Title = "Save File As";
                                    if (dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        string filename = dialog.FileName;
                                        //if (File.Exists(filename))
                                        //    File.Delete(filename);

                                        //Create excel file on physical disk    
                                        FileStream objFileStrm = File.Create(filename);
                                        objFileStrm.Close();

                                        //Write content to excel file    
                                        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                                        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            if (File.Exists(filename))
                                                System.Diagnostics.Process.Start(filename);
                                            else
                                            {
                                                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                int num;
                                bool isNumeric = int.TryParse(txtQuantity.Text, out num);
                                if (txtQuantity.Text == string.Empty)
                                {
                                    MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtQuantity.Focus();
                                }
                                else if (isNumeric == false)
                                {
                                    MessageBox.Show("Số lượng phải là số. Xin nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //txtQuantity.Text = "";
                                    txtQuantity.Focus();
                                }
                                else
                                {
                                    var listObjs = BLLR_ReportByBusiness.Instance.GetLongestTransBusiness(num);
                                    var excelPackage = new ExcelPackage();
                                    excelPackage.Workbook.Properties.Author = "GPRO";
                                    excelPackage.Workbook.Properties.Title = "Báo Cáo Khách Hàng Có Thời Gian Giao Dịch Lâu Nhất";
                                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                                    sheet.Name = "Sheet1";
                                    sheet.Cells.Style.Font.Size = 12;
                                    sheet.Cells.Style.Font.Name = "Times New Roman";
                                    sheet.Cells.AutoFitColumns();
                                    sheet.Cells[1, 2].Value = "DANH SÁCH KHÁCH HÀNG CÓ THỜI GIAN GIAO DỊCH LÂU NHẤT";
                                    sheet.Cells[1, 2].Style.Font.Size = 14;
                                    sheet.Cells[1, 2, 1, 9].Merge = true;
                                    sheet.Cells[1, 2, 1, 9].Style.Font.Bold = true;
                                    sheet.Cells[1, 2].Style.WrapText = true;
                                    sheet.Cells[1, 2, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[1, 2, 1, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    sheet.Cells[1, 2, 1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[1, 2, 1, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                    sheet.Cells[1, 2, 1, 9].Style.Font.Color.SetColor(Color.White);

                                    sheet.Row(1).Height = 25;
                                    sheet.Row(3).Height = 20;
                                    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    sheet.Row(3).Style.Font.Bold = true;

                                    sheet.Cells[3, 2, 3, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[3, 2, 3, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                                    sheet.Cells[3, 2, 3, 9].Style.Font.Color.SetColor(Color.White);

                                    sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year;
                                    //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    sheet.Cells[2, 2].Style.Font.Bold = true;
                                    sheet.Cells[2, 2, 2, 9].Merge = true;
                                    int rowIndex = 3;

                                    sheet.Cells[rowIndex, 2].Value = "STT";
                                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 3].Value = "Số phiếu";
                                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 4].Value = "Nghiệp vụ";
                                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 5].Value = "Giờ giao dịch";
                                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 6].Value = "Giờ kết thúc";
                                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 7].Value = "Thời gian giao dịch";
                                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 8].Value = "Nhân viên";
                                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    sheet.Cells[rowIndex, 9].Value = "Quầy";
                                    sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    rowIndex++;
                                    if (listObjs.Count > 0)
                                    {
                                        foreach (var item in listObjs)
                                        {
                                            sheet.Cells[rowIndex, 2].Value = item.Index;
                                            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 3].Value = item.TicketNumber;
                                            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 4].Value = item.MajorName;
                                            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            //sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
                                            sheet.Cells[rowIndex, 5].Value = String.Format("{0:HH:mm:ss}", item.ProcessTime);
                                            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 6].Value = String.Format("{0:HH:mm:ss}", item.EndProcessTime);
                                            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 7].Value = item.TotalTransTime;
                                            sheet.Cells[rowIndex, 7].Style.Numberformat.Format = "HH:mm:ss";  // vi la kieu TimeSpan nen dung dinh dang
                                            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 8].Value = item.UserName;
                                            sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                            sheet.Cells[rowIndex, 9].Value = item.CounterName;
                                            sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                            rowIndex++;
                                        }
                                    }

                                    sheet.Cells.AutoFitColumns(5);
                                    sheet.Column(2).Width = 10;
                                    sheet.Column(3).Width = 20;
                                    sheet.Column(4).Width = 30;
                                    sheet.Column(5).Width = 20;
                                    sheet.Column(6).Width = 20;
                                    sheet.Column(7).Width = 20;
                                    sheet.Column(8).Width = 30;
                                    sheet.Column(9).Width = 20;
                                    for (int i = 2; i <= 9; i++)
                                    {
                                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }

                                    sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    SaveFileDialog dialog = new SaveFileDialog();
                                    dialog.DefaultExt = "xlsx";
                                    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                                    dialog.InitialDirectory = @"C:\";
                                    dialog.Title = "Save File As";
                                    if (dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        string filename = dialog.FileName;
                                        //if (File.Exists(filename))
                                        //    File.Delete(filename);

                                        //Create excel file on physical disk    
                                        FileStream objFileStrm = File.Create(filename);
                                        objFileStrm.Close();

                                        //Write content to excel file    
                                        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                                        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            if (File.Exists(filename))
                                                System.Diagnostics.Process.Start(filename);
                                            else
                                            {
                                                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridReportByBusiness.Visible = false;
            lblCurrentDate.Visible = false;
            if (cbSelect.SelectedIndex == 2 || cbSelect.SelectedIndex == 3)
            {
                lblQuantity.Visible = true;
                txtQuantity.Visible = true;
                txtQuantity.Text = "";
            }
            else
            {
                lblQuantity.Visible = false;
                txtQuantity.Visible = false;
                txtQuantity.Text = "";
            }
        }
    }
}
