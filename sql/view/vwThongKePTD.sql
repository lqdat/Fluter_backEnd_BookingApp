create view vwThongKePTD as
select ptd.Name,TenDonVi,ptd.HanKiemDinh,
	(case
		when ptd.HanKiemDinh <= 0 then 'Het han'
		when ptd.HanKiemDinh > 0 and ptd.HanKiemDinh < 30 then 'Sap het han'
		when ptd.HanKiemDinh > 30 then 'Binh thuong'
	end) as TrangThai
from SYS_DonVi
left join
(
	select DonViId, Name, 
	DATEDIFF(day,GETDATE(),HieuLuc) as HanKiemDinh
	from DM_PhuongTienDo
)	as ptd
on SYS_DonVi.Id = ptd.DonViId