using AutoMapper;
using ELearningPlatform.DTOs.Quizzes;
using ELearningPlatform.DTOs.Questions;
using ELearningPlatform.DTOs.Results;
using ELearningPlatform.Entities;
using ELearningPlatform.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public QuizService(IQuizRepository quizRepo,
                       IUserRepository userRepo,
                       IMapper mapper)
    {
        _quizRepo = quizRepo;
        _userRepo = userRepo;
        _mapper   = mapper;
    }

    public async Task<IEnumerable<QuizResponseDto>> GetByCourseIdAsync(int courseId)
    {
        var quizzes = await _quizRepo.GetByCourseIdAsync(courseId);
        return _mapper.Map<IEnumerable<QuizResponseDto>>(quizzes);
    }

    public async Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto dto)
    {
        var quiz = _mapper.Map<Quiz>(dto);
        var created = await _quizRepo.AddAsync(quiz);
        return _mapper.Map<QuizResponseDto>(created);
    }

    public async Task<IEnumerable<QuestionResponseDto>> GetQuestionsAsync(int quizId)
    {
        var questions = await _quizRepo.GetQuestionsByQuizIdAsync(quizId);
        return _mapper.Map<IEnumerable<QuestionResponseDto>>(questions);
    }

    public async Task<QuestionResponseDto> AddQuestionAsync(CreateQuestionDto dto)
    {
        var question = _mapper.Map<Question>(dto);
        var created = await _quizRepo.AddQuestionAsync(question);
        return _mapper.Map<QuestionResponseDto>(created);
    }

    public async Task<ResultResponseDto> SubmitQuizAsync(int quizId, SubmitQuizDto dto)
    {
        var quiz = await _quizRepo.GetByIdAsync(quizId)
            ?? throw new KeyNotFoundException($"Quiz {quizId} not found.");

        var result = new Result
        {
            UserId      = dto.UserId,
            QuizId      = quizId,
            Score       = dto.Score,
            AttemptDate = DateTime.UtcNow
        };

        var saved = await _userRepo.AddResultAsync(result);
        return _mapper.Map<ResultResponseDto>(saved);
    }

    public async Task<IEnumerable<ResultResponseDto>> GetResultsAsync(int userId)
    {
        var results = await _userRepo.GetResultsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ResultResponseDto>>(results);
    }
}