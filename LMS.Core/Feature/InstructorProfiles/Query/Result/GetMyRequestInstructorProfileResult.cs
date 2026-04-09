namespace LMS.Core.Feature.InstructorProfiles.Query.Result
{
    public class GetMyRequestInstructorProfileResult
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public decimal Rating { get; set; }
        public string StatusOfInstructorProfile { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string? ReasonOfReject { get; set; }
        public string Email { get; set; }

    }
}
