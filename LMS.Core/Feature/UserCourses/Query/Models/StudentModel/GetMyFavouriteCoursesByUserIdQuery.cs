using LMS.Core.Feature.UserCourses.Query.Result;
using MediatR;

namespace LMS.Core.Feature.UserCourses.Query.Models.StudentModel
{
    public record GetMyFavouriteCoursesByUserIdQuery : IRequest<Response<List<GetMyFavouriteCoursesByUserIdResult>>>;

}
