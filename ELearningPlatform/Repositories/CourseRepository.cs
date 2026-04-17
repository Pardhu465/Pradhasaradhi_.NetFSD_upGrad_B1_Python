using ELearningPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(Data.ApplicationDbContext db) : base(db) { }

    public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        => await _db.Courses
            .Include(c => c.Creator)
            .Include(c => c.Lessons)
            .Include(c => c.Quizzes)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Course?> GetByIdWithDetailsAsync(int id)
        => await _db.Courses
            .Include(c => c.Creator)
            .Include(c => c.Lessons.OrderBy(l => l.OrderIndex))
            .Include(c => c.Quizzes).ThenInclude(q => q.Questions)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CourseId == id);
}