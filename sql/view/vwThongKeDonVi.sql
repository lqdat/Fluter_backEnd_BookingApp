alter view vwThongKeDonVi
as
select TenDonVi
, tb.SLHetHanHieuLuc
,tb.SLSapHetHanHieuLuc15Ngay
from SYS_DonVi
left outer join
(
	
	select
	DonViId,
	sum(SLHetHanHieuLuc) as SLHetHanHieuLuc,
	sum(SLSapHetHanHieuLuc15Ngay) as SLSapHetHanHieuLuc15Ngay
	from
	(
	select
	DonViId
	, count(Id) as SLSapHetHanHieuLuc15Ngay
	, SLHetHanHieuLuc = 0
	from DM_PhuongTienDo
	where DATEDIFF(day,GETDATE(),HieuLuc) > 0 and DATEDIFF(day,GETDATE(),HieuLuc) < 15
	group by DonViId

	union 

	select
	DonViId
	,SLSapHetHanHieuLuc = 0
	, count(Id) as SLHetHanHieuLuc
	from DM_PhuongTienDo
	where DATEDIFF(day, HieuLuc, GETDATE()) > 0
	group by DonViId
	)tb2
	group by DonViId


) as tb on Id = tb.DonViId