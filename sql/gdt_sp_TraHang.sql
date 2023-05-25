alter proc gdt_sp_TraHang
(
	@DiemKinhDoanhId int,
	@nguoiTao varchar(50),
	@tuNgay datetime,
	@denNgay datetime
)
as
select 
NgayTao,
MaTraHang,
TienHangTra,
PhiTra,
ThanhTien
from
	Sale_DonTraHang dth
	where dth.IsDeleted = 0
	and dth.DiemKinhDoanh_Id = @DiemKinhDoanhId 
	and @tuNgay is null or (dth.NgayTao >= @tuNgay and dth.NgayTao <= @denNgay)
	and @nguoiTao is null or (dth.NguoiTao  = @nguoiTao)
	order by NgayTao desc