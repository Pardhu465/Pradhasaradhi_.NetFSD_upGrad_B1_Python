using AutoMapper;
using ELearningPlatform.DTOs.Auth;
using ELearningPlatform.Entities;
using ELearningPlatform.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper   = mapper;
    }

    public async Task<(bool success, string message)> RegisterAsync(RegisterUserDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing != null)
            return (false, "Email already registered.");

        var user = new User
        {
            FullName     = dto.FullName,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt    = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user);
        return (true, "Registered successfully.");
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null) return null;

        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return null;

        return new LoginResponseDto(user.UserId, user.FullName, user.Email, "");
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return null;
        user.FullName = dto.FullName;
        await _userRepo.UpdateAsync(user);
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<IEnumerable<int>> GetCompletedCourseIdsAsync(int userId)
    {
        var completions = await _userRepo.GetCourseCompletionsByUserIdAsync(userId);
        return completions == null ? new int[0] : completions.Select(cc => cc.CourseId);
    }

    public async Task<int> GetCompletedCourseCountAsync(int userId)
    {
        var completions = await _userRepo.GetCourseCompletionsByUserIdAsync(userId);
        return completions?.Count() ?? 0;
    }

    public async Task<bool> AddCourseCompletionAsync(int userId, int courseId)
    {
        var exists = await _userRepo.GetCourseCompletionAsync(userId, courseId);
        if (exists != null) return false;

        var completion = new CourseCompletion
        {
            UserId = userId,
            CourseId = courseId,
            CompletedAt = DateTime.UtcNow
        };

        await _userRepo.AddCourseCompletionAsync(completion);
        return true;
    }
}