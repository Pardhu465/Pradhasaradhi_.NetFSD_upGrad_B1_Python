namespace ELearningPlatform.DTOs.Results;

public record ResultResponseDto(
    int ResultId,
    int UserId,
    int QuizId,
    int Score,
    DateTime AttemptDate
);