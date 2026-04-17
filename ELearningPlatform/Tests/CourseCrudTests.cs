using AutoMapper;
using ELearningPlatform.Data;
using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.Entities;
using ELearningPlatform.Mapping;
using ELearningPlatform.Repositories;
using ELearningPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ELearningPlatform.Tests;

public class CourseCrudTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly ICourseService _service;

    public CourseCrudTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestCourseDb_" + Guid.NewGuid())
            .Options;
        _db = new ApplicationDbContext(options);

        // Seed a user (needed for FK)
        _db.Users.Add(new User { UserId = 1, FullName = "Admin",
            Email = "admin@test.com", PasswordHash = "hash" });
        _db.SaveChanges();

        var config  = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        var mapper  = config.CreateMapper();
        var cRepo   = new CourseRepository(_db);
        var lRepo   = new Repository<Lesson>(_db);
        _service    = new CourseService(cRepo, lRepo, mapper);
    }

    [Fact]
    public async Task Create_Course_ReturnsCreatedCourse()
    {
        var dto = new CreateCourseDto("Test Course", "Description", 1);
        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Test Course", result.Title);
        Assert.True(result.CourseId > 0);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCourses()
    {
        await _service.CreateAsync(new CreateCourseDto("C1", null, 1));
        await _service.CreateAsync(new CreateCourseDto("C2", null, 1));

        var all = await _service.GetAllAsync();
        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsCourse()
    {
        var created = await _service.CreateAsync(new CreateCourseDto("C3", null, 1));
        var found   = await _service.GetByIdAsync(created.CourseId);

        Assert.NotNull(found);
        Assert.Equal("C3", found!.Title);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(9999);
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_Course_ChangesTitle()
    {
        var created = await _service.CreateAsync(new CreateCourseDto("Old", null, 1));
        var updated = await _service.UpdateAsync(created.CourseId,
            new UpdateCourseDto("New Title", "Updated desc"));

        Assert.NotNull(updated);
        Assert.Equal("New Title", updated!.Title);
    }

    [Fact]
    public async Task Delete_Course_RemovesFromDb()
    {
        var created = await _service.CreateAsync(new CreateCourseDto("ToDelete", null, 1));
        var deleted = await _service.DeleteAsync(created.CourseId);

        Assert.True(deleted);
        Assert.Null(await _service.GetByIdAsync(created.CourseId));
    }

    [Fact]
    public async Task Delete_NonExisting_ReturnsFalse()
    {
        var result = await _service.DeleteAsync(9999);
        Assert.False(result);
    }

    public void Dispose() => _db.Dispose();
}