using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Courses;

public record UpdateCourseDto(
    [Required][StringLength(200)] string Title,
    string? Description
);