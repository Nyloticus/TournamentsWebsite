using Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Common
{
    public class Result
    {
        public Result()
        {
        }

        internal Result(bool success, ApiExeptionType code, object payload = null, string message = default)
        {
            Success = success;
            Payload = payload;
            Code = code;
            Message = GetMessage(message);
        }


        internal Result(bool success, ApiExeptionType code, string message = default,
            IEnumerable<ErrorResult> errors = default, string errorStrings = default)
        {
            Success = success;
            Code = code;
            Errors = errors?.ToArray();
            Errors_String = errorStrings;
            Message = GetMessage(message);
        }

        private string GetMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
                return message;
            var attr = Code.GetAttribute<ErrorAttribute>();
            return attr.Message;
        }

        #region Properties

        public bool Success { get; set; }
        public ErrorResult[] Errors { get; set; } = new ErrorResult[] { };
        public string Errors_String { get; set; }
        public ApiExeptionType Code { get; set; }
        public object Payload { get; set; }
        public string Message { get; set; }

        #endregion


        public static Result Successed()
        {
            return new Result(true, ApiExeptionType.Ok, null);
        }

        public static Result Successed(object data, string message = default)
        {
            return new Result(true, ApiExeptionType.Ok, data, message);
        }

        public static Result Successed(object data, ApiExeptionType code, string message = default)
        {
            return new Result(true, code, data, message);
        }

        public static Result Successed<T>(PagedList<T> data)
        {
            // var items = new {
            //   Items = data,
            //   MetaDate = data.GetMetaData()
            // };

            return new Result(true, ApiExeptionType.Ok, data);
        }

        public static Result Failure(ApiExeptionType code, string message = default,
            IEnumerable<ErrorResult> errors = default, string errorStrings = default)
        {
            return new Result(false, code, message, errors, errorStrings);
        }

    }

    public class Result<T> : Result
    {
        public new T Payload { get; set; }
        public static Result<T> Successed(T data, string message = default)
        {
            return new Result<T> { Success = true, Code = ApiExeptionType.Ok, Payload = data, Message = message };
        }

        public static Result<T> Failure(ApiExeptionType code, string message = default,
            IEnumerable<ErrorResult> errors = default, string errorStrings = default)
        {
            return new Result<T> { Success = false, Code = code, Message = message, Errors = errors?.ToArray(), Errors_String = errorStrings };
        }

    }

}