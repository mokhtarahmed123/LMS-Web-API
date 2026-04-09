using LMS.Data_.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;


        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;



        public DateTime? CompletionDate { get; set; }

        public float Progress { get; set; } = 0;


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Courses Course { get; set; }
    }
}
