using System.Net;
using System.Web.Http;
using System.Web.Http.OData;

namespace BookingApp.Ultility.BaseControllers
{
    [Authorize]
    public class BaseOdataWithDeleteController<TKey, TEntity, TContext> : BaseOdataController<TKey, TEntity, TContext>
        where TEntity : class
        where TContext : BaseContext
    {
        [HttpDelete]
        public IHttpActionResult Delete([FromODataUri] TKey key)
        {
            TEntity entity = db.Set<TEntity>().Find(key);
            if (entity == null)
                return NotFound();
            validationDelete(entity);

            db.Set<TEntity>().Remove(entity);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}