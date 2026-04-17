using AutoMapper;
using ELearningPlatform.Data;
using ELearningPlatform.DTOs.Results;
using ELearningPlatform.Entities;
using ELearningPlatform.Mapping;
using ELearningPlatform.Repositories;
using ELearningPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ELearningPlatform.Tests;

public class QuizScoringTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly IQuizService _quizService;

    public QuizScoringTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("QuizScoreDb_" + Guid.NewGuid())
            .Options;
        _db = new ApplicationDbContext(options);

        // Seed
        _db.Users.Add(new User { UserId = 1, FullName = "Stu",
            Email = "s@t.com", PasswordHash = "h" });
        _db.Courses.Add(new Course { CourseId = 1, Title = "C",
            CreatedBy = 1, CreatedAt = DateTime.UtcNow });
        _db.Quizzes.Add(new Quiz { QuizId = 1, CourseId = 1, Title = "Q1" });
        _db.SaveChanges();

        var config  = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        var mapper  = config.CreateMapper();
        var qRepo   = new QuizRepository(_db);
        var uRepo   = new UserRepository(_db);
        _quizService = new QuizService(qRepo, uRepo, mapper);
    }

    [Fact]
    public async Task Submit_ValidQuiz_StoresResult()
    {
        var dto    = new SubmitQuizDto(1, 85);
        var result = await _quizService.SubmitQuizAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal(85, result.Score);
        Assert.Equal(1,  result.UserId);
        Assert.Equal(1,  result.QuizId);
    }

    [Fact]
    public async Task Submit_ScoreZero_Allowed()
    {
        var result = await _quizService.SubmitQuizAsync(1, new SubmitQuizDto(1, 0));
        Assert.Equal(0, result.Score);
    }

    [Fact]
    public async Task Submit_Score100_Allowed()
    {
        var result = await _quizService.SubmitQuizAsync(1, new SubmitQuizDto(1, 100));
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public async Task GetResults_ReturnsUserResults()
    {
        await _quizService.SubmitQuizAsync(1, new SubmitQuizDto(1, 70));
        await _quizService.SubmitQuizAsync(1, new SubmitQuizDto(1, 90));

        var results = await _quizService.GetResultsAsync(1);
        Assert.Equal(2, results.Count());
    }

    public void Dispose() => _db.Dispose();
}