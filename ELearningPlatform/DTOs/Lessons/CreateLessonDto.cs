using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Lessons;

public record CreateLessonDto(
    [Required] int CourseId,
    [Required][StringLength(200)] string Title,
    string? Content,
    int OrderIndex = 1
);