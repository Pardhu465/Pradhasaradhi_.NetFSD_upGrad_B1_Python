
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

public class ExceptionHandlingTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly IQuizService _quizService;

    public ExceptionHandlingTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ExcDb_" + Guid.NewGuid())
            .Options;
        _db = new ApplicationDbContext(options);
        _db.Users.Add(new User { UserId=1, FullName="U",
            Email="u@t.com", PasswordHash="h" });
        _db.SaveChanges();

        var config   = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        var mapper   = config.CreateMapper();
        var qRepo    = new QuizRepository(_db);
        var uRepo    = new UserRepository(_db);
        _quizService = new QuizService(qRepo, uRepo, mapper);
    }

    [Fact]
    public async Task SubmitQuiz_InvalidQuizId_ThrowsKeyNotFoundException()
    {
        var dto = new SubmitQuizDto(1, 50);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _quizService.SubmitQuizAsync(9999, dto));
    }

    [Fact]
    public async Task GetQuestions_NonExistentQuiz_ReturnsEmpty()
    {
        var questions = await _quizService.GetQuestionsAsync(9999);
        Assert.Empty(questions);
    }

    [Fact]
    public async Task GetResults_UserWithNoResults_ReturnsEmpty()
    {
        var results = await _quizService.GetResultsAsync(9999);
        Assert.Empty(results);
    }

    [Fact]
    public async Task GetByCourse_NoCourseQuizzes_ReturnsEmpty()
    {
        var quizzes = await _quizService.GetByCourseIdAsync(9999);
        Assert.Empty(quizzes);
    }

    public void Dispose() => _db.Dispose();
}