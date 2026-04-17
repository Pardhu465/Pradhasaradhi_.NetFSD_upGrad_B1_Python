using ELearningPlatform.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<Result>> GetResultsByUserIdAsync(int userId);
    Task<Result> AddResultAsync(Result result);
    Task<IEnumerable<CourseCompletion>> GetCourseCompletionsByUserIdAsync(int userId);
    Task<CourseCompletion?> GetCourseCompletionAsync(int userId, int courseId);
    Task<CourseCompletion> AddCourseCompletionAsync(CourseCompletion completion);
}