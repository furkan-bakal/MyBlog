using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core
{
    public struct NoContent;

    public record ResponseModelDto<T>
    {
        [JsonIgnore] public bool IsSuccess { get; init; }
        public List<string>? FailMessages { get; init; }
        public T? Data { get; init; }
        [JsonIgnore] public HttpStatusCode StatusCode { get; set; }

        // static factory methods for success and failure responses
        public static ResponseModelDto<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ResponseModelDto<T> Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true,
                StatusCode = statusCode
            };
        }

        public static ResponseModelDto<T> Failure(List<string> failMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessages = failMessages,
                StatusCode = statusCode
            };
        }

        public static ResponseModelDto<T> Failure(string failMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessages = new List<string> { failMessage },
                StatusCode = statusCode
            };
        }
    }
}
