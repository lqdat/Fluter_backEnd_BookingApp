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
    builder.EntitySet<DM_Voucher>("DM_Voucher");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DM_VoucherController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/DM_Voucher
        [EnableQuery]
        public IQueryable<DM_Voucher> GetDM_Voucher()
        {
            return db.DM_Voucher;
        }

        // GET: odata/DM_Voucher(5)
        [EnableQuery]
        public SingleResult<DM_Voucher> GetDM_Voucher([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DM_Voucher.Where(dM_Voucher => dM_Voucher.Id == key));
        }

        // PUT: odata/DM_Voucher(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<DM_Voucher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_Voucher dM_Voucher = db.DM_Voucher.Find(key);
            if (dM_Voucher == null)
            {
                return NotFound();
            }

            patch.Put(dM_Voucher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_VoucherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_Voucher);
        }

        // POST: odata/DM_Voucher
        public IHttpActionResult Post(DM_Voucher dM_Voucher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dM_Voucher.Id= Guid.NewGuid();
            db.DM_Voucher.Add(dM_Voucher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DM_VoucherExists(dM_Voucher.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dM_Voucher);
        }

        // PATCH: odata/DM_Voucher(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<DM_Voucher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DM_Voucher dM_Voucher = db.DM_Voucher.Find(key);
            if (dM_Voucher == null)
            {
                return NotFound();
            }

            patch.Patch(dM_Voucher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DM_VoucherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dM_Voucher);
        }

        // DELETE: odata/DM_Voucher(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            DM_Voucher dM_Voucher = db.DM_Voucher.Find(key);
            if (dM_Voucher == null)
            {
                return NotFound();
            }

            db.DM_Voucher.Remove(dM_Voucher);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DM_VoucherExists(Guid key)
        {
            return db.DM_Voucher.Count(e => e.Id == key) > 0;
        }
    }
}
