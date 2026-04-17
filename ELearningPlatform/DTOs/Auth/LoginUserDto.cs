using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Auth;

public record LoginUserDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);