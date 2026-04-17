using ELearningPlatform.DTOs.Quizzes;
using ELearningPlatform.DTOs.Questions;
using ELearningPlatform.DTOs.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public interface IQuizService
{
    Task<IEnumerable<QuizResponseDto>>    GetByCourseIdAsync(int courseId);
    Task<QuizResponseDto>                 CreateQuizAsync(CreateQuizDto dto);
    Task<IEnumerable<QuestionResponseDto>>GetQuestionsAsync(int quizId);
    Task<QuestionResponseDto>             AddQuestionAsync(CreateQuestionDto dto);
    Task<ResultResponseDto>               SubmitQuizAsync(int quizId, SubmitQuizDto dto);
    Task<IEnumerable<ResultResponseDto>>  GetResultsAsync(int userId);
}