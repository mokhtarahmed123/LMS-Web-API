using LMS.Data_.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class InstructorProfiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(1500)]
        public string? Bio { get; set; }

        [StringLength(300)]
        public string? ProfilePictureUrl { get; set; }

        [StringLength(250)]
        [Url]
        public string? LinkedInUrl { get; set; }



        public StatusOfInstructorProfileEnum StatusOfInstructorProfile { get; set; } = StatusOfInstructorProfileEnum.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReasonOfReject { get; set; }


        // -------------------- Relation --------------------

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;
    }
}
