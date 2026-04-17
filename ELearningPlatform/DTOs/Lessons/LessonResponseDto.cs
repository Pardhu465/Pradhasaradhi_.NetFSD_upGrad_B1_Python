namespace ELearningPlatform.DTOs.Lessons;

public record LessonResponseDto(
    int LessonId,
    int CourseId,
    string Title,
    string? Content,
    int OrderIndex
);