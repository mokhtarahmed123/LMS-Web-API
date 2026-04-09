namespace LMS.Core.Feature.InstructorProfiles.Query.Result
{
    public class GetAllInstructorProfilesPaginatedResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string LinkedinUrl { get; set; }

        public string StatusOfInstructorProfile { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string? ReasonOfReject { get; set; }

        public GetAllInstructorProfilesPaginatedResult
            (int id, string name, string bio, string profilepictureurl,
            string linkedinurl, string statusOfInstructorProfile,
            DateOnly createdat, string reasonOfReject)
        {
            Id = id;
            Name = name;
            Bio = bio;
            ProfilePictureUrl = profilepictureurl;
            LinkedinUrl = linkedinurl;

            StatusOfInstructorProfile = statusOfInstructorProfile;
            CreatedAt = createdat;
            ReasonOfReject = reasonOfReject;

        }

    }
}
