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

namespace BookingApp.Controllers.QuanLyKhuyenMai
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.DataProvider.EF;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DM_MaKhuyenMai>("DM_MaKhuyenMai");
    builder.EntitySet<TaiKhoan>("TaiKhoans"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DM_MaKhuyenMaiController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/DM_MaKhuyenMai
        [EnableQuery]
        public IQueryable<DM_MaKhuyenMai> GetDM_MaKhuyenMai()
        {
            return db.DM_MaKhuyenMai;
        }

        // GET: odata/DM_MaKhuyenMai(5)
        [EnableQuery]
        public SingleResult<DM_MaKhuyenMai> GetDM_MaKhuyenMai([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DM_MaKhuyenMai.Where(dM_MaKhuyenMai => dM_MaKhuyenMai.Id == key));
        }

        // PUT: odata/DM_MaKhuyenMai(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<DM_MaKhuyenMai> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_MaKhuyenMai dM_MaKhuyenMai = db.DM_MaKhuyenMai.Find(key);
            if (dM_MaKhuyenMai == null)
            {
                return NotFound();
            }

            patch.Put(dM_MaKhuyenMai);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_MaKhuyenMaiExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_MaKhuyenMai);
        }

        // POST: odata/DM_MaKhuyenMai
        public IHttpActionResult Post(DM_MaKhuyenMai dM_MaKhuyenMai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dM_MaKhuyenMai.Id=Guid.NewGuid();
            db.DM_MaKhuyenMai.Add(dM_MaKhuyenMai);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DM_MaKhuyenMaiExists(dM_MaKhuyenMai.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dM_MaKhuyenMai);
        }

        // PATCH: odata/DM_MaKhuyenMai(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<DM_MaKhuyenMai> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_MaKhuyenMai dM_MaKhuyenMai = db.DM_MaKhuyenMai.Find(key);
            if (dM_MaKhuyenMai == null)
            {
                return NotFound();
            }

            patch.Patch(dM_MaKhuyenMai);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_MaKhuyenMaiExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_MaKhuyenMai);
        }

        // DELETE: odata/DM_MaKhuyenMai(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            DM_MaKhuyenMai dM_MaKhuyenMai = db.DM_MaKhuyenMai.Find(key);
            if (dM_MaKhuyenMai == null)
            {
                return NotFound();
            }

            db.DM_MaKhuyenMai.Remove(dM_MaKhuyenMai);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DM_MaKhuyenMai(5)/TaiKhoan
        [EnableQuery]
        public SingleResult<TaiKhoan> GetTaiKhoan([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DM_MaKhuyenMai.Where(m => m.Id == key).Select(m => m.TaiKhoan));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DM_MaKhuyenMaiExists(Guid key)
        {
            return db.DM_MaKhuyenMai.Count(e => e.Id == key) > 0;
        }
    }
}
