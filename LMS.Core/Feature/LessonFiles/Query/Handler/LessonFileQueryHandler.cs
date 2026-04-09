using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.LessonFiles.Query.Models;
using LMS.Core.Feature.LessonFiles.Query.Result;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.LessonFiles.Query.Handler
{
    public class LessonFileQueryHandler : ResponseHandler,
        IRequestHandler<GetAllFilesByLessonIdQuery, Response<List<AllLessonFileResult>>>
        , IRequestHandler<GetFileByIdQuery, Response<GetFileByIdResult>>
    {
        private readonly ILessonFilesService lessonFilesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IMapper mapper;

        public LessonFileQueryHandler(ILessonFilesService lessonFilesService, IStringLocalizer<SharedResources> stringLocalizer, IMapper mapper) : base(stringLocalizer)
        {
            this.lessonFilesService = lessonFilesService;
            this.stringLocalizer = stringLocalizer;
            this.mapper = mapper;
        }

        public async Task<Response<List<AllLessonFileResult>>> Handle(GetAllFilesByLessonIdQuery request, CancellationToken cancellationToken)
        {


            var ListOfFiles = lessonFilesService.GetAllLessonsFileByLessonsId(request.Id);
            var result = mapper.Map<List<AllLessonFileResult>>(ListOfFiles);
            return Success(result);


        }

        public async Task<Response<GetFileByIdResult>> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return (BadRequest<GetFileByIdResult>(stringLocalizer["InvalidId"]));
            }
            var file = await lessonFilesService.GetByIdAsync(request.Id);
            if (file == null)
            {
                return NotFound<GetFileByIdResult>(stringLocalizer["FileNotFound"]);
            }
            var result = mapper.Map<GetFileByIdResult>(file);
            return Success(result);


        }
    }
}
