using AutoMapper;
using ELearningPlatform.Controllers;
using ELearningPlatform.Data;
using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.Entities;
using ELearningPlatform.Mapping;
using ELearningPlatform.Repositories;
using ELearningPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ELearningPlatform.Tests;

public class ApiResponseTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly CoursesController _controller;

    public ApiResponseTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ApiDb_" + Guid.NewGuid())
            .Options;
        _db = new ApplicationDbContext(options);
        _db.Users.Add(new User { UserId=1, FullName="A",
            Email="a@t.com", PasswordHash="h"});
        _db.SaveChanges();

        var config  = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        var mapper  = config.CreateMapper();
        var cRepo   = new CourseRepository(_db);
        var lRepo   = new Repository<Lesson>(_db);
        var service = new CourseService(cRepo, lRepo, mapper);
        _controller = new CoursesController(service);
    }

    [Fact]
    public async Task GetAll_Returns200WithList()
    {
        var result = await _controller.GetAll() as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(200, result!.StatusCode);
    }

    [Fact]
    public async Task Create_ValidDto_Returns201()
    {
        var dto    = new CreateCourseDto("New Course", "Desc", 1);
        var result = await _controller.Create(dto) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal(201, result!.StatusCode);
        var data = result.Value as CourseResponseDto;
        Assert.Equal("New Course", data!.Title);
    }

    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        var result = await _controller.GetById(9999) as NotFoundObjectResult;
        Assert.NotNull(result);
        Assert.Equal(404, result!.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingCourse_Returns204()
    {
        var created = await _controller.Create(
            new CreateCourseDto("ToDelete","",1)) as CreatedAtActionResult;
        var dto    = created!.Value as CourseResponseDto;

        var result = await _controller.Delete(dto!.CourseId) as NoContentResult;
        Assert.NotNull(result);
        Assert.Equal(204, result!.StatusCode);
    }

    public void Dispose() => _db.Dispose();
}
