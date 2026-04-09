namespace LMS.Core.Feature.InstructorProfiles.Query.Result
{
    public class GetInstructorByUserIdProfileResult
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }

        public decimal Rating { get; set; }
        public string Email { get; set; }
        public string LinkedInUrl { get; set; }
        public string StatusOfInstructor { get; set; }
        public string? ReasonOfRejected { get; set; }



    }
}
