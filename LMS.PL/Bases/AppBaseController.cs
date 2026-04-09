using LMS.Core;
using LMS.Data_;
using LMS.Infrastructure.Caching.Redis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LMS.API.Bases
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppBaseController : ControllerBase
    {
        private IMediator _mediatorInstance;

        private ICacheService _redisCacheService;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ICacheService RedisCacheService =>
            _redisCacheService ??= HttpContext.RequestServices.GetService<ICacheService>();

        protected ICurrentUserService CurrentUserService => HttpContext.RequestServices.GetService<ICurrentUserService>();
        #region Actions
        public ObjectResult NewResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }
        #endregion


    }
}
