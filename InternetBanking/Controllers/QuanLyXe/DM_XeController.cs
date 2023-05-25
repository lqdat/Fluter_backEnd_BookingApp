using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using BookingApp.DataProvider.EF;

namespace BookingApp.Controllers.QuanLyXe
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.DataProvider.EF;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DM_Xe>("DM_Xe");
    builder.EntitySet<BookHistory>("BookHistories"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DM_XeController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/DM_Xe
        [EnableQuery]
        public IQueryable<DM_Xe> GetDM_Xe()
        {
            return db.DM_Xe;
        }

        // GET: odata/DM_Xe(5)
        [EnableQuery]
        public SingleResult<DM_Xe> GetDM_Xe([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DM_Xe.Where(dM_Xe => dM_Xe.Id == key));
        }

        // PUT: odata/DM_Xe(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<DM_Xe> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_Xe dM_Xe = db.DM_Xe.Find(key);
            if (dM_Xe == null)
            {
                return NotFound();
            }

            patch.Put(dM_Xe);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_XeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_Xe);
        }

        // POST: odata/DM_Xe
        public IHttpActionResult Post(DM_Xe dM_Xe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dM_Xe.Id = Guid.NewGuid();
            db.DM_Xe.Add(dM_Xe);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DM_XeExists(dM_Xe.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dM_Xe);
        }

        // PATCH: odata/DM_Xe(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<DM_Xe> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_Xe dM_Xe = db.DM_Xe.Find(key);
            if (dM_Xe == null)
            {
                return NotFound();
            }

            patch.Patch(dM_Xe);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_XeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_Xe);
        }

        // DELETE: odata/DM_Xe(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            DM_Xe dM_Xe = db.DM_Xe.Find(key);
            if (dM_Xe == null)
            {
                return NotFound();
            }

            db.DM_Xe.Remove(dM_Xe);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DM_Xe(5)/BookHistories
        [EnableQuery]
        public IQueryable<BookHistory> GetBookHistories([FromODataUri] Guid key)
        {
            return db.DM_Xe.Where(m => m.Id == key).SelectMany(m => m.BookHistories);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DM_XeExists(Guid key)
        {
            return db.DM_Xe.Count(e => e.Id == key) > 0;
        }
    }
}
