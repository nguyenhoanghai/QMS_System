﻿using QMS_System.Data.BLL;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Threading;

namespace QMS_System
{
    public partial class frmR_GeneralByTimeRange : Form
    {
        List<R_GeneralInDayModel> list;
        Thread thread;
        bool isSearch = true;
        bool danglamexcell = false;
        public frmR_GeneralByTimeRange()
        {
            InitializeComponent();
        }
        private void frmR_GeneralByTimeRange_Load(object sender, EventArgs e)
        {
            dtFromDate.EditValue = DateTime.Now;
            dtToDate.EditValue = DateTime.Now;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            thread = new Thread(LoadData);
            thread.Start();
            timer1.Enabled = true;
            isSearch = true;
            btnExportToExcel.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void GetUser()
        {
            var list = new List<ModelSelectItem>();
            list.Add(new ModelSelectItem() { Id = 0, Name = "Chọn tất cả" });
            var listObj = BLLUser.Instance.GetLookUp();
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
            var listObj = BLLMajor.Instance.GetLookUp();
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
            var listObj = BLLService.Instance.GetLookUp();
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
            btnSearch_Click(sender, e);
          //  int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
            //switch (value)
            //{
            //    case 1: GetUser(); break;
            //    case 2: GetMajor(); break;
            //    case 3: GetService(); break;
            //}
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
          //  thread = new Thread(LoadData);
          //  thread.Start();
          //  timer1.Enabled = true;
         //   isSearch = false;
            btnExportToExcel.Enabled = false;
            btnSearch.Enabled = false;


            int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
            SaveFileDialog vrow = new SaveFileDialog();
            vrow.Filter = "excel files (*.xlsx)|*.xlsx ";
            if (vrow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = vrow.FileName;
                gridGeneralByTimeRange.ExportToXlsx(path);
                if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    if (File.Exists(path))
                        System.Diagnostics.Process.Start(path);
                    else
                        MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            }
           // timer1.Enabled = true;
           // isSearch = false;
            btnExportToExcel.Enabled = true;
            btnSearch.Enabled = true;
        }

        private void LoadData()
        {
            try
            {
                int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                list = BLLReport.Instance.GeneralReport(((string.IsNullOrEmpty(lookUpSelect.EditValue.ToString()) || lookUpSelect.EditValue.ToString() == "-1") ? 0 : int.Parse(lookUpSelect.EditValue.ToString())), value, DateTime.Parse(dtFromDate.EditValue.ToString()), DateTime.Parse(dtToDate.EditValue.ToString()));

            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (list != null)
            {
                if (isSearch)
                    ShowGrid();
                else
                    if (!danglamexcell)
                        Excel();
                timer1.Enabled = false;
                btnSearch.Enabled = true;
                btnExportToExcel.Enabled = true;
            }
        }

        private void ShowGrid()
        {
            gridGeneralByTimeRange.DataSource = null;
            gridGeneralByTimeRange.DataSource = list;
        }
        private void Excel()
        {
            try
            {
                danglamexcell = true;
                //  int value = int.Parse(radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString());
                //   var listObjs = BLLReport.Instance.GeneralReport(((string.IsNullOrEmpty(lookUpSelect.EditValue.ToString()) || lookUpSelect.EditValue.ToString() == "-1") ? 0 : int.Parse(lookUpSelect.EditValue.ToString())), value, DateTime.Parse(dtFromDate.EditValue.ToString()), DateTime.Parse(dtToDate.EditValue.ToString()));

                var excelPackage = new ExcelPackage();
                excelPackage.Workbook.Properties.Author = "GPRO";
                excelPackage.Workbook.Properties.Title = "Báo Cáo Tổng Hợp Trong Ngày Theo " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString();
                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                sheet.Name = "Sheet1";
                sheet.Cells.Style.Font.Size = 12;
                sheet.Cells.Style.Font.Name = "Times New Roman";
                sheet.Cells.AutoFitColumns();
                sheet.Cells[1, 2].Value = "BÁO CÁO TỔNG HỢP TRONG NGÀY CỦA " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString().ToUpper();
                sheet.Cells[1, 2].Style.Font.Size = 14;
                sheet.Cells[1, 2, 1, 6].Merge = true;
                sheet.Cells[1, 2, 1, 6].Style.Font.Bold = true;
                sheet.Cells[1, 2].Style.WrapText = true;
                sheet.Cells[1, 2, 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 2, 1, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[1, 2, 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[1, 2, 1, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[1, 2, 1, 6].Style.Font.Color.SetColor(Color.White);

                sheet.Row(1).Height = 25;
                sheet.Row(3).Height = 20;
                sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Row(3).Style.Font.Bold = true;

                sheet.Cells[3, 2, 3, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[3, 2, 3, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 149, 221));
                sheet.Cells[3, 2, 3, 6].Style.Font.Color.SetColor(Color.White);
                sheet.Cells[2, 2].Style.Font.Bold = true;
                sheet.Cells[2, 2, 2, 6].Merge = true;
                int rowIndex = 3;

                sheet.Cells[rowIndex, 2].Value = "STT";
                sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[rowIndex, 3].Value = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].ToString();
                sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[rowIndex, 4].Value = "Số lần giao dịch";
                sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[rowIndex, 5].Value = "Tổng thời gian giao dịch (phút)";
                sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[rowIndex, 6].Value = "Thời gian giao dịch trung bình(phút/gd)";
                sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                rowIndex++;

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        sheet.Cells[rowIndex, 2].Value = item.Index;
                        sheet.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 3].Value = item.Name;
                        sheet.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 4].Value = item.TotalTransaction;
                        sheet.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 5].Value = item.TotalTransTime;
                        sheet.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[rowIndex, 6].Value = item.AverageTimePerTrans;
                        sheet.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        rowIndex++;
                    }
                }

                //sheet.Cells.AutoFitColumns(5);
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 40;
                for (int i = 2; i <= 6; i++)
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

                    //Create excel file on physical disk    
                    FileStream objFileStrm = File.Create(filename);
                    objFileStrm.Close();

                    //Write content to excel file    
                    File.WriteAllBytes(filename, excelPackage.GetAsByteArray());
                    if (MessageBox.Show("Bạn có muốn mở file này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (File.Exists(filename))
                            System.Diagnostics.Process.Start(filename);
                        else
                            MessageBox.Show("File không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            danglamexcell = false;
            thread.Abort();
        }
    }
}
