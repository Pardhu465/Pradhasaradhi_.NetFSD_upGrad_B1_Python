using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Results;

public record SubmitQuizDto(
    [Required] int UserId,
    [Required][Range(0,100)] int Score
);