using ELearningPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPlatform.Repositories;

public class QuizRepository : Repository<Quiz>, IQuizRepository
{
    public QuizRepository(Data.ApplicationDbContext db) : base(db) { }

    public async Task<IEnumerable<Quiz>> GetByCourseIdAsync(int courseId)
        => await _db.Quizzes
            .Where(q => q.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(int quizId)
        => await _db.Questions
            .Where(q => q.QuizId == quizId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Question> AddQuestionAsync(Question question)
    {
        await _db.Questions.AddAsync(question);
        await _db.SaveChangesAsync();
        return question;
    }
}