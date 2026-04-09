using LMS.Core.Feature.Lessons.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Lessons.Query.Models
{
    public record GetLessonByIdQuery(int Id) : IRequest<Response<GetLessonByIdResult>>;

}
