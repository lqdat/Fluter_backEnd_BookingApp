using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace BookingApp.Ultility.BaseControllers
{
    public class BaseContext : DbContext
    {
        private string v;

        public BaseContext(string v) : base(v)
        {
            this.v = v;
        }

        public string UserName
        {
            get; private set;
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);
                //throw new Exception("Thao tác thất bại. Vui lòng liên hệ GDTVietNam để được hỗ trợ!");

            }


        }


        /// <summary>
        /// Savechanges với kiểu dữ liệu primarykey là Int
        /// </summary>
        /// <returns></returns>
        public int SaveChangesWithInt()
        {
            try
            {
                SavingChangesInt();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);

                //throw new Exception("Thao tác thất bại. Vui lòng liên hệ GDTVietNam để được hỗ trợ!");
            }

        }
        private void SavingChangesInt()
        {

            var objects = this.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added ||
                    p.State == EntityState.Deleted ||
                    p.State == EntityState.Modified);
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            UserName = identity?.FindFirst("ma_tai_khoan")?.Value ?? (identity?.IsAuthenticated == true ? "Anonymous" : "Public");

            ObjectContext objectContext = ((IObjectContextAdapter)this).ObjectContext;
            foreach (DbEntityEntry entry in objects
                .Where(p => p.State == EntityState.Added))
            {
                if (entry.Entity != null)
                {
                    //Lấy primary key
                    var a = entry.Entity.GetType();
                    string name = a.Name;
                    MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                    .MakeGenericMethod(a);
                    dynamic objectSet = method.Invoke(objectContext, null);

                    a.GetProperty("CreatedBy").SetValue(entry.Entity, UserName);
                    a.GetProperty("CreatedDate").SetValue(entry.Entity, DateTime.Now);
                }
            }

            foreach (DbEntityEntry entry in objects
                .Where(p => p.State == EntityState.Modified))
            {
                if (entry.Entity != null)
                {
                    var a = entry.Entity.GetType().BaseType;
                    string name = a.Name;

                    if (HttpContext.Current.Request.HttpMethod == "POST")
                    {
                        if ((entry.Property("CreatedBy") != null && entry.Property("CreatedBy").IsModified) ||
                        (entry.Property("CreatedDate") != null && entry.Property("CreatedDate").IsModified))
                        {
                            throw new Exception("Thao tác thất bại. Vui lòng liên hệ GDTVietNam để được hỗ trợ!");
                        }
                    }
                    MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                    .MakeGenericMethod(a);
                    dynamic objectSet = method.Invoke(objectContext, null);
                    IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
                    string keyNames = keyMembers.Select(k => (string)k.Name).First();

                    if (HttpContext.Current.Request.HttpMethod == "PUT" || HttpContext.Current.Request.HttpMethod == "PATCH")
                    {
                        a.GetProperty("UpdatedBy").SetValue(entry.Entity, UserName);
                        a.GetProperty("UpdatedDate").SetValue(entry.Entity, DateTime.Now);
                    }
                }
            }
        }

        /// <summary>
        /// Savechanges với kiểu dữ liệu primarykey là Guid
        /// </summary>
        /// <returns></returns>
        public int SaveChangesWithGuid()
        {
            try
            {

                SavingChanges();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);

                //throw new Exception("Thao tác thất bại. Vui lòng liên hệ GDTVietNam để được hỗ trợ!");
            }

        }

        private void SavingChanges()
        {

            var objects = this.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added ||
                    p.State == EntityState.Deleted ||
                    p.State == EntityState.Modified);
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            UserName = identity?.FindFirst("ma_tai_khoan")?.Value ?? (identity?.IsAuthenticated == true ? "Anonymous" : "Public");

            ObjectContext objectContext = ((IObjectContextAdapter)this).ObjectContext;
            foreach (DbEntityEntry entry in objects
                .Where(p => p.State == EntityState.Added))
            {
                if (entry.Entity != null)
                {
                    //Lấy primary key
                    var a = entry.Entity.GetType();
                    string name = a.Name;
                    MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                    .MakeGenericMethod(a);
                    dynamic objectSet = method.Invoke(objectContext, null);

                    Guid guid = Guid.NewGuid();
                    a.GetProperty("CreatedBy").SetValue(entry.Entity, UserName);
                    a.GetProperty("CreatedDate").SetValue(entry.Entity, DateTime.Now);
                    a.GetProperty("Id").SetValue(entry.Entity, guid);
                }
            }

            foreach (DbEntityEntry entry in objects
                .Where(p => p.State == EntityState.Modified))
            {
                if (entry.Entity != null)
                {
                    var a = entry.Entity.GetType().BaseType;
                    string name = a.Name;

                    if (HttpContext.Current.Request.HttpMethod == "POST")
                    {
                        if ((entry.Property("CreatedBy") != null && entry.Property("CreatedBy").IsModified) ||
                        (entry.Property("CreatedDate") != null && entry.Property("CreatedDate").IsModified))
                        {
                            throw new Exception("Thao tác thất bại. Vui lòng liên hệ GDTVietNam để được hỗ trợ!");
                        }
                    }
                    MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                    .MakeGenericMethod(a);
                    dynamic objectSet = method.Invoke(objectContext, null);
                    IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
                    string keyNames = keyMembers.Select(k => (string)k.Name).First();

                    if (HttpContext.Current.Request.HttpMethod == "PUT" || HttpContext.Current.Request.HttpMethod == "PATCH")
                    {
                        a.GetProperty("UpdatedBy").SetValue(entry.Entity, UserName);
                        a.GetProperty("UpdatedDate").SetValue(entry.Entity, DateTime.Now);
                    }
                }
            }
        }
    }
}
