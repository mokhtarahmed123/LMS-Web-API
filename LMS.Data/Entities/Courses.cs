using LMS.Data_.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities;

public class Courses
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Course title is required")]
    [StringLength(150, MinimumLength = 5)]
    public string Title { get; set; } = null!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal DurationHours { get; set; } = 0;


    public int NumberOfRatings { get; set; } = 0;

    [Required]
    public CoursesLevelEnum Level { get; set; } = CoursesLevelEnum.Beginner;

    [Required]
    public CoursesLanguageEnum CourseLanguage { get; set; } = CoursesLanguageEnum.English;

    [Required]
    [StringLength(300)]
    public string ThumbnailUrl { get; set; } = null!;  // Photo Of the course


    public int NumberOfEnrolledStudents { get; set; } = 0;

    public int NumberOfLessons { get; set; } = 0;
    [Precision(18, 2)]


    public decimal AverageRating { get; set; } = 0;

    public CourseStatusEnum CourseStatus { get; set; } = CourseStatusEnum.Pending;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? ReasonOfReject { get; set; }

    // -------------------- Relations --------------------

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Categories Category { get; set; } = null!;

    [Required]
    public int InstructorProfileId { get; set; }

    [ForeignKey(nameof(InstructorProfileId))]
    public InstructorProfiles InstructorProfile { get; set; } = null!;






}
