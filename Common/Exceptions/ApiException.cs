using Common.Extensions;
using System;
using System.Net;

namespace Common
{
    public class ApiException : ApplicationException
    {
        private ApiExeptionType apiExeptionType;
        private string errorMessage;
        public HttpStatusCode StatusCode { get { return apiExeptionType.GetAttribute<ErrorAttribute>().StatusCode; } }
        public string ErrorCode { get => apiExeptionType.GetAttribute<ErrorAttribute>().Code; }
        public string ErrorMessage
        {
            get { return string.IsNullOrEmpty(errorMessage) ? apiExeptionType.GetAttribute<ErrorAttribute>().Message : errorMessage; }
            set { errorMessage = value; }
        }
        public ErrorResult[] Errors { get; private set; }
        public ApiException(ApiExeptionType exceptionType)
        {
            apiExeptionType = exceptionType;
        }
        public ApiException(ApiExeptionType exceptionType, string message)
        {
            ErrorMessage = message;
            apiExeptionType = exceptionType;
        }
        public ApiException(ApiExeptionType exceptionType, ErrorResult[] errors)
        {
            Errors = errors;
            apiExeptionType = exceptionType;
        }
    }
}
