using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.LessonFiles.Command.Handler
{
    public class IncreaseNumberOfFileForLessonTableHandler : INotificationHandler<IncreaseNumberOfFileForLessonTable>
    {
        private readonly ILessonsService lessonsService;

        public IncreaseNumberOfFileForLessonTableHandler(ILessonsService lessonsService)
        {
            this.lessonsService = lessonsService;
        }
        public async Task Handle(IncreaseNumberOfFileForLessonTable notification, CancellationToken cancellationToken)
        {
            var Lesson = await lessonsService.GetLessonsById(notification.LessonId);

            Lesson.NumberOfFiles++;
            await lessonsService.UpdateWithoutFile(Lesson);

        }
    }
    public class IncreaseNumberOfFileForLessonTable : INotification
    {
        public int LessonId { get; set; }
        public IncreaseNumberOfFileForLessonTable(int Lesson)
        {
            this.LessonId = Lesson;

        }

    }
}
