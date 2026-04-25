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
        public static ResponseModelDto<T> Success(T data)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static ResponseModelDto<T> Success()
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true
            };
        }

        public static ResponseModelDto<T> Failure(List<string> failMessages)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessages = failMessages
            };
        }

        public static ResponseModelDto<T> Failure(string failMessage)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessages = new List<string> { failMessage }
            };
        }
    }
}
