using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Auth;

public record UpdateUserDto(
    [Required][StringLength(150)] string FullName
);