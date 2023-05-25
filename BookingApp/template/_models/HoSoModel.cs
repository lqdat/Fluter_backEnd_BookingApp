using System;
using System.Collections.Generic;

namespace BookingApp.Controllers.Report
{
    public class HoSoModel
    {
        public string MaHoSo { get; set; }
        public string TenHoSo { get; set; }
        public DateTime TGTao { get; set; }
        public string MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public string SoCongVan { get; set; }
        public List<TramBTSModel> ListTramBTS { get; set; }
        public List<TuyenCapNgamModel> ListTuyenCapNgam { get; set; }
        public List<TuyenCapNoiModel> ListTuyenCapNoi { get; set; }
        public List<DiemDichVuModel> ListDiemDichVu { get; set; }
    }

    public class TramBTSModel
    {
        public string MaTram { get; set; }
        public string TenTram { get; set; }
        public decimal? ToaDoX { get; set; }
        public decimal? ToaDoY { get; set; }
        public string DiaChi { get; set; }
        public int? TrangThai { get; set; }
        public string LyDo { get; set; }
    }

    public class TuyenCapNoiModel
    {
        public int? TrangThai { get; set; }
        public string LyDo { get; set; }
        public string MaTuyen { get; set; }
        public string TenTuyen { get; set; }
        public string DiemDau { get; set; }
        public string DiemCuoi { get; set; }
        public decimal? ChieuDaiThucTe { get; set; }
    }

    public class TuyenCapNgamModel
    {
        public int? TrangThai { get; set; }
        public string LyDo { get; set; }
        public string MaTuyen { get; set; }
        public string TenTuyen { get; set; }
        public string DiemDau { get; set; }
        public string DiemCuoi { get; set; }
        public decimal? ChieuDaiThucTe { get; set; }
    }

    public class DiemDichVuModel
    {
        public string SoHieu { get; set; }
        public string TenDiem { get; set; }
        public decimal? ToaDoX { get; set; }
        public decimal? ToaDoY { get; set; }
        public int? TrangThai { get; set; }
        public string LyDo { get; set; }
    }
}