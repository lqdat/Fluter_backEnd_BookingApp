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

namespace BookingApp.Controllers.QuanlyDatXe
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.DataProvider.EF;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<BookHistory>("BookHistories");
    builder.EntitySet<TaiKhoan>("TaiKhoans"); 
    builder.EntitySet<DM_Xe>("DM_Xe"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BookHistoriesController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/BookHistories
        [EnableQuery]
        public IQueryable<BookHistory> GetBookHistories()
        {
            return db.BookHistories;
        }

        // GET: odata/BookHistories(5)
        [EnableQuery]
        public SingleResult<BookHistory> GetBookHistory([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BookHistories.Where(bookHistory => bookHistory.Id == key));
        }

        // PUT: odata/BookHistories(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<BookHistory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookHistory bookHistory = db.BookHistories.Find(key);
            if (bookHistory == null)
            {
                return NotFound();
            }

            patch.Put(bookHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookHistoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bookHistory);
        }

        // POST: odata/BookHistories
        public IHttpActionResult Post(BookHistory bookHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            bookHistory.Id=Guid.NewGuid();
            db.BookHistories.Add(bookHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookHistoryExists(bookHistory.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(bookHistory);
        }

        // PATCH: odata/BookHistories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<BookHistory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookHistory bookHistory = db.BookHistories.Find(key);
            if (bookHistory == null)
            {
                return NotFound();
            }

            patch.Patch(bookHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookHistoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bookHistory);
        }

        // DELETE: odata/BookHistories(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            BookHistory bookHistory = db.BookHistories.Find(key);
            if (bookHistory == null)
            {
                return NotFound();
            }

            db.BookHistories.Remove(bookHistory);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BookHistories(5)/TaiKhoan
        [EnableQuery]
        public SingleResult<TaiKhoan> GetTaiKhoan([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BookHistories.Where(m => m.Id == key).Select(m => m.TaiKhoan));
        }

        // GET: odata/BookHistories(5)/DM_Xe
        [EnableQuery]
        public SingleResult<DM_Xe> GetDM_Xe([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BookHistories.Where(m => m.Id == key).Select(m => m.DM_Xe));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookHistoryExists(Guid key)
        {
            return db.BookHistories.Count(e => e.Id == key) > 0;
        }
    }
}
