using ELearningPlatform.DTOs.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public interface IUserService
{
    Task<(bool success, string message)> RegisterAsync(RegisterUserDto dto);
    Task<LoginResponseDto?>              LoginAsync(LoginUserDto dto);
    Task<UserResponseDto?>               GetByIdAsync(int id);
    Task<UserResponseDto?>               UpdateAsync(int id, UpdateUserDto dto);
    Task<IEnumerable<int>>               GetCompletedCourseIdsAsync(int userId);
    Task<int>                           GetCompletedCourseCountAsync(int userId);
    Task<bool>                          AddCourseCompletionAsync(int userId, int courseId);
}