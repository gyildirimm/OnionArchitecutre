using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.CrossCuttingConcerns
{
    public interface ICacheService
    {
        void Add(string key, object value, int duration = 60);

        void Add<T>(string key, T value, int duration = 60);

        T Get<T>(string key);

        object Get(string key, bool fromString = false);

        Task<T> GetAsync<T>(string key);

        Task<object> GetAsync(string key);

        bool IsAdd(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);
    }
}
