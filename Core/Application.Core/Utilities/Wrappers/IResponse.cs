using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Wrappers
{
    public interface IResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
