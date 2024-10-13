using System.Text.Json.Serialization;
using ToDosProject.Domain.Constants;

namespace ToDosProject.Domain.DTOs
{
    public class Response<TData>
    {
        public int Code { get; set; } = AppConfiguration.DefaultStatusCode;

        [JsonConstructor]
        public Response() => Code = AppConfiguration.DefaultStatusCode;
        public Response(
            TData? data,
            int code = AppConfiguration.DefaultStatusCode,
            string? message = null)
        {
            Data = data;
            Code = code;
            Message = message;
        }

        public Response(
            int code = AppConfiguration.DefaultStatusCode,
            string? message = null)
        {
            Code = code;
            Message = message;
        }
        public TData? Data { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess => Code >= 200 && Code <= 299;
    }
}
