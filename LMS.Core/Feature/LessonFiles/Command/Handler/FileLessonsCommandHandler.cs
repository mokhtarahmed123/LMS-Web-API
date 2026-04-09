using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.LessonFiles.Command.Models;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.LessonFiles.Command.Handler
{
    public class FileLessonsCommandHandler : ResponseHandler, IRequestHandler<AddLessonsFileCommand, Response<string>>
        , IRequestHandler<EditLessonsFileCommand, Response<string>>
        , IRequestHandler<DeleteLessonsFileCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IMapper mapper;
        private readonly ILessonFilesService lessonFilesService;
        private readonly ICurrentUserService currentUserService;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ILessonsService lessonsService;
        private readonly IMediator mediator;

        public FileLessonsCommandHandler(IStringLocalizer<SharedResources> stringLocalizer, IMapper mapper, ILessonFilesService lessonFilesService, ICurrentUserService currentUserService, IInstructorProfilesService instructorProfilesService, ILessonsService lessonsService, IMediator mediator) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.mapper = mapper;
            this.lessonFilesService = lessonFilesService;
            this.currentUserService = currentUserService;
            this.instructorProfilesService = instructorProfilesService;
            this.lessonsService = lessonsService;
            this.mediator = mediator;
        }









        public async Task<Response<string>> Handle(AddLessonsFileCommand request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;
            var Instructor = await IfInstructorIsFoundAsync(UserId);
            if (Instructor == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            var lesson = await IfLessonIsFoundAsync(request.LessonId);

            if (lesson.Course.InstructorProfileId != Instructor.Id) return BadRequest<string>("You Don't Have This Lesson ");

            var File = mapper.Map<LMS.Data_.Entities.LessonFiles>(request);
            var Adding = await lessonFilesService.Add(File, request.FileUrl);
            if (Adding == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
            // 
            await mediator.Publish(new IncreaseNumberOfFileForLessonTable(request.LessonId));

            return Success<string>(stringLocalizer[SharedResourcesKeys.Created]);





        }

        public async Task<Response<string>> Handle(EditLessonsFileCommand request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;
            var Instructor = await IfInstructorIsFoundAsync(UserId);
            if (Instructor == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            var lesson = await IfLessonIsFoundAsync(request.LessonId);
            if (lesson == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);


            if (lesson.Course.InstructorProfileId != Instructor.Id) return BadRequest<string>("You Don't Have This Lesson ");


            var OldFile = await IfFileIsFoundAsync(request.FileId);

            var File = mapper.Map(request, OldFile);
            var Editing = await lessonFilesService.UpdateWithOutFile(File);
            if (Editing == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>();



        }
        public async Task<Response<string>> Handle(DeleteLessonsFileCommand request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;
            var Instructor = await IfInstructorIsFoundAsync(UserId);
            if (Instructor == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            var File = await IfFileIsFoundAsync(request.Id);
            if (File == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            var lesson = await IfLessonIsFoundAsync(File.LessonId);

            if (lesson.Course.InstructorProfileId != Instructor.Id)
                return BadRequest<string>("You Don't Have This Lesson");

            await lessonFilesService.Delete(request.Id);
            lesson.NumberOfFiles--;
            await lessonsService.UpdateWithoutFile(lesson);

            return Deleted<string>();
        }






        private async Task<LMS.Data_.Entities.InstructorProfiles> IfInstructorIsFoundAsync(string UserId)
        {
            var Instructor = await instructorProfilesService.GetInstructorByUserId(UserId);
            return Instructor;
        }
        private async Task<LMS.Data_.Entities.Lessons> IfLessonIsFoundAsync(int LessonId)
        {
            var lesson = await lessonsService.GetLessonsById(LessonId);
            return lesson;

        }
        private async Task<LMS.Data_.Entities.LessonFiles> IfFileIsFoundAsync(int FileId)
        {
            var File = await lessonFilesService.GetByIdAsync(FileId);
            return File;
        }
    }
}