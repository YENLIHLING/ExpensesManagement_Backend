using Microsoft.AspNetCore.Mvc;
using WepAPI.Responses;

namespace WepAPI.Common
{
    public class ErrorHandling
    {
        public static IActionResult Error(int statusCode, string message)
        {
            return new JsonResult(new ErrorResponse { Message = message })
            {
                StatusCode = statusCode
            };
        }
    }
}
