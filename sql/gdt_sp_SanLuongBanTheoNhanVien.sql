alter proc gdt_sp_SanLuongBanTheoNhanVien
(
	@DiemKinhDoanhId int,
	@tuNgay datetime,
	@denNgay datetime
)
as

select TenNhanVien
, MaNhanVien
, SoDienThoai
, MaSanPham
, TenSanPham
, DonViTinh
, sum(SoLuong) as SoLuong
, max(DonGia) as DonGia
, sum(ThanhTien) as ThanhTien
, sum(SoLuongKM) as SoLuongKM
, sum(TongSoLuong) as TongSoLuong
from
(
	select nv.TenNhanVien as TenNhanVien
	, nv.MaNhanVien as MaNhanVien
	, nv.SoDienThoai
	, sp.MaSanPham
	, sp.TenSanPham
	, dv.Ten as DonViTinh
	, ct.SoLuong
	, ct.DonGia
	, ct.ThanhTien
	, ct.SoLuongKM
	, ct.SoLuong + ct.SoLuongKM as TongSoLuong
	from
	(
		select KhachHang_Id, DonHang_Id, SanPhamDichVu_Id, SoLuong, DonGia, ct.ThanhTien, 0 as SoLuongKM
		from Sale_DonHang dh
		inner join Sale_ChiTietDonHang ct on ct.DonHang_Id = dh.Id
		where dh.IsDeleted = 0
		and ct.ChiTietKhuyenMai_Id is null
		and dh.DiemKinhDoanh_Id = @DiemKinhDoanhId
		and (@tuNgay is null or (dh.NgayTao >= @tuNgay and dh.NgayTao <= @denNgay))

		union all
		select KhachHang_Id, DonHang_Id, SanPhamDichVu_Id, 0 as SoLuong, DonGia, ct.ThanhTien, SoLuong as SoLuongKM
		from Sale_DonHang dh
		inner join Sale_ChiTietDonHang ct on ct.DonHang_Id = dh.Id
		where dh.IsDeleted = 0
		and ct.ChiTietKhuyenMai_Id is not null
		and dh.DiemKinhDoanh_Id = @DiemKinhDoanhId
		and (@tuNgay is null or (dh.NgayTao >= @tuNgay and dh.NgayTao <= @denNgay))
	) ct
	left outer join DM_LienHe kh on kh.Id = ct.KhachHang_Id
	left outer join DM_NhanVien nv on nv.Id = kh.NhanVienKinhDoanh_Id
	left outer join DM_SanPhamDichVu sp on sp.Id = ct.SanPhamDichVu_Id
	left outer join DM_DonViTinh dv on dv.Id = sp.DonViTinh_Id
)g
GROUP BY TenNhanVien
, MaNhanVien
, SoDienThoai
, MaSanPham
, TenSanPham
, DonViTinh
order by MaSanPham