alter proc gdt_sp_LoiNhuanTheoThoiGian
(
	@DiemKinhDoanhId int,
	@nguoiTao varchar(50),
	@tuNgay datetime,
	@denNgay datetime 
)
as
select
CAST (NgayTao as date) as NgayTao ,
sum(g.TienHangThu) - sum(g.TienHangTra) as TienHang,
sum(g.GiamGia) as GiamGia,
sum(g.TienThu) - sum(g.TienTra) as DoanhThu,
sum(g.TienVonThu) - sum(g.TienVonTra) as TienVon,
sum(g.LoiNhuanGopThu) - sum(g.LoiNhuanGopTra) as LoiNhuanGop
from (
	 select 
	cast( dh.NgayTao as date) as NgayTao,
	dh.TienHang as TienHangThu,
	dh.GiamGia as GiamGia,
	dh.ThanhTien as TienThu,
	sum( ct.GiaVon * ct.SoLuong) as TienVonThu,
	dh.ThanhTien - sum( ct.GiaVon * ct.SoLuong) as LoiNhuanGopThu,
	 TienHangTra =0,
	 TienTra = 0,
	 TienVonTra=0,
	 LoiNhuanGopTra = 0
	 from 
	 Sale_DonHang dh 
	left join Sale_ChiTietDonHang ct on ct.DonHang_Id = dh.Id
	where  dh.IsDeleted = 0 
	and dh.DiemKinhDoanh_Id = @DiemKinhDoanhId
	and @tuNgay is null or (dh.NgayTao  >= @tuNgay and  dh.NgayTao <= @denNgay)
	and @nguoiTao is null or (dh.NguoiTao  = @nguoiTao)
	group by dh.MaDonHang,
	dh.TienHang,
	dh.GiamGia,
	dh.ThanhTien,
	dh.NgayTao ,
	cast( dh.NgayTao as date)

	union

	 select 
	 cast( dth.NgayTao as date) as NgayTao,
	 TienHangThu =0,
	 GiamGia = 0,
	 TienThu = 0,
	 TienVonThu=0,
	 LoiNhuanGopThu =0,
	 dth.TienHangTra as TienHangTra,
	 dth.ThanhTien as TienTra,
	 sum(ctdth.GiaVon * ctdth.SoLuong) as TienVonTra,
	 dth.ThanhTien - sum(ctdth.GiaVon * ctdth.SoLuong) as LoiNhuanGopTra
	 from 
	 Sale_DonTraHang dth 
	left join Sale_ChiTietDonTraHang ctdth on ctdth.DonTraHang_Id = dth.Id
	where dth.IsDeleted = 0 
	and dth.DiemKinhDoanh_Id = @DiemKinhDoanhId
	and @tuNgay is null or (dth.NgayTao  >= @tuNgay and  dth.NgayTao <= @denNgay)
	and @nguoiTao is null or (dth.NguoiTao  = @nguoiTao)
	group by 
	dth.MaTraHang,
	dth.ThanhTien,
	dth.TienHangTra,
	dth.NgayTao,
	cast( dth.NgayTao as date)
)g
group by CAST (NgayTao as date)