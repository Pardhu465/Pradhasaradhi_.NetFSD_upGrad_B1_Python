using ELearningPlatform.Data;
using ELearningPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ELearningPlatform.Tests;

public class LinqFilterTests : IDisposable
{
    private readonly ApplicationDbContext _db;

    public LinqFilterTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("LinqDb_" + Guid.NewGuid())
            .Options;
        _db = new ApplicationDbContext(options);

        _db.Users.AddRange(
            new User { UserId=1, FullName="Alice", Email="a@t.com",PasswordHash="h"},
            new User { UserId=2, FullName="Bob",   Email="b@t.com",PasswordHash="h"}
        );
        _db.Courses.AddRange(
            new Course { CourseId=1, Title="C# Basics",   CreatedBy=1, CreatedAt=DateTime.UtcNow },
            new Course { CourseId=2, Title="ASP.NET Core", CreatedBy=1, CreatedAt=DateTime.UtcNow },
            new Course { CourseId=3, Title="SQL Server",  CreatedBy=2, CreatedAt=DateTime.UtcNow }
        );
        _db.SaveChanges();
    }

    [Fact]
    public async Task Filter_CoursesByTitle_ReturnsMatches()
    {
        var results = await _db.Courses
            .Where(c => c.Title.Contains("C#"))
            .AsNoTracking()
            .ToListAsync();

        Assert.Single(results);
        Assert.Equal("C# Basics", results[0].Title);
    }

    [Fact]
    public async Task OrderBy_CourseTitle_ReturnsOrdered()
    {
        var courses = await _db.Courses.OrderBy(c => c.Title)
            .AsNoTracking().ToListAsync();

        Assert.Equal("ASP.NET Core", courses[0].Title);
        Assert.Equal("C# Basics",   courses[1].Title);
        Assert.Equal("SQL Server",  courses[2].Title);
    }

    [Fact]
    public async Task Count_CoursesByCreator_IsCorrect()
    {
        var count = await _db.Courses
            .Where(c => c.CreatedBy == 1)
            .CountAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GroupBy_CoursesByCreator_CorrectGroups()
    {
        var groups = await _db.Courses
            .GroupBy(c => c.CreatedBy)
            .Select(g => new { CreatedBy = g.Key, Count = g.Count() })
            .ToListAsync();

        Assert.Equal(2, groups.Count);
        Assert.Contains(groups, g => g.CreatedBy == 1 && g.Count == 2);
        Assert.Contains(groups, g => g.CreatedBy == 2 && g.Count == 1);
    }

    public void Dispose() => _db.Dispose();
}