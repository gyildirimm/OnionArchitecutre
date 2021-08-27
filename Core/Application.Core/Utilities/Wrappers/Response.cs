using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Wrappers
{
    [Serializable()]
    public class Response<T> : IResponse<T>
    {
        public Response()
        {

        }

        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }

        //private string type { get; set; }

        public static Response<T> Error(List<string> errorList)
        {
            return new Response<T>() { Data = default(T), Errors = errorList, IsSuccess = false };
        }

        public static Response<T> Error(List<string> errorList, string message)
        {
            return new Response<T>() { Data = default(T), Errors = errorList, IsSuccess = false, Message = message };
        }

        public static Response<T> Error(string error)
        {
            return new Response<T>() { Data = default(T), Errors = new List<string>() { error }, IsSuccess = false };
        }

        public static Response<T> Error()
        {
            return new Response<T>() { Data = default(T), Errors = new List<string>(), IsSuccess = false };
        }

        public static Response<T> Success(T data)
        {
            return new Response<T>() { Data = data, Errors = new List<string>() { }, IsSuccess = true };
        }

        public static Response<T> Success(T data, string message)
        {
            return new Response<T>() { Data = data, Errors = new List<string>() { }, IsSuccess = true, Message = message };
        }

        public static Response<T> Success()
        {
            return new Response<T>() { Data = default(T), Errors = new List<string>() { }, IsSuccess = true };
        }

        public static Response<T> Success(string message)
        {
            return new Response<T>() { Data = default(T), Errors = new List<string>() { }, IsSuccess = true, Message = message };
        }
    }
}
