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

namespace BookingApp.Controllers.QuanLyThongBao.Odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.DataProvider.EF;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ThongBao>("ThongBaos");
    builder.EntitySet<TaiKhoan>("TaiKhoans"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ThongBaosController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/ThongBaos
        [EnableQuery]
        public IQueryable<ThongBao> GetThongBaos()
        {
            return db.ThongBaos;
        }

        // GET: odata/ThongBaos(5)
        [EnableQuery]
        public SingleResult<ThongBao> GetThongBao([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ThongBaos.Where(thongBao => thongBao.Id == key));
        }

        // PUT: odata/ThongBaos(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<ThongBao> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ThongBao thongBao = db.ThongBaos.Find(key);
            if (thongBao == null)
            {
                return NotFound();
            }

            patch.Put(thongBao);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThongBaoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(thongBao);
        }

        // POST: odata/ThongBaos
        public IHttpActionResult Post(ThongBao thongBao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            thongBao.Id = Guid.NewGuid();
            db.ThongBaos.Add(thongBao);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ThongBaoExists(thongBao.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(thongBao);
        }

        // PATCH: odata/ThongBaos(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<ThongBao> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ThongBao thongBao = db.ThongBaos.Find(key);
            if (thongBao == null)
            {
                return NotFound();
            }

            patch.Patch(thongBao);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThongBaoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(thongBao);
        }

        // DELETE: odata/ThongBaos(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            ThongBao thongBao = db.ThongBaos.Find(key);
            if (thongBao == null)
            {
                return NotFound();
            }

            db.ThongBaos.Remove(thongBao);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ThongBaos(5)/TaiKhoan
        [EnableQuery]
        public SingleResult<TaiKhoan> GetTaiKhoan([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ThongBaos.Where(m => m.Id == key).Select(m => m.TaiKhoan));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThongBaoExists(Guid key)
        {
            return db.ThongBaos.Count(e => e.Id == key) > 0;
        }
    }
}
