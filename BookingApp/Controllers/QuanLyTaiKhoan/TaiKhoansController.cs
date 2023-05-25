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

namespace BookingApp.Controllers.QuanLyTaiKhoan
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.DataProvider.EF;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<TaiKhoan>("TaiKhoans");
    builder.EntitySet<BookHistory>("BookHistories"); 
    builder.EntitySet<DM_MaKhuyenMai>("DM_MaKhuyenMai"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TaiKhoansController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/TaiKhoans
        [EnableQuery]
        public IQueryable<TaiKhoan> GetTaiKhoans()
        {
            return db.TaiKhoans;
        }

        // GET: odata/TaiKhoans(5)
        [EnableQuery]
        public SingleResult<TaiKhoan> GetTaiKhoan([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.TaiKhoans.Where(taiKhoan => taiKhoan.Id == key));
        }

        // PUT: odata/TaiKhoans(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<TaiKhoan> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaiKhoan taiKhoan = db.TaiKhoans.Find(key);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            patch.Put(taiKhoan);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(taiKhoan);
        }

        // POST: odata/TaiKhoans
        public IHttpActionResult Post(TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaiKhoans.Add(taiKhoan);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TaiKhoanExists(taiKhoan.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(taiKhoan);
        }

        // PATCH: odata/TaiKhoans(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<TaiKhoan> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaiKhoan taiKhoan = db.TaiKhoans.Find(key);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            patch.Patch(taiKhoan);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(taiKhoan);
        }

        // DELETE: odata/TaiKhoans(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(key);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/TaiKhoans(5)/BookHistories
        [EnableQuery]
        public IQueryable<BookHistory> GetBookHistories([FromODataUri] Guid key)
        {
            return db.TaiKhoans.Where(m => m.Id == key).SelectMany(m => m.BookHistories);
        }

        // GET: odata/TaiKhoans(5)/DM_MaKhuyenMai
        [EnableQuery]
        public IQueryable<DM_MaKhuyenMai> GetDM_MaKhuyenMai([FromODataUri] Guid key)
        {
            return db.TaiKhoans.Where(m => m.Id == key).SelectMany(m => m.DM_MaKhuyenMai);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaiKhoanExists(Guid key)
        {
            return db.TaiKhoans.Count(e => e.Id == key) > 0;
        }
    }
}
