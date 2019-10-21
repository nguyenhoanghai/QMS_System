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
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using QMS_System.Helper;
using GPRO.Core.Hai;

namespace QMS_System
{
    public partial class frmR_DetailByTimeRange : Form
    {
        string connect = BaseCore.Instance.GetEntityConnectString(Application.StartupPath + "\\DATA.XML");
        Thread loadDataThread;
        List<ReportModel> list;
        bool isShow = false;
        public frmR_DetailByTimeRange()
        {
            InitializeComponent();
        }

        private void frmR_DetailByTimeRange_Load(object sender, EventArgs e)
        {
            dtFromDate.EditValue = DateTime.Now;
            dtToDate.EditValue = DateTime.Now;
            gridNV.Dock = DockStyle.Fill;
            gridNghiepVu.Dock = DockStyle.Fill;
            gridDV.Dock = DockStyle.Fill;
            lookUpSelect.Properties.NullText = string.Empty;
            radioGroup1.SelectedIndex = 0;
            radioGroup1_SelectedIndexChanged(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadDataThread = new Thread(LoadData);
            loadDataThread.Start();
            btnExportToExcel.Enabled = false;
            btnSearch.Enabled = false;
            timer1.Enabled = true;
        }

        private void GetUser()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLUser.Instance.GetLookUp(connect);
            foreach (var item in listObj)
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Nhân viên";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.EditValue = 0;
        }

        private void GetMajor()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLMajor.Instance.GetLookUp(connect);
            foreach (var item in listObj)
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Nghiệp vụ";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.EditValue = 0;

        }

        private void GetService()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLService.Instance.GetLookUp(connect);
            foreach (var item in listObj)
                list.Add(new ModelSelectItem() { Id = item.Id, Name = item.Name });

            lookUpSelect.Properties.DataSource = null;
            lookUpSelect.Properties.DataSource = list;
            lookUpSelect.Properties.DisplayMember = "Name";
            lookUpSelect.Properties.ValueMember = "Id";
            lookUpSelect.Properties.PopulateViewColumns();
            lookUpSelect.Properties.View.Columns[0].Visible = false;
            lookUpSelect.Properties.View.Columns[1].Caption = "Dịch vụ";
            lookUpSelect.Properties.View.Columns[2].Visible = false;
            lookUpSelect.Properties.View.Columns[3].Visible = false;
            lookUpSelect.EditValue = 0;
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
                    break;
                case 2:
                    GetMajor();
                    gridNghiepVu.Visible = true;
                    break;
                case 3:
                    GetService();
                    gridDV.Visible = true;
                    break;
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                // var listObjs = BLLReport.Instance.DetailReport(((string.IsNullOrEmpty(lookUpSelect.EditValue.ToString()) || lookUpSelect.EditValue.ToString() == "-1") ? 0 : int.Parse(lookUpSelect.EditValue.ToString())), value, DateTime.Parse(dtFromDate.EditValue.ToString()), DateTime.Parse(dtToDate.EditValue.ToString()));

                //string templatePath = Application.StartupPath + @"\ReportTemplate\";
                //switch (value)
                //{
                //    case 1: templatePath += "fromToNhanVien.xlsx";
                //        if (!File.Exists(templatePath))
                //            MessageBox.Show("Không tìm thấy file mail template.");
                //        else
                //            ExportUser( templatePath);
                //        break;
                //    case 2: templatePath += "fromToNghiepVu.xlsx";
                //        if (!File.Exists(templatePath))
                //            MessageBox.Show("Không tìm thấy file mail template.");
                //        else
                //            ExportMajor( templatePath);
                //        break;
                //    case 3: templatePath += "fromToDichVu.xlsx";
                //        if (!File.Exists(templatePath))
                //            MessageBox.Show("Không tìm thấy file mail template.");
                //        else
                //            ExportService( templatePath);
                //        break;
                //}


                SaveFileDialog vrow = new SaveFileDialog();
                vrow.Filter = "excel files (*.xlsx)|*.xlsx ";
                if (vrow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = vrow.FileName;
                    switch (value)
                    {
                        case 1:
                            {
                                gridNV.ExportToXlsx(path);
                                break;
                            }

                        case 2:
                            {
                                gridNghiepVu.ExportToXlsx(path);
                                break;
                            }

                        case 3:
                            {
                                gridDV.ExportToXlsx(path);
                                break;
                            }
                    }
                    if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (File.Exists(path))
                            System.Diagnostics.Process.Start(path);
                        else
                            MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }








                //    var excelPackage = new ExcelPackage();
                //    excelPackage.Workbook.Properties.Author = "GPRO";
                //    excelPackage.Workbook.Properties.Title = "Báo Cáo Chi Tiết Theo Khoảng Thời Gian Của " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString();
                //    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                //    sheet.Name = "Sheet1";
                //    sheet.Cells.Style.Font.Size = 12;
                //    sheet.Cells.Style.Font.Name = "Times New Roman";
                //    sheet.Cells.AutoFitColumns();
                //    sheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT THEO THỜI GIAN CỦA " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString().ToUpper();
                //    sheet.Cells[1, 2].Style.Font.Size = 14;
                //    sheet.Cells[1, 2, 1, 10].Merge = true;
                //    sheet.Cells[1, 2, 1, 10].Style.Font.Bold = true;
                //    sheet.Cells[1, 2].Style.WrapText = true;
                //    sheet.Cells[1, 2, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    sheet.Cells[1, 2, 1, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    sheet.Cells[1, 2, 1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    sheet.Cells[1, 2, 1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                //    sheet.Cells[1, 2, 1, 10].Style.Font.Color.SetColor(Color.White);

                //    sheet.Row(1).Height = 25;
                //    sheet.Row(3).Height = 20;
                //    sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    sheet.Row(3).Style.Font.Bold = true;

                //    sheet.Cells[3, 2, 3, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    sheet.Cells[3, 2, 3, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                //    sheet.Cells[3, 2, 3, 10].Style.Font.Color.SetColor(Color.White);

                //    if (dtFromDate.EditValue == null && dtToDate.EditValue != null)
                //        sheet.Cells[2, 2].Value = "Từ  ?  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + listObjs.Count.ToString();
                //    else if (dtFromDate.EditValue != null && dtToDate.EditValue == null)
                //        sheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  ? " + "    Tổng số lượt khách giao dịch: " + listObjs.Count.ToString();
                //    else
                //        sheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + listObjs.Count.ToString();


                //    sheet.Cells[2, 2].Style.Font.Bold = true;
                //    sheet.Cells[2, 2, 2, 10].Merge = true;
                //    int rowIndex = 3;

                //    switch (value)
                //    {
                //        case 1:
                //        case 2:
                //            #region MyRegion
                //            sheet.Cells[rowIndex, 2].Value = "STT";
                //            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 3].Value = "Số phiếu";
                //            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 4].Value = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString();
                //            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 5].Value = "Thiết bị";
                //            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 6].Value = "Nghiệp vụ";
                //            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 7].Value = "Giờ lấy phiếu";
                //            sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 8].Value = "Giờ giao dịch";
                //            sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 9].Value = "Giờ kết thúc";
                //            sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 10].Value = "Thời gian giao dịch";
                //            sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //            rowIndex++;
                //            if (listObjs.Count > 0)
                //            {
                //                foreach (var item in listObjs)
                //                {
                //                    sheet.Cells[rowIndex, 2].Value = item.stt;
                //                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 3].Value = item.Number;
                //                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 4].Value = item.UserName;
                //                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
                //                    sheet.Cells[rowIndex, 5].Value = "";
                //                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 6].Value = item.MajorName;
                //                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 7].Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.PrintTime);
                //                    sheet.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 8].Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.ProcessTime);
                //                    sheet.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 9].Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.End);
                //                    sheet.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 10].Value = item.ProcessTime;
                //                    sheet.Cells[rowIndex, 10].Style.Numberformat.Format = "HH:mm:ss";  // vi la kieu TimeSpan nen dung dinh dang
                //                    sheet.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //                    rowIndex++;
                //                }
                //            }

                //            //sheet.Cells.AutoFitColumns(5);
                //            sheet.Column(3).Width = 20;
                //            sheet.Column(4).Width = 30;
                //            sheet.Column(5).Width = 20;
                //            sheet.Column(6).Width = 20;
                //            sheet.Column(7).Width = 25;
                //            sheet.Column(8).Width = 25;
                //            sheet.Column(9).Width = 25;
                //            sheet.Column(10).Width = 20;

                //            for (int i = 2; i <= 10; i++)
                //                sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //            sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                //            #endregion
                //            break;
                //        case 3:
                //            #region MyRegion
                //            sheet.Cells[rowIndex, 2].Value = "STT";
                //            sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 3].Value = "Số phiếu";
                //            sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 4].Value = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString();
                //            sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 5].Value = "Giờ lấy phiếu";
                //            sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //            sheet.Cells[rowIndex, 6].Value = "Trạng thái";
                //            sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //            rowIndex++;
                //            if (listObjs.Count > 0)
                //            {
                //                foreach (var item in listObjs)
                //                {
                //                    sheet.Cells[rowIndex, 2].Value = item.stt;
                //                    sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //                    sheet.Cells[rowIndex, 3].Value = item.Number;
                //                    sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 4].Value = item.ServiceName;
                //                    sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 4].Style.Font.Color.SetColor(Color.Red);
                //                    sheet.Cells[rowIndex, 5].Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.PrintTime);
                //                    sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    sheet.Cells[rowIndex, 6].Value = item.StatusName;
                //                    sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //                    rowIndex++;
                //                }
                //            }

                //            //sheet.Cells.AutoFitColumns(5);
                //            sheet.Column(2).Width = 10;
                //            sheet.Column(3).Width = 20;
                //            sheet.Column(4).Width = 20;
                //            sheet.Column(5).Width = 25;
                //            sheet.Column(6).Width = 20;

                //            for (int i = 2; i <= 6; i++)
                //                sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //            sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                //            #endregion
                //            break;
                //    }
                //    SaveFileDialog dialog = new SaveFileDialog();
                //    dialog.DefaultExt = "xlsx";
                //    dialog.Filter = "Excel Files (*.xlsx) | *.xlsx |Excel Files (*.xls) | *.xls";
                //    dialog.InitialDirectory = @"C:\";
                //    dialog.Title = "Save File As";
                //    if (dialog.ShowDialog() == DialogResult.OK)
                //    {
                //        string filename = dialog.FileName;

                //        //Create excel file on physical disk    
                //        FileStream objFileStrm = File.Create(filename);
                //        objFileStrm.Close();

                //        //Write content to excel file    
                //        File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                //        if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        {
                //            if (File.Exists(filename))
                //                System.Diagnostics.Process.Start(filename);
                //            else
                //            {
                //                MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            }
                //        }
                //    }

            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ExportService(string templatePath)
        {
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;
                xlSheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT THEO THỜI GIAN CỦA " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString().ToUpper();
                if (dtFromDate.EditValue == null && dtToDate.EditValue != null)
                    xlSheet.Cells[2, 2].Value = "Từ  ?  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else if (dtFromDate.EditValue != null && dtToDate.EditValue == null)
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  ? " + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                if (list.Count > 0)
                {
                    int rowIndex = 4;
                    foreach (var item in list)
                    {
                        for (int i = 2; i < 7; i++)
                        {
                            oRng = xlSheet.get_Range(ConvertChar(64 + i) + rowIndex);
                            switch (i)
                            {
                                case 2: oRng.Value = item.stt; break;
                                case 3: oRng.Value = item.Number; break;
                                case 4: oRng.Value = item.ServiceName; break;
                                case 5: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.PrintTime); break;
                                case 6: oRng.Value = item.StatusName; break;
                            }
                            SetBorder_TextAlign(oRng, false);
                        }
                        rowIndex++;
                    }
                }
                //save file
                string path = (Application.StartupPath + "\\ReportFolder\\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filename = path + "ThongKeChiTiet_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx";
                xlBook.SaveAs(filename, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                MessageBox.Show("Tạo file excel thành công.");
                #endregion
                btnExportToExcel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file excel bị lỗi.\n" + ex.Message);
            }
        }

        private static string ConvertChar(int number)
        {
            var cha = number > 90 ? "A" : "";
            number = number > 90 ? (number - 26) : number;
            cha += Convert.ToChar(number).ToString();
            return cha;
        }
        private static void SetBorder_TextAlign(Excel.Range oRng, bool fontWeight)
        {
            oRng.Select();
            oRng.Merge();
            oRng.Interior.ColorIndex = 2;
            oRng.Borders.ColorIndex = 56;
            oRng.Font.Name = "Times New Roman";
            oRng.Font.Bold = fontWeight;
            oRng.Font.Size = 10;
            oRng.HorizontalAlignment = Excel.Constants.xlCenter;
            oRng.VerticalAlignment = Excel.Constants.xlCenter;
            oRng.WrapText = true;
        }
        static public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void ExportMajor(string templatePath)
        {
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;
                xlSheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT THEO THỜI GIAN CỦA " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString().ToUpper();
                if (dtFromDate.EditValue == null && dtToDate.EditValue != null)
                    xlSheet.Cells[2, 2].Value = "Từ  ?  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else if (dtFromDate.EditValue != null && dtToDate.EditValue == null)
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  ? " + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                if (list.Count > 0)
                {
                    int rowIndex = 4;
                    foreach (var item in list)
                    {
                        for (int i = 2; i < 11; i++)
                        {
                            oRng = xlSheet.get_Range(ConvertChar(64 + i) + rowIndex);
                            switch (i)
                            {
                                case 2: oRng.Value = item.stt; break;
                                case 3: oRng.Value = item.Number; break;
                                case 4: oRng.Value = item.UserName; break;
                                case 5: oRng.Value = item.MajorName; break;
                                case 6: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.PrintTime); break;
                                case 7: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Start); break;
                                case 8: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.End); break;
                                case 9: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.ProcessTime); break;
                                case 10: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.WaitingTime); break;

                            }
                            SetBorder_TextAlign(oRng, false);
                        }
                        rowIndex++;
                    }
                }
                //save file
                string path = (Application.StartupPath + "\\ReportFolder\\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filename = path + "ThongKeChiTiet_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx"; xlBook.SaveAs(filename, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                MessageBox.Show("Tạo file excel thành công.");
                #endregion
                btnExportToExcel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file excel bị lỗi.\n" + ex.Message);
            }
        }

        private void ExportUser(string templatePath)
        {
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;
                xlSheet.Cells[1, 2].Value = "BÁO CÁO CHI TIẾT THEO THỜI GIAN CỦA " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString().ToUpper();
                if (dtFromDate.EditValue == null && dtToDate.EditValue != null)
                    xlSheet.Cells[2, 2].Value = "Từ  ?  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else if (dtFromDate.EditValue != null && dtToDate.EditValue == null)
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  ? " + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                else
                    xlSheet.Cells[2, 2].Value = "Từ  " + Convert.ToDateTime(dtFromDate.EditValue).ToString("dd/MM/yyyy") + "  Đến  " + Convert.ToDateTime(dtToDate.EditValue).ToString("dd/MM/yyyy") + "    Tổng số lượt khách giao dịch: " + list.Count.ToString();
                if (list.Count > 0)
                {
                    int rowIndex = 4;
                    foreach (var item in list)
                    {
                        for (int i = 2; i < 11; i++)
                        {
                            oRng = xlSheet.get_Range(ConvertChar(64 + i) + rowIndex);
                            switch (i)
                            {
                                case 2: oRng.Value = item.stt; break;
                                case 3: oRng.Value = item.Number; break;
                                case 4: oRng.Value = item.UserName; break;
                                case 5: oRng.Value = item.MajorName; break;
                                case 6: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.PrintTime); break;
                                case 7: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Start); break;
                                case 8: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.End); break;
                                case 9: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.ProcessTime); break;
                                case 10: oRng.Value = String.Format("{0:dd/MM/yyyy HH:mm:ss}", item.WaitingTime); break;

                            }
                            SetBorder_TextAlign(oRng, false);
                        }
                        rowIndex++;
                    }
                }
                //save file
                string path = (Application.StartupPath + "\\ReportFolder\\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filename = path + "ThongKeChiTiet_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".xlsx"; xlBook.SaveAs(filename, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                MessageBox.Show("Tạo file excel thành công.");
                #endregion
                btnExportToExcel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file excel bị lỗi.\n" + ex.Message);
            }
        }

        public void LoadData()
        {
            try
            {
                int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                list = BLLReport.Instance.DetailReport(connect,((string.IsNullOrEmpty(lookUpSelect.EditValue.ToString()) || lookUpSelect.EditValue.ToString() == "-1") ? 0 : int.Parse(lookUpSelect.EditValue.ToString())), value, DateTime.Parse(dtFromDate.EditValue.ToString()), DateTime.Parse(dtToDate.EditValue.ToString()));
                isShow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            loadDataThread.Abort();
        }

        public void ExportExcel()
        {

        }

        public void ShowGrid()
        {
            int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
            switch (value)
            {
                case 1:
                    {
                        gridNV.DataSource = null;
                        gridNV.DataSource = list;
                        break;
                    }
                case 2:
                    {
                        gridNghiepVu.DataSource = null;
                        gridNghiepVu.DataSource = list;
                        break;
                    }

                case 3:
                    {
                        gridDV.DataSource = null;
                        gridDV.DataSource = list;
                        break;
                    }
            }
            timer1.Enabled = false;
            btnSearch.Enabled = true;
            btnExportToExcel.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (list != null && !isShow)
            {
                ShowGrid();
                isShow = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string path = (Application.StartupPath + "\\ReportFolder\\");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Process.Start(path);
        }
    }
}
