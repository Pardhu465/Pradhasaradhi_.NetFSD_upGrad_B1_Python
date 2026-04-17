using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Auth;

public record RegisterUserDto(
    [Required][StringLength(150)] string FullName,
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password
);