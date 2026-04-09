namespace LMS.Core.Feature.LessonFiles.Query.Result
{
    public class AllLessonFileResult
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateOnly UploadAt { get; set; }
    }
}
