using ELearningPlatform.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetAllWithDetailsAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);
}