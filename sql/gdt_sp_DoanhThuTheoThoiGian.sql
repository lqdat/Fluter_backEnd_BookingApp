alter proc gdt_sp_DoanhThuTheoThoiGian
(
	@DiemKinhDoanhId int,
	@nguoiTao varchar(50),
	@tuNgay datetime,
	@denNgay datetime
)
as

select CAST(NgayTao AS DATE) as Ngay
, sum(TienThu) as TienThu
, sum(TienTra) as TienTra
, sum(DoanhThuThuan) as DoanhThuThuan
from
(
	select *
	, TienThu - TienTra as DoanhThuThuan
	from
	(
		select NgayTao
		, ThanhTien as TienThu
		, 0 as TienTra
		from Sale_DonHang
		where IsDeleted = 0 
		and DiemKinhDoanh_Id = @DiemKinhDoanhId
		and (@nguoiTao is null or NguoiTao = @nguoiTao)
		and (@tuNgay is null or (NgayTao >= @tuNgay and NgayTao <= @denNgay))
	)tb

	union all

	select *
	, TienThu - TienTra as DoanhThuThuan
	from
	(
		select NgayTao
		, 0 as TienThu
		, ThanhTien as TienTra
		from Sale_DonTraHang
		where IsDeleted = 0 
		and DiemKinhDoanh_Id = @DiemKinhDoanhId
		and @nguoiTao is null or NguoiTao = @nguoiTao
		and (@tuNgay is null or (NgayTao >= @tuNgay and NgayTao <= @denNgay))
	)tb2
)g
GROUP BY CAST(NgayTao AS DATE)