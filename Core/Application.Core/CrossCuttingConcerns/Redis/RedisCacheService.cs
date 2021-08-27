using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Core.CrossCuttingConcerns.Redis
{
    public class RedisCacheService : ICacheService
    {
        private IDistributedCache _redisCache;

        public RedisCacheService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public void Add(string key, object value, int duration)
        {
            var objByte = ToByteArray(value);
            _redisCache.Set(key, objByte, GetOptions(duration));
            //string jsonData = JsonConvert.SerializeObject(value);
            //_redisCache.SetString(key, jsonData, GetOptions(duration));
        }

        public T Get<T>(string key)
        {
            var jsonObject = _redisCache.GetString(key);

            T deserializeObject = (T)JsonConvert.DeserializeObject(jsonObject, typeof(T));
            return deserializeObject;
        }

        public string Get(string key)
        {
            var jsonData = _redisCache.GetString(key);
            return jsonData;
        }

        public object GetObject(string key)
        {
            var redisData = _redisCache.Get(key);
            //var jsonModel = JsonConvert.DeserializeObject(jsonData);
            return FromByteArray(redisData);
        }

        public async Task<object> GetObjectAsync(string key)
        {
            var redisData = await _redisCache.GetAsync(key);
            //var jsonModel = JsonConvert.DeserializeObject(jsonData);
            return FromByteArray(redisData);
        }

        public bool IsAdd(string key)
        {
            string jsonData = _redisCache.GetString(key);
            return !string.IsNullOrEmpty(jsonData);
        }

        public void Remove(string key)
        {
            _redisCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var cacheEntriesCollectionDefinition = _redisCache.GetType().GetProperty("db0", BindingFlags.NonPublic | BindingFlags.Instance);
            //var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_redisCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _redisCache.Remove(key.ToString());
            }
        }

        private DistributedCacheEntryOptions GetOptions(int duration = 60)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromSeconds(duration));
            return options;
        }

        private byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        private object FromByteArray(byte[] data)
        {
            if (data == null)
                return default(object);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                var obj = bf.Deserialize(ms);

                return obj;
            }
        }
    }
}
