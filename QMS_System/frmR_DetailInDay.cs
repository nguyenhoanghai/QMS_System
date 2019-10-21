using DevExpress.XtraGrid.Views.Grid;
using QMS_System.Data.BLL;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMS_System.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Columns;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using QMS_System.Helper;
using GPRO.Core.Hai;

namespace QMS_System
{
    public partial class frmR_DetailInDay : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        public frmR_DetailInDay()
        {
            InitializeComponent();
        }

        private void frmR_DetailInDay_Load(object sender, EventArgs e)
        {
            gridNV.Dock = DockStyle.Fill;
            gridNghiepVu.Dock = DockStyle.Fill;
            gridDV.Dock = DockStyle.Fill;
            lookUpSelect.Properties.NullText = string.Empty;
            radioGroup1.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                var list = BLLReport.Instance.DetailReport(connect,((string.IsNullOrEmpty(lookUpSelect.EditValue.ToString()) || lookUpSelect.EditValue.ToString() == "-1") ? 0 : int.Parse(lookUpSelect.EditValue.ToString())), value, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),DateTime.Now);
                switch (value)
                {
                    case 1:
                        {
                            gridNV.DataSource = null;
                            gridNV.DataSource = list;
                            lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString() + "    Tổng số lượt khách hàng giao dịch: " + list.Count.ToString();
                            break;
                        }

                    case 2:
                        {
                            gridNghiepVu.DataSource = null;
                            gridNghiepVu.DataSource = list;
                            lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString() + "    Tổng số lượt khách hàng giao dịch: " + list.Count.ToString(); ;
                            break;
                        }

                    case 3:
                        {
                            gridDV.DataSource = null;
                            gridDV.DataSource = list;
                            lblCurrentDate.Text = "Ngày " + DateTime.Now.Day.ToString() + " Tháng " + DateTime.Now.Month.ToString() + " Năm " + DateTime.Now.Year.ToString() + "    Tổng số lượt khách hàng giao dịch: " + list.Count.ToString(); ;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                SaveFileDialog vrow = new SaveFileDialog();
                vrow.DefaultExt = "xlsx";
                vrow.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                vrow.InitialDirectory = @"C:\";
                vrow.Title = "Save File As"; 
                if (vrow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = vrow.FileName;
                    switch (value)
                    {
                        case 1:
                            {
                             //   path += "\\ThongTinNgay_NhanVien_" + DateTime.Now.ToString("ddMMyyyHHmm")+".xlsx";
                                gridNV.ExportToXlsx(path);
                                break;
                            }

                        case 2:
                            {
                               // path += "\\ThongTinNgay_NghiepVu_" + DateTime.Now.ToString("ddMMyyyHHmm") + ".xlsx";
                                gridNghiepVu.ExportToXlsx(path);
                                break;
                            }

                        case 3:
                            {
                              //  path += "\\ThongTinNgay_DichVu_" + DateTime.Now.ToString("ddMMyyyHHmm") + ".xlsx";
                                gridDV.ExportToXlsx(path);
                                break;
                            }
                    }
                    if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (File.Exists(path))
                            System.Diagnostics.Process.Start(path);
                        else
                        {
                            MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            //try
            //{
            //    if (radioGroup1.SelectedIndex == -1)
            //    {

            //    }
            //    else
            //    {
            //        int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
            //        switch (value)
            //        {
            //            case 1:
            //                if (int.Parse(lookUpSelect.EditValue.ToString()) == -1)
            //                {
            //                    MessageBox.Show("Vui lòng chọn nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //                else
            //                {
            //                    int userId = int.Parse(lookUpSelect.EditValue.ToString());

            //                    var listObjs = BLLR_DetailInDay.Instance.GetDetailInDayByUser(userId);
            //                    var excelPackage = new ExcelPackage();
            //                    excelPackage.Workbook.Properties.Author = "GPRO";
            //                    excelPackage.Workbook.Properties.Title = "Báo Cáo Chi Tiết Trong Ngày Theo Nhân Viên";
            //                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            //                    sheet.Name = "Sheet1";
            //                    sheet.Cells.Style.Font.Size = 12;
            //                    sheet.Cells.Style.Font.Name = "Times New Roman";
            //                    sheet.Cells.AutoFitColumns();
            //                    if (userId == 0)
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA TẤT CẢ NHÂN VIÊN";
            //                    else
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA NHÂN VIÊN" + (listObjs.Count > 0 ? ": " + listObjs[0].UserName.ToUpper() : string.Empty);
            //                    sheet.Cells[1, 2].Style.Font.Size = 14;
            //                    sheet.Cells[1, 2, 1, 10].Merge = true;
            //                    sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
            //                    sheet.Cells[1, 2].Style.WrapText = true;
            //                    sheet.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Row(1).Height = 25;
            //                    sheet.Row(3).Height = 20;
            //                    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Row(3).Style.Font.Bold = true;

            //                    sheet.Cells[3, 2, 3, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[3, 2, 3, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[3, 2, 3, 10].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year + "   " + "Tổng số lượt khách giao dịch: " + listObjs.Count.ToString();
            //                    //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //                    sheet.Cells[2, 2].Style.Font.Bold = true;
            //                    sheet.Cells[2, 2, 2, 10].Merge = true;
            //                    int rowIndex = 3;

            //                    sheet.Cells[rowIndex, 2].Value = "STT";
            //                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 3].Value = "Số phiếu";
            //                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 4].Value = "Nhân viên";
            //                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 5].Value = "Thiết bị";
            //                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 6].Value = "Nghiệp vụ";
            //                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 7].Value = "Giờ lấy phiếu";
            //                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 8].Value = "Giờ giao dịch";
            //                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 9].Value = "Giờ kết thúc";
            //                    sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 10].Value = "Thời gian giao dịch";
            //                    sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //                    rowIndex++;
            //                    if (listObjs.Count > 0)
            //                    {
            //                        foreach (var item in listObjs)
            //                        {
            //                            sheet.Cells[rowIndex, 2].Value = item.Index;
            //                            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 3].Value = item.TicketNumber;
            //                            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 4].Value = item.UserName;
            //                            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
            //                            sheet.Cells[rowIndex, 5].Value = item.EquipmentName;
            //                            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 6].Value = item.MajorName;
            //                            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 7].Value = String.Format("{0:HH:mm:ss}", item.PrintTime);
            //                            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 8].Value = String.Format("{0:HH:mm:ss}", item.ProcessTime);
            //                            sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 9].Value = String.Format("{0:HH:mm:ss}", item.EndProcessTime);
            //                            sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 10].Value = item.TotalTransTime;
            //                            sheet.Cells[rowIndex, 10].Style.Numberformat.Format = "HH:mm:ss";  // vi la kieu TimeSpan nen dung dinh dang
            //                            sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //                            rowIndex++;
            //                        }
            //                    }

            //                    //sheet.Cells.AutoFitColumns(5);
            //                    sheet.Column(3).Width = 20;
            //                    sheet.Column(4).Width = 30;
            //                    sheet.Column(5).Width = 15;
            //                    sheet.Column(6).Width = 15;
            //                    sheet.Column(7).Width = 15;
            //                    sheet.Column(8).Width = 15;
            //                    sheet.Column(9).Width = 20;
            //                    sheet.Column(10).Width = 20;
            //                    for (int i = 2; i <= 10; i++)
            //                    {
            //                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    }
            //                    sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            //                    SaveFileDialog dialog = new SaveFileDialog();
            //                    dialog.DefaultExt = "xlsx";
            //                    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
            //                    dialog.InitialDirectory = @"C:\";
            //                    dialog.Title = "Save File As";
            //                    if (dialog.ShowDialog() == DialogResult.OK)
            //                    {
            //                        string filename = dialog.FileName;
            //                        //if (File.Exists(filename))
            //                        //    File.Delete(filename);

            //                        //Create excel file on physical disk    
            //                        FileStream objFileStrm = File.Create(filename);
            //                        objFileStrm.Close();

            //                        //Write content to excel file    
            //                        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
            //                        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //                        {
            //                            if (File.Exists(filename))
            //                                System.Diagnostics.Process.Start(filename);
            //                            else
            //                            {
            //                                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                            }
            //                        }
            //                    }
            //                }
            //                break;
            //            case 2:

            //                if (int.Parse(lookUpSelect.EditValue.ToString()) == -1)
            //                {
            //                    MessageBox.Show("Vui lòng chọn nghiệp vụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //                else
            //                {
            //                    int majorId = int.Parse(lookUpSelect.EditValue.ToString());

            //                    var listObjs = BLLR_DetailInDay.Instance.GetDetailInDayByMajor(majorId);
            //                    var excelPackage = new ExcelPackage();
            //                    excelPackage.Workbook.Properties.Author = "GPRO";
            //                    excelPackage.Workbook.Properties.Title = "Báo Cáo Chi Tiết Trong Ngày Theo Nghiệp Vụ";
            //                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            //                    sheet.Name = "Sheet1";
            //                    sheet.Cells.Style.Font.Size = 12;
            //                    sheet.Cells.Style.Font.Name = "Times New Roman";
            //                    sheet.Cells.AutoFitColumns();
            //                    if (majorId == 0)
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA TẤT CẢ NGHIỆP VỤ";
            //                    else
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA NGHIỆP VỤ" + (listObjs.Count > 0 ? ": " + listObjs[0].MajorName.ToUpper() : string.Empty);
            //                    sheet.Cells[1, 2].Style.Font.Size = 14;
            //                    sheet.Cells[1, 2, 1, 10].Merge = true;
            //                    sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
            //                    sheet.Cells[1, 2].Style.WrapText = true;
            //                    sheet.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Row(1).Height = 25;
            //                    sheet.Row(3).Height = 20;
            //                    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Row(3).Style.Font.Bold = true;

            //                    sheet.Cells[3, 2, 3, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[3, 2, 3, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[3, 2, 3, 10].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year + "   " + "Tổng số lượt khách giao dịch: " + listObjs.Count.ToString();
            //                    //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //                    sheet.Cells[2, 2].Style.Font.Bold = true;
            //                    sheet.Cells[2, 2, 2, 10].Merge = true;
            //                    int rowIndex = 3;

            //                    sheet.Cells[rowIndex, 2].Value = "STT";
            //                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 3].Value = "Số phiếu";
            //                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 4].Value = "Nhân viên";
            //                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 5].Value = "Thiết bị";
            //                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 6].Value = "Nghiệp vụ";
            //                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 7].Value = "Giờ lấy phiếu";
            //                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 8].Value = "Giờ giao dịch";
            //                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 9].Value = "Giờ kết thúc";
            //                    sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 10].Value = "Thời gian giao dịch";
            //                    sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //                    rowIndex++;
            //                    if (listObjs.Count > 0)
            //                    {
            //                        foreach (var item in listObjs)
            //                        {
            //                            sheet.Cells[rowIndex, 2].Value = item.Index;
            //                            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 3].Value = item.TicketNumber;
            //                            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 4].Value = item.UserName;
            //                            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 5].Value = item.CounterName;
            //                            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 6].Value = item.MajorName;
            //                            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 6].Style.Font.Color.SetColor(Color.Red);
            //                            sheet.Cells[rowIndex, 7].Value = String.Format("{0:HH:mm:ss}", item.PrintTime);
            //                            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 8].Value = String.Format("{0:HH:mm:ss}", item.ProcessTime);
            //                            sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 9].Value = String.Format("{0:HH:mm:ss}", item.EndProcessTime);
            //                            sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 10].Value = item.TotalTransTime;
            //                            sheet.Cells[rowIndex, 10].Style.Numberformat.Format = "HH:mm:ss";  // vi la kieu TimeSpan nen dung dinh dang
            //                            sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //                            rowIndex++;
            //                        }
            //                    }

            //                    //sheet.Cells.AutoFitColumns(5);
            //                    sheet.Column(3).Width = 20;
            //                    sheet.Column(4).Width = 30;
            //                    sheet.Column(5).Width = 15;
            //                    sheet.Column(6).Width = 15;
            //                    sheet.Column(7).Width = 15;
            //                    sheet.Column(8).Width = 15;
            //                    sheet.Column(9).Width = 20;
            //                    sheet.Column(10).Width = 20;
            //                    for (int i = 2; i <= 10; i++)
            //                    {
            //                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    }
            //                    sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            //                    SaveFileDialog dialog = new SaveFileDialog();
            //                    dialog.DefaultExt = "xlsx";
            //                    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
            //                    dialog.InitialDirectory = @"C:\";
            //                    dialog.Title = "Save File As";
            //                    if (dialog.ShowDialog() == DialogResult.OK)
            //                    {
            //                        string filename = dialog.FileName;
            //                        //if (File.Exists(filename))
            //                        //    File.Delete(filename);

            //                        //Create excel file on physical disk    
            //                        FileStream objFileStrm = File.Create(filename);
            //                        objFileStrm.Close();

            //                        //Write content to excel file    
            //                        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
            //                        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //                        {
            //                            if (File.Exists(filename))
            //                                System.Diagnostics.Process.Start(filename);
            //                            else
            //                            {
            //                                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                            }
            //                        }
            //                    }
            //                }
            //                break;
            //            case 3:
            //                if (int.Parse(lookUpSelect.EditValue.ToString()) == -1)
            //                {
            //                    MessageBox.Show("Vui lòng chọn dịch vụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //                else
            //                {
            //                    int serviceId = int.Parse(lookUpSelect.EditValue.ToString());

            //                    var listObjs = BLLR_DetailInDay.Instance.GetDetailInDayByService(serviceId);
            //                    var excelPackage = new ExcelPackage();
            //                    excelPackage.Workbook.Properties.Author = "GPRO";
            //                    excelPackage.Workbook.Properties.Title = "Báo Cáo Chi Tiết Trong Ngày Theo Dịch Vụ";
            //                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            //                    sheet.Name = "Sheet1";
            //                    sheet.Cells.Style.Font.Size = 12;
            //                    sheet.Cells.Style.Font.Name = "Times New Roman";
            //                    sheet.Cells.AutoFitColumns();
            //                    if (serviceId == 0)
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA TẤT CẢ DỊCH VỤ";
            //                    else
            //                        sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT TRONG NGÀY CỦA DỊCH VỤ" + (listObjs.Count > 0 ? ": " + listObjs[0].ServiceName.ToUpper() : string.Empty);
            //                    sheet.Cells[1, 2].Style.Font.Size = 14;
            //                    sheet.Cells[1, 2, 1, 6].Merge = true;
            //                    sheet.Cells[1, 2, 1, 6].Style.Font.Bold = true;
            //                    sheet.Cells[1, 2].Style.WrapText = true;
            //                    sheet.Cells[1, 2, 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Cells[1, 2, 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[1, 2, 1, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[1, 2, 1, 6].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Row(1).Height = 25;
            //                    sheet.Row(3).Height = 20;
            //                    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //                    sheet.Row(3).Style.Font.Bold = true;

            //                    sheet.Cells[3, 2, 3, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //                    sheet.Cells[3, 2, 3, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
            //                    sheet.Cells[3, 2, 3, 6].Style.Font.Color.SetColor(Color.White);

            //                    sheet.Cells[2, 2].Value = "Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year + "   " + "Số yêu cầu: " + listObjs.Count.ToString();
            //                    //sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //                    sheet.Cells[2, 2].Style.Font.Bold = true;
            //                    sheet.Cells[2, 2, 2, 6].Merge = true;
            //                    int rowIndex = 3;

            //                    sheet.Cells[rowIndex, 2].Value = "STT";
            //                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 3].Value = "Số phiếu";
            //                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 4].Value = "Dịch vụ";
            //                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 5].Value = "Giờ lấy phiếu";
            //                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                    sheet.Cells[rowIndex, 6].Value = "Trạng thái";
            //                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //                    rowIndex++;
            //                    if (listObjs.Count > 0)
            //                    {
            //                        foreach (var item in listObjs)
            //                        {
            //                            sheet.Cells[rowIndex, 2].Value = item.Index;
            //                            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                            sheet.Cells[rowIndex, 3].Value = item.TicketNumber;
            //                            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 4].Value = item.ServiceName;
            //                            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
            //                            sheet.Cells[rowIndex, 5].Value = String.Format("{0:HH:mm:ss}", item.PrintTime);
            //                            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            sheet.Cells[rowIndex, 6].Value = item.StatusCode;
            //                            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //                            rowIndex++;
            //                        }
            //                    }

            //                    //sheet.Cells.AutoFitColumns(5);
            //                    sheet.Column(2).Width = 10;
            //                    sheet.Column(3).Width = 20;
            //                    sheet.Column(4).Width = 20;
            //                    sheet.Column(5).Width = 20;
            //                    sheet.Column(6).Width = 20;

            //                    for (int i = 2; i <= 6; i++)
            //                    {
            //                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //                    }
            //                    sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            //                    SaveFileDialog dialog = new SaveFileDialog();
            //                    dialog.DefaultExt = "xlsx";
            //                    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
            //                    dialog.InitialDirectory = @"C:\";
            //                    dialog.Title = "Save File As";
            //                    if (dialog.ShowDialog() == DialogResult.OK)
            //                    {
            //                        string filename = dialog.FileName;
            //                        //if (File.Exists(filename))
            //                        //    File.Delete(filename);

            //                        //Create excel file on physical disk    
            //                        FileStream objFileStrm = File.Create(filename);
            //                        objFileStrm.Close();

            //                        //Write content to excel file    
            //                        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
            //                        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //                        {
            //                            if (File.Exists(filename))
            //                                System.Diagnostics.Process.Start(filename);
            //                            else
            //                            {
            //                                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                            }
            //                        }
            //                    }
            //                }
            //                break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //    MessageBox.Show("Lỗi: " + ex.Message);
            //}
        }

        private void GetUser()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLUser.Instance.GetLookUp(connect);
            foreach (var item in listObj)
            {
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });
            }

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Nhân viên";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.Properties.NullText = string.Empty;
            lookUpSelect.EditValue = -1;
        }

        private void GetMajor()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLMajor.Instance.GetLookUp(connect);
            foreach (var item in listObj)
            {
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });
            }

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Nghiệp vụ";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.Properties.NullText = string.Empty;
            lookUpSelect.EditValue = -1;

        }

        private void GetService()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLService.Instance.GetLookUp(connect);
            foreach (var item in listObj)
            {
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });
            }

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Dịch vụ";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.Properties.NullText = string.Empty;
            lookUpSelect.EditValue = -1;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
            gridNV.Visible = false;
            gridNghiepVu.Visible = false;
            gridDV.Visible = false;
            switch (value)
            {
                case 1:
                    GetUser();
                    gridNV.Visible = true;
                    lblCurrentDate.Text = string.Empty;
                    lblTotalTransaction.Text = string.Empty;
                    break;
                case 2:
                    GetMajor();
                    gridNghiepVu.Visible = true;
                    lblCurrentDate.Text = string.Empty;
                    lblTotalTransaction.Text = string.Empty;
                    break;
                case 3:
                    GetService();
                    gridDV.Visible = true;
                    lblCurrentDate.Text = string.Empty;
                    lblTotalTransaction.Text = string.Empty;
                    break;
            }
        }
    }
}
