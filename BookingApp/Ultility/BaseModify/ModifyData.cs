using BookingApp.Ultility.BaseObject;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace BookingApp.Ultility.BaseModify
{
    public class ModifyData : BaseClass
    {
        private MemoryCache memoryCache = MemoryCache.Default;
        //private object _sync;

        public object GetValue<T>(string key)
        {
            Type type = typeof(T);
            return memoryCache.Get(CreateKeyWithRegion(key, type.Name));
        }

        public void Set(string key, object value, DateTimeOffset absExpiration, string regionName)
        {
            if (memoryCache.Contains(CreateKeyWithRegion(key, regionName)))
            {
                memoryCache.Set(CreateKeyWithRegion(key, regionName), value, absExpiration);
            }
            else
            {
                memoryCache.Add(CreateKeyWithRegion(key, regionName), value, absExpiration);
            }
        }

        public void Delete(string key, string regionName = null)
        {
            if (memoryCache.Contains(CreateKeyWithRegion(key, regionName)))
            {
                memoryCache.Remove(CreateKeyWithRegion(key, regionName));
            }
        }

        private string CreateKeyWithRegion(string key, string type)
        {
            return "type:" + (type == null ? "null_type" : type) + ";key=" + key;
        }
    }

    public static class DictionaryStore
    {
        /// <summary>
        /// In-memory cache dictionary
        /// </summary>
        private static Dictionary<string, object> _cache;

        private static object _sync;

        /// <summary>
        /// Cache initializer
        /// </summary>
        static DictionaryStore()
        {
            _cache = new Dictionary<string, object>();
            _sync = new object();
        }

        /// <summary>
        /// Check if an object exists in cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Name of key in cache</param>
        /// <returns>True, if yes; False, otherwise</returns>
        public static bool Exists<T>(string key) where T : class
        {
            Type type = typeof(T);
            lock (_sync)
            {
                return _cache.ContainsKey(type.Name + key);
            }
        }

        /// <summary>
        /// Check if an object exists in cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>True, if yes; False, otherwise</returns>
        public static bool Exists<T>() where T : class
        {
            Type type = typeof(T);

            lock (_sync)
            {
                return _cache.ContainsKey(type.Name);
            }
        }

        /// <summary>
        /// Get an object from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>Object from cache</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);

            lock (_sync)
            {
                if (_cache.ContainsKey(type.Name) == false)
                    throw new ApplicationException("An object of the desired type does not exist: " + type.Name);

                lock (_sync)
                {
                    return (T)_cache[type.Name];
                }
            }
        }

        /// <summary>
        /// Get an object from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Name of key in cache</param>
        /// <returns>Object from cache</returns>
        public static T Get<T>(string key) where T : class

        {
            Type type = typeof(T);
            lock (_sync)
            {
                if (_cache.ContainsKey(key + type.Name) == false)
                    //throw new ApplicationException(String.Format("An object with key '{0}' does not exists", key));
                    return null;

                lock (_sync)
                {
                    return (T)_cache[key + type.Name];
                }
            }
        }

        public static dynamic GetObject(string key)
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(key) == false)
                    //throw new ApplicationException(String.Format("An object with key '{0}' does not exists", key));
                    return null;

                lock (_sync)
                {
                    return _cache[key];
                }
            }
        }

        /// <summary>
        /// Create default instance of the object and add it in cache
        /// </summary>
        /// <typeparam name="T">Class whose object is to be created</typeparam>
        /// <returns>Object of the class</returns>
        public static T Create<T>(string key, params object[] constructorParameters) where T : class
        {
            Type type = typeof(T);
            T value = (T)Activator.CreateInstance(type, constructorParameters);
            lock (_sync)
            {
                if (_cache.ContainsKey(key + type.Name))
                    //throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));
                    _cache.Remove(key + type.Name);

                lock (_sync)
                {
                    _cache.Add(key + type.Name, value);
                }
            }
            return value;
        }

        /// <summary>
        /// Create default instance of the object and add it in cache
        /// </summary>
        /// <typeparam name="T">Class whose object is to be created</typeparam>
        /// <returns>Object of the class</returns>
        public static T Create<T>(params object[] constructorParameters) where T : class
        {
            Type type = typeof(T);
            T value = (T)Activator.CreateInstance(type, constructorParameters);
            lock (_sync)
            {
                if (_cache.ContainsKey(type.Name))
                    throw new ApplicationException(String.Format("An object of type '{0}' already exists", type.Name));

                lock (_sync)
                {
                    _cache.Add(type.Name, value);
                }
            }
            return value;
        }

        //public static void Add<T>(string key, T value)
        //{
        //    Type type = typeof(T);
        //    if (value.GetType() != type)
        //        throw new ApplicationException(String.Format("The type of value passed to cache {0} does not match the cache type {1} for key {2}", value.GetType().FullName, type.FullName, key));
        //    lock (_sync)
        //    {
        //        if (_cache.ContainsKey(key + type.Name))
        //            //throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));
        //            _cache.Remove(key + type.Name);
        //        lock (_sync)
        //        {
        //            _cache.Add(key + type.Name, value);
        //        }
        //    }
        //}

        //public static void AddCanHo(string k, CacheCanHo v)
        //{
        //    lock (_sync)
        //    {
        //        List<CacheCanHo> a = new List<CacheCanHo>();
        //        if (_cache.ContainsKey(k))
        //        {
        //            a = (List<CacheCanHo>)_cache[k];
        //            a.Remove(a.Find(m => m.k == v.k));
        //        }
        //        a.Add(v);
        //        _cache.Remove(k);
        //        lock (_sync)
        //        {
        //            _cache.Add(k, a);
        //        }
        //    }
        //}

        public static void Add<T>(string key, T value) where T : class
        {
            lock (_sync)
            {
                List<T> a = new List<T>();
                if (_cache.ContainsKey(key))
                {
                    a = (List<T>)_cache[key];
                    a.Remove(a.Find(m => m.GetType().GetProperty("k").GetValue(m) == value.GetType().GetProperty("k").GetValue(value)));
                }
                a.Add(value);
                _cache.Remove(key);
                lock (_sync)
                {
                    _cache.Add(key, a);
                }
            }
        }

        public static void Add<T>(string key, List<T> value) where T : class
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(key))
                    _cache.Remove(key);
                lock (_sync)
                {
                    _cache.Add(key, value);
                }
            }
        }

        public static void Update<T>(string key, T value)
        {
            Type type = typeof(T);
            if (value.GetType() != type)
                throw new ApplicationException(String.Format("The type of value passed to cache {0} does not match the cache type {1} for key {2}", value.GetType().FullName, type.FullName, key));
            lock (_sync)
            {
                if (_cache.ContainsKey(key + type.Name))
                    //throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));
                    _cache.Remove(key + type.Name);
                lock (_sync)
                {
                    _cache.Add(key + type.Name, value);
                }
            }
        }

        //public static void Update(string key, CacheModel value)
        //{
        //    lock (_sync)
        //    {
        //        if (_cache.ContainsKey(key))
        //        {
        //            List<CacheModel> a = (List<CacheModel>)_cache[key];
        //            a.Remove(a.Find(m => m.k == value.k));
        //            a.Add(value);
        //            _cache.Remove(key);
        //            _cache.Add(key,a);
        //        }
        //    }
        //}

        /// <summary>
        /// Remove an object type from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        public static void Remove<T>()
        {
            Type type = typeof(T);
            lock (_sync)
            {
                if (_cache.ContainsKey(type.Name) == false)
                    throw new ApplicationException(String.Format("An object of type '{0}' does not exists in cache", type.Name));
                lock (_sync)
                {
                    _cache.Remove(type.Name);
                }
            }
        }

        /// <summary>
        /// Remove an object stored with a key from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of the object</param>
        public static void Remove<T>(string key)
        {
            Type type = typeof(T);
            lock (_sync)
            {
                if (_cache.ContainsKey(key + type.Name) == false)
                    throw new ApplicationException(String.Format("An object with key '{0}' does not exists in cache", key));

                lock (_sync)
                {
                    _cache.Remove(key + type.Name);
                }
            }
        }

        public static void Remove<T>(string key, T value) where T : class
        {
            lock (_sync)
            {
                if (_cache.ContainsKey(key))
                {
                    List<T> a = (List<T>)_cache[key];
                    a.Remove(a.Find(m => m.GetType().GetProperty("k").GetValue(m) == value.GetType().GetProperty("k").GetValue(value)));
                    _cache.Remove(key);
                    lock (_sync)
                    {
                        _cache.Add(key, a);
                    }
                }
            }
        }

        //public static void RemoveCanHo(string key, CacheCanHo value)
        //{
        //    lock (_sync)
        //    {
        //        if (_cache.ContainsKey(key))
        //        {
        //            List<CacheCanHo> a = (List<CacheCanHo>)_cache[key];
        //            a.Remove(a.Find(m => m.k == value.k));
        //            _cache.Remove(key);
        //            lock (_sync)
        //            {
        //                _cache.Add(key, a);
        //            }
        //        }
        //    }
        //}
    }
}