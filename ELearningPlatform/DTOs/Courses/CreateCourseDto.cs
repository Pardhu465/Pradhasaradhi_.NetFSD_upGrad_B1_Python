using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Courses;

public record CreateCourseDto(
    [Required][StringLength(200)] string Title,
    string? Description,
    [Required] int CreatedBy
);