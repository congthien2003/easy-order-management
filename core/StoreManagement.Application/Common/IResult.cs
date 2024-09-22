using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Application.Common
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }

    public interface IResult<T> : IResult
    {
        T Data { get; }
    }

    public class Result : IResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string message = "")
        {
            return new Result(true, message);
        }

        public static Result Failure(string message)
        {
            return new Result(false, message);
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; private set; }

        private Result(bool isSuccess, T data, string message)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static Result<T> Success(T data, string message = "")
        {
            return new Result<T>(true, data, message);
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>(false, default, message);
        }
    }
}
