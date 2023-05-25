
alter proc gdt_sp_LoiNhuanTheoDonHang
(
	@DiemKinhDoanhId int,
	@nguoiTao varchar(50),
	@tuNgay datetime,
	@denNgay datetime
)
as 
	select 
	dh.NgayTao as NgayTao,
	dh.MaDonHang as MaDonHang,
	dh.TienHang as TienHang,
	dh.GiamGia  as GiamGia,
	dh.ThanhTien as DoanhThu,
	sum( ct.GiaVon * ct.SoLuong) as TienVonThu,
	dh.ThanhTien - sum( ct.GiaVon * ct.SoLuong) as LoiNhuanGop
	 from 
	 Sale_DonHang dh 
	left join Sale_ChiTietDonHang ct on ct.DonHang_Id = dh.Id
	where  dh.IsDeleted = 0 
	and dh.DiemKinhDoanh_Id = @DiemKinhDoanhId
    and (@nguoiTao is null or (dh.NguoiTao  = @nguoiTao))
	and @tuNgay is null or (dh.NgayTao  >= @tuNgay and  dh.NgayTao <= @denNgay)

	group by dh.MaDonHang,
	dh.TienHang,
	dh.GiamGia,
	dh.ThanhTien,
	dh.NgayTao 
	
	union 

	select
	 dth.NgayTao as NgayTao,
	 dth.MaTraHang as MaDonHang,
	 dth.TienHangTra as TienHang,
	 GiamGia = 0,
	 dth.ThanhTien as DoanhThu,
	 sum(ctdth.GiaVon * ctdth.SoLuong) as TienVon,
	 dth.ThanhTien - sum(ctdth.GiaVon * ctdth.SoLuong) as LoiNhuanGop
	 from 
	 Sale_DonTraHang dth 
	left join Sale_ChiTietDonTraHang ctdth on ctdth.DonTraHang_Id = dth.Id
	where dth.DiemKinhDoanh_Id = @DiemKinhDoanhId
    and (@nguoiTao is null or dth.NguoiTao  = @nguoiTao)
	and @tuNgay is null or (dth.NgayTao >= @tuNgay and dth.NgayTao <= @denNgay) 

	group by dth.MaTraHang,
	dth.ThanhTien,
	dth.TienHangTra,
	dth.NgayTao

	order by NgayTao desc