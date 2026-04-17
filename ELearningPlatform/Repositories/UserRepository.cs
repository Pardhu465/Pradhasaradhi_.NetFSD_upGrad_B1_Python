using ELearningPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(Data.ApplicationDbContext db) : base(db) { }

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<Result>> GetResultsByUserIdAsync(int userId)
        => await _db.Results
            .Where(r => r.UserId == userId)
            .Include(r => r.Quiz)
            .AsNoTracking()
            .OrderByDescending(r => r.AttemptDate)
            .ToListAsync();

    public async Task<Result> AddResultAsync(Result result)
    {
        await _db.Results.AddAsync(result);
        await _db.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<CourseCompletion>> GetCourseCompletionsByUserIdAsync(int userId)
        => await _db.CourseCompletions
            .Where(cc => cc.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<CourseCompletion?> GetCourseCompletionAsync(int userId, int courseId)
        => await _db.CourseCompletions
            .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

    public async Task<CourseCompletion> AddCourseCompletionAsync(CourseCompletion completion)
    {
        await _db.CourseCompletions.AddAsync(completion);
        await _db.SaveChangesAsync();
        return completion;
    }
}