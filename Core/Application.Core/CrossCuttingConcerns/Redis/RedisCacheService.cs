using Application.Core.Utilities.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IDistributedCache redisCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _redisCache = redisCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public void Add(string key, object value, int duration = 60)
        {
            byte[] objByte = ToByteArray(value);

            _redisCache.Set(key, objByte, GetOptions(duration));
        }

        public void Add<T>(string key, T value, int duration = 60)
        {
            string cacheData = EntityDumperHelper.Dump<T>(value);

            _redisCache.SetString(key, cacheData, GetOptions(duration));
        }

        public T Get<T>(string key)
        {
            string redisData = _redisCache.GetString(key);


            return EntityDumperHelper.LoadBack<T>(redisData);
        }

        public object Get(string key, bool fromString = false)
        {
            if (!fromString)
            {
                byte[] redisData = _redisCache.Get(key);

                return FromByteArray(redisData);
            }
            else
            {
                string jsonData = _redisCache.GetString(key);

                return jsonData;
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            byte[] redisData = await _redisCache.GetAsync(key);

            return (T)FromByteArray(redisData);
        }

        public async Task<object> GetAsync(string key)
        {
            byte[] redisData = await _redisCache.GetAsync(key);

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
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            IEnumerable<string> keys = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First()).Keys().Select(s => s.ToString());

            List<string> keysToRemove = keys.Where(d => regex.IsMatch(d)).Select(d => d).ToList();

            foreach (var key in keysToRemove)
            {
                _redisCache.Remove(key.ToString());
            }

            #region OldCode
            //var cacheEntriesCollectionDefinition = _redisCache.GetType().GetProperty("db0", BindingFlags.NonPublic | BindingFlags.Instance);
            ////var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_redisCache) as dynamic;
            //List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            //foreach (var cacheItem in cacheEntriesCollection)
            //{
            //    ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
            //    cacheCollectionValues.Add(cacheItemValue);
            //}
            #endregion
        }

        private DistributedCacheEntryOptions GetOptions(int duration = 60)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();

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
