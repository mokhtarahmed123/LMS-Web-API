using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class Lessons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(500)]
        public string VideoUrl { get; set; } = null!;

        [Required]
        [Precision(18, 2)]
        public decimal DurationMinutes { get; set; }

        [Required]
        [Range(1, 1000)]
        public int OrderNumber { get; set; }

        public bool IsPreview { get; set; } = false;

        public int NumberOfFiles { get; set; } = 0;


        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // -------------------- Relation --------------------

        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Courses Course { get; set; } = null!;

    }
}
