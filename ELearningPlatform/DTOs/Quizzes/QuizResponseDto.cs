namespace ELearningPlatform.DTOs.Quizzes;

public record QuizResponseDto(
    int QuizId,
    int CourseId,
    string Title
);