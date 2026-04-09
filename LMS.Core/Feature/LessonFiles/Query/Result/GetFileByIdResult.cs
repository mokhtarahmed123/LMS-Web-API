namespace LMS.Core.Feature.LessonFiles.Query.Result
{
    public class GetFileByIdResult
    {
        public int Number { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string LessonName { get; set; }
        public DateOnly UploadAt { get; set; }
        public int LessonNumber { get; set; }
    }
}
