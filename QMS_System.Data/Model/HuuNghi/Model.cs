using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model.HuuNghi
{
    public class khoaCls
    {
        public List<int> dichvu_Ids { get; set; }
        public int Khoa_Id { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public List<phongkhamCls> dsPhongKham { get; set; }
        public khoaCls()
        {
            dichvu_Ids = new List<int>();
            dsPhongKham = new List<phongkhamCls>();
        }
    }
    public class dichvuCls
    {
        public List<int> khoa_Ids { get; set; }
        public int DichVu_Id { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public dichvuCls()
        {
            khoa_Ids = new List<int>();
        }
    }
    public class phongkhamCls
    {
        public List<int> dichvu_Ids { get; set; }
        public int PhongKham_Id { get; set; }
        public string MaPhongKham { get; set; }
        public string TenPhongKham { get; set; }
        public List<dichvuCls> dsDichVu { get; set; }
        public phongkhamCls()
        {
            dichvu_Ids = new List<int>();
            dsDichVu = new List<dichvuCls>();
        }
    }

    public class KhoaModel
    {
        public int Id { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public List<DichVuModel> DichVus { get; set; }
        public KhoaModel()
        {
            DichVus = new List<DichVuModel>();
        }
    }

    public class DichVuModel
    {
        public int Id { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public List<PhongKhamModel> PhongKhams { get; set; }
        public DichVuModel()
        {
            PhongKhams = new List<PhongKhamModel>();
        }
    }

    public class PhongKhamModel
    {
        public int Id { get; set; }
        public string MaPK { get; set; }
        public string TenPK { get; set; }
    }
}
