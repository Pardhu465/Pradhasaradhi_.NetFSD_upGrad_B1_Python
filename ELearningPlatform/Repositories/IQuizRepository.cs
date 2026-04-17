using ELearningPlatform.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<IEnumerable<Quiz>> GetByCourseIdAsync(int courseId);
    Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(int quizId);
    Task<Question> AddQuestionAsync(Question question);
}

