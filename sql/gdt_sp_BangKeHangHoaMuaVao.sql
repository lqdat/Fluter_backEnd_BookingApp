alter proc gdt_sp_BangKeHangHoaMuaVao
(
	@DiemKinhDoanhId int,
	@NccId int,
	@tuNgay datetime,
	@denNgay datetime
)
as

select TenNhaCungCap
, NhomSanPham
, MaSanPham
, TenSanPham
, DonViTinh
, sum(SoLuong) as SoLuong
, DonGia
, sum(ThanhTien) as ThanhTien
from
(
	select ncc.Ten as TenNhaCungCap
	, nhomsp.Ten as NhomSanPham
	, sp.MaSanPham
	, sp.TenSanPham
	, dv.Ten as DonViTinh
	, ct.SoLuong
	, ct.DonGia
	, ct.ThanhTien
	from Sale_MuaHang mh
	inner join Sale_ChiTietMuaHang ct on ct.MuaHang_Id = mh.Id
	left outer join DM_LienHe ncc on ncc.Id = mh.NhaCungCap_Id
	left outer join DM_SanPhamDichVu sp on sp.Id = ct.SanPhamDichVu_Id
	left outer join DM_NhomSanPhamDichVu nhomsp on nhomsp.Id = sp.NhomSanPhamDichVu_Id
	left outer join DM_DonViTinh dv on dv.Id = sp.DonViTinh_Id
	where mh.IsDeleted = 0 and ( @NccId is null or mh.NhaCungCap_Id = @NccId) 
	and mh.DiemKinhDoanhId = @DiemKinhDoanhId
	and (@tuNgay is null or (mh.NgayTao >= @tuNgay and mh.NgayTao <= @denNgay))
)g
GROUP BY TenNhaCungCap
, NhomSanPham
, MaSanPham
, TenSanPham
, DonViTinh
, DonGia
order by MaSanPham