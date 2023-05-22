using Api.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                var responseModel = new ErrorResponseModel();
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case RestException exp:
                        response.StatusCode = (int)exp.Code;
                        responseModel.Errors = exp.Errors;
                        responseModel.Message = exp.Message;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = error.Message;
                        break;
                }

                var result = JsonConvert.SerializeObject(responseModel, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
                });
                await response.WriteAsync(result);
            }
        }
    }
}
