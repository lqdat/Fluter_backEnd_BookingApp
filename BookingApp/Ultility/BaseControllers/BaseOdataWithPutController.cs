using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.OData;

namespace BookingApp.Ultility.BaseControllers
{
    public class BaseOdataWithPutController<TKey, TEntity, TContext> : BaseOdataGetController<TKey, TEntity, TContext>
        where TEntity : class
        where TContext : BaseContext
    {
        [AcceptVerbs("PATCH", "MERGE")]
        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            Validate(patch.GetEntity());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TEntity entity = db.Set<TEntity>().Find(key);
            entity = CustomPatch(entity);
            validationPatch(entity);
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