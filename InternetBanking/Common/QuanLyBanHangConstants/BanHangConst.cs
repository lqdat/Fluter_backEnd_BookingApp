namespace BookingApp.Common.QuanLyBanHangConstants
{
    public static class BanHangConst
    {
        /// <summary>
        /// Thẻ kho _ loại và hình thức.
        /// </summary>
        public const string BH_TRANGTHAI_MOI = "don-moi";
        public const string BH_TRANGTHAI_HOANTHANH = "hoan-thanh";
        public const string BH_TRANGTHAI_HUY = "da-huy";
        public const string BH_MAKHACHLE = "khach-le";

        public const string TK_LOAI_NHAP = "PN";
        public const string TK_LOAI_XUAT = "PX";
        public const string TK_LOAI_DONHANG = "DH";
        public const string TK_LOAI_XUATHUY = "XH";

        public const string TK_HINHTHUC_NCC = "Nhập hàng";
        public const string TK_HINHTHUC_DONHANG = "Bán hàng";
        public const string TK_HINHTHUC_TRAHANG = "Trả hàng";
        public const string TK_HINHTHUC_HUYTRAHANG = "Hủy đơn trả hàng";
        public const string TK_HINHTHUC_CHUYENKHO = "Chuyển kho";
        public const string TK_HINHTHUC_KIEMKHO = "Kiểm kho";
        public const string TK_HINHTHUC_XUATHUY = "Xuất hủy";
        public const string TK_HINHTHUC_XUATKHUYENMAI = "Xuất hàng khuyến mãi";
        public const string TK_HINHTHUC_NHAPKHUYENMAI = "Nhập hàng khuyến mãi";

        /// <summary>
        /// Kho_ loại
        /// </summary>
        public const string K_LOAI_NCC = "NCC";
        public const string K_LOAI_BANHANG = "ban-hang";
        public const string K_LOAI_CHUYENKHO = "chuyen-kho";
        public const string K_LOAI_TRA = "tra";
        public const string K_LOAI_KIEMKHO = "kiem-kho";

        /// <summary>
        /// Chuyển kho _ trạng thái
        /// </summary>
        public const string CK_TRANGTHAI_YEUCAUMOI = "yeu_cau_moi";
        public const string CK_TRANGTHAI_DAXUATKHO = "da_xuat_kho";
        public const string CK_TRANGTHAI_TUCHOI = "tu_choi";
        public const string CK_TRANGTHAI_DANHAPKHO = "da_nhap_kho";

        /// <summary>
        /// Kiểm kho _ trạng thái.
        /// </summary>
        public const string KK_TRANGTHAI_HOANTHANH = "hoan_thanh";
        public const string KK_TRANGTHAI_LUUTAM = "luu_tam";
        public const string KK_TRANGTHAI_DAHUY = "da-huy";

        /// <summary>
        /// Mã chúng từ
        /// </summary>
        public const string MCT_XUATHANG = "PX";
        public const string MCT_NHAPHANG = "PN";
        public const string MCT_CHUYENKHO = "CK";
        public const string MCT_BANHANG = "BH";
        public const string MCT_PHIEUTHU = "PT";
        public const string MCT_TRAHANG = "TH";
        public const string MCT_PHIEUCHI = "PC";
        public const string MCT_KIEMKHO = "KK";
        public const string MCT_MUAHANG = "MH";
        public const string MCT_DIEUCHINH = "DC";
        public const string MCT_KHUYENMAI = "KM";
        public const string MCT_XUATHUY = "XH";
        public const string MCT_HOPDONG = "HD";

        public const string SP_LOAI_SANPHAM = "SanPham";
        public const string SP_LOAI_DICHVU = "DichVu";
        public const string SP_LOAI_THANHPHAM = "ThanhPham";

        /*
         * loại Công nợ
         */
        public const string CN_LOAI_BANHANG = "Bán hàng";
        public const string CN_LOAI_MUAHANG = "Mua hàng";
        public const string CN_LOAI_THANHTOAN = "Thanh toán";
        public const string CN_LOAI_DIEUCHINH = "Điều chỉnh";
        public const string CN_LOAI_TRAHANG = "Trả hàng";
        public const string CN_LOAI_THANHTOANTRAHANG = "Thanh toán trả hàng";

        /*
         * loại khuyến mãi
         */
        public const string KM_LOAI_SANPHAM = "KM-san-pham";
        public const string KM_LOAI_SOLUONG = "KM-so-luong";
        public const string KM_LOAI_TANGTHEM = "KM-tang-them";


        /*
         * hàng hóa nhà cung cấp PEPSI 
         */
        public const string SP_LOAIKET = "ket";
        public const string SP_MAPEPSI = "PIVN";

        /* 
         * Mã các khoản thu/chi
         * */
        public const string PC_KHOAN_TRAHANG = "tra-hang";
        public const string PC_KHOAN_MUAHANG = "mua-hang";
        public const string PT_KHOAN_BANHANG = "BH";
        public const string PT_KHOAN_THANHTOANNO = "thanh-toan-no";

        /* 
         * Mã liên hệ
         */
        public const string LH_KHACHHANG = "khachhang";
        public const string LH_NHACUNGCAP = "nhacungcap";

        /* 
         * Mã vai trò
         */
        public const string VAITRO_KETOAN = "ketoan";
        public const string VAITRO_DIEUHANH = "dieuhanh";
        public const string VAITRO_THUKHO = "thukho";

    }
}