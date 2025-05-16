using System.Data;
using System.Text.Json;

namespace ToDoList.Helpers
{
    public static class ResponseHelper
    {
        public static string CreateSuccessResponse(string message, object? data = null)
        {
            var response = new Response
            {
                Status = "Success",
                Message = message,
                Data = data
            };

            return JsonSerializer.Serialize(response);
        }

        public static string CreateErrorResponse(string message, object? data = null)
        {
            var response = new Response
            {
                Status = "Error",
                Message = message,
                Data = data
            };

            return JsonSerializer.Serialize(response);
        }

        public static string CreateResponse(string status, string message, object? data = null)
        {
            var response = new Response
            {
                Status = status,
                Message = message,
                Data = data
            };

            return JsonSerializer.Serialize(response);
        }

        public class Response
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
            public object? Data { get; set; }
        }
    }
}