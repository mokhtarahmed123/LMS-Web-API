using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Bases
{
    public class ResponseHandler
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public ResponseHandler(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }
        public ResponseHandler()
        {

        }
        public Response<T> Deleted<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrEmpty(message) ? stringLocalizer[SharedResourcesKeys.Deleted] : message
            };
        }
        public Response<T> Success<T>(T? entity, object? Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = stringLocalizer[SharedResourcesKeys.Retrieved],
                Meta = Meta != null ? Meta : ""
            };
        }
        public Response<T> Unauthorized<T>()
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = stringLocalizer[SharedResourcesKeys.UnAuthorized]
            };
        }
        public Response<T> Updated<T>(string? message = null, string name = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrEmpty(message) ? stringLocalizer[SharedResourcesKeys.Updated] : message

            };
        }
        public Response<T> BadRequest<T>(string? Message = null, object? value = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message
            };
        }
        public Response<T> ServerError<T>(string? Message = null, object? value = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Succeeded = false,
                Message = Message == null ? "Internal Server Error" : Message
            };
        }

        public Response<T> NotFound<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.NotFound] : message
            };
        }

        public Response<T> Created<T>(T entity, object? Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = stringLocalizer[SharedResourcesKeys.Created],

                Meta = Meta
            };
        }
    }
}
