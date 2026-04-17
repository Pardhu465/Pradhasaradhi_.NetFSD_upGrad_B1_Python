using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Quizzes;

public record CreateQuizDto(
    [Required] int CourseId,
    [Required][StringLength(200)] string Title
);