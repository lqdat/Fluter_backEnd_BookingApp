using BookingApp.Ultility.BaseMethod;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.OData;

namespace BookingApp.Ultility.BaseControllers
{
    [Authorize]
    public class BaseOdataController<TKey, TEntity, TContext> : BaseOdataGetController<TKey, TEntity, TContext>
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

            if (entity.GetType().GetProperty("IsDM") != null)
            {
                var isDM = true;
                entity.GetType().GetProperty("IsDM").SetValue(entity, isDM);
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

        [AcceptVerbs("PATCH", "MERGE")]
        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            Validate(patch.GetEntity());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TEntity entity = db.Set<TEntity>().Find(key);

            if (entity == null)
                return NotFound();

            patch.Patch(entity);

            try
            {
                db.SaveChangesWithGuid();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TExists(key))
                    return NotFound();
                else
                    throw;
            }

            return Updated(entity);
        }
    }
}