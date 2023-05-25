using BookingApp.Ultility.BaseMethod;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;

namespace BookingApp.Ultility.BaseControllers
{
    public class BaseOdataGetController<TKey, TEntity, TContext> : ODataController
        where TEntity : class
        where TContext : BaseContext
    {
        protected TContext db;

        [EnableQuery(PageSize = 50)]
        [HttpGet]
        public virtual IQueryable<TEntity> Get()
        {
            return db.Set<TEntity>();
        }

        [EnableQuery]
        [HttpGet]
        public virtual SingleResult<TEntity> Get([FromODataUri] TKey key)
        {
            return SingleResult.Create(db.Set<TEntity>().Where(GenericMethod.CreatePredicate<TEntity>(GenericMethod.GetPrimaryKey<TEntity>(db), key)));
        }

        public virtual void validationPost(TEntity entity)
        {

        }

        public virtual void validationPatch(TEntity entity)
        { }

        public virtual void validationDelete(TEntity entity)
        { }

        public TEntity CustomPost(TEntity entity)
        {
            return entity;
        }
        public TEntity CustomPatch(TEntity entity)
        {
            return entity;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        protected bool TExists(TKey key)
        {
            return db.Set<TEntity>().Find(key) != null;
        }
    }
}