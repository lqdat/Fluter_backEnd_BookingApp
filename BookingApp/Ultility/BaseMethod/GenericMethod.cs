using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BookingApp.Ultility.BaseMethod
{
    public static class GenericMethod
    {
        public static string GetPrimaryKey<TEntity>(this DbContext _context) where TEntity : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            return set.EntitySet.ElementType.KeyMembers.Select(k => k.Name).First();
        }

        public static void RemoveTree<TEntity>(this DbContext _context, TEntity entity) where TEntity : class
        {
            string keyName = GetPrimaryKey<TEntity>(_context);
            var value = entity.GetType().GetProperty(keyName).GetValue(entity);
            List<TEntity> items = _context.Set<TEntity>().Where(CreatePredicate<TEntity>("Parent_ID", value)).ToList();
            foreach (TEntity item in items)
                if (item.GetType().GetProperty("Parent_Id").GetValue(item).ToString() == value.ToString())
                    _context.RemoveTree(item);
            _context.Set<TEntity>().Remove(entity);
        }

        public static Expression<Func<TEntity, bool>> CreatePredicate<TEntity>(string columnName, object searchValue)
            where TEntity : class
        {
            var xType = typeof(TEntity);
            var x = Expression.Parameter(xType, "x");
            var column = xType.GetProperties().FirstOrDefault(p => p.Name == columnName);

            var body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Equal(
                    Expression.PropertyOrField(x, columnName),
                    Expression.Convert(Expression.Constant(searchValue), Expression.PropertyOrField(x, columnName).Type));

            return Expression.Lambda<Func<TEntity, bool>>(body, x);
        }

        public static Expression<Func<TEntity, bool>> CreateNotNull<TEntity>(string columnName)
            where TEntity : class
        {
            var xType = typeof(TEntity);
            var x = Expression.Parameter(xType, "x");
            var column = xType.GetProperties().FirstOrDefault(p => p.Name == columnName);

            var body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.NotEqual(
                    Expression.PropertyOrField(x, columnName),
                    Expression.Constant(null, Expression.Parameter(xType).Type));

            return Expression.Lambda<Func<TEntity, bool>>(body, x);
        }

        public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;

                //#if EF5 || EF6
                var isDbSet = setType.IsGenericType && (typeof(IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof(IDbSet<>).FullName) != null);
                //#elif EF7
                //            var isDbSet = setType.IsGenericType && (typeof (DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                //#endif

                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }

            return dbSetProperties;

        }
    }
}