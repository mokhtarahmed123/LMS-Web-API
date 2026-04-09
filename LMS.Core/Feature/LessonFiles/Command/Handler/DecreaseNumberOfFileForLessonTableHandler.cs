using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.LessonFiles.Command.Handler
{
    public class DecreaseNumberOfFileForLessonTableHandler : INotificationHandler<DecreaseNumberOfFileForLessonTable>
    {
        private readonly ILessonsService lessonsService;

        public DecreaseNumberOfFileForLessonTableHandler(ILessonsService lessonsService)
        {
            this.lessonsService = lessonsService;
        }
        public async Task Handle(DecreaseNumberOfFileForLessonTable notification, CancellationToken cancellationToken)
        {
            var Lesson = await lessonsService.GetLessonsById(notification.LessonId);

            Lesson.NumberOfFiles--;
            await lessonsService.Update(Lesson, null);
        }
    }

    public class DecreaseNumberOfFileForLessonTable : INotification
    {
        public int LessonId { get; set; }
        public DecreaseNumberOfFileForLessonTable(int Lesson)
        {
            this.LessonId = Lesson;

        }

    }
}
