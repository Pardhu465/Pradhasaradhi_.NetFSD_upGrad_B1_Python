namespace ELearningPlatform.DTOs.Courses;

public record CourseResponseDto(
    int CourseId,
    string Title,
    string? Description,
    int CreatedBy,
    DateTime CreatedAt
);