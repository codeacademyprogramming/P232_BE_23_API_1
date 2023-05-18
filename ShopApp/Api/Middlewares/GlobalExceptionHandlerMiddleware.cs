using Newtonsoft.Json;
using Service.Exceptions;
using System.Net;

namespace Api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case NotFoundException exp:
                        response.StatusCode = 404;
                        break;
                    case EntityDublicateException exp:
                        response.StatusCode = 400;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonConvert.SerializeObject(new { message = error.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
