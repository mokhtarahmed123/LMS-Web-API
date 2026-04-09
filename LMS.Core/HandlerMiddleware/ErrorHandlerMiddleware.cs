using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace LMS.Core.HandlerMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>()
                {
                    Succeeded = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };

                switch (ex)
                {
                    case UnauthorizedAccessException:
                        responseModel.StatusCode = HttpStatusCode.Unauthorized;
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case ValidationException validationEx:
                        // FluentValidation errors
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = validationEx.Errors.Count() > 0
                            ? validationEx.Errors.First().ErrorMessage
                            : validationEx.Message;
                        break;

                    case KeyNotFoundException:
                        responseModel.StatusCode = HttpStatusCode.NotFound;
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case ForbiddenContext:
                        responseModel.StatusCode = HttpStatusCode.Forbidden;
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;

                    case DbUpdateException dbEx:
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = dbEx.Message;
                        break;

                    default:
                        // Any other unhandled exceptions
                        responseModel.StatusCode = HttpStatusCode.InternalServerError;
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        if (ex.InnerException != null)
                        {
                            responseModel.Message += "\n" + ex.InnerException.Message;
                        }
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
