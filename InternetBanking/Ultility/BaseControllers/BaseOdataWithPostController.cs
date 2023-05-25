using BookingApp.Ultility.BaseMethod;
using BookingApp.Ultility.BaseObject;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Http;

namespace BookingApp.Ultility.BaseControllers
{
    public class BaseOdataWithPostController<TKey, TEntity, TContext> : BaseOdataGetController<TKey, TEntity, TContext>
        where TEntity : class
        where TContext : BaseContext
    {
        [HttpPost]
        public IHttpActionResult Post(TEntity entity)
        {
            entity = CustomPost(entity);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            validationPost(entity);
            if (entity.GetType().GetProperty("MangNgayThangNam") != null)
            {
                DateTime tuNgay = (DateTime)entity.GetType().GetProperty("TuNgay").GetValue(entity, null);
                DateTime denNgay = (DateTime)entity.GetType().GetProperty("DenNgay").GetValue(entity, null);
                if (tuNgay > denNgay)
                    return BadRequest("Từ ngày phải nhỏ hơn đến ngày!");
                List<NgayThangNamModel> ngayThangNams = NgayThangNamModel.get_NgayThangNam(tuNgay, denNgay, false);
                entity.GetType().GetProperty("MangNgayThangNam").SetValue(entity, NgayThangNamModel.JsonStringify(ngayThangNams));
            }

            db.Set<TEntity>().Add(entity);

            try
            {
                db.SaveChangesWithGuid();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TExists((TKey)entity.GetType().GetProperty(GenericMethod.GetPrimaryKey<TEntity>(db)).GetValue(entity, null)))
                    return NotFound();
                else
                    throw;
            }

            return Created(entity);
        }

    }
}