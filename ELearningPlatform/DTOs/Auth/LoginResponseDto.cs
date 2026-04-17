namespace ELearningPlatform.DTOs.Auth;

public record LoginResponseDto(
    int UserId,
    string FullName,
    string Email,
    string Token
);