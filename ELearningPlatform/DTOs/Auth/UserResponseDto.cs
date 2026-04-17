namespace ELearningPlatform.DTOs.Auth;

public record UserResponseDto(
    int UserId,
    string FullName,
    string Email,
    DateTime CreatedAt
);