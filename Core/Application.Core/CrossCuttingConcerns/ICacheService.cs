using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.CrossCuttingConcerns
{
    public interface ICacheService
    {
        T Get<T>(string key);

        string Get(string key);
        
        object GetObject(string key);

        Task<object> GetObjectAsync(string key);

        void Add(string key, object value, int duration);

        bool IsAdd(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);
    }
}
