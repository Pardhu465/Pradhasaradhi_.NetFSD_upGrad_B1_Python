using Microsoft.AspNetCore.Mvc;
using ELearningPlatform.DTOs.Quizzes;
using ELearningPlatform.DTOs.Questions;
using ELearningPlatform.DTOs.Results;
using ELearningPlatform.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ELearningPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _service;
    public QuizzesController(IQuizService service) => _service = service;

    // GET /api/quizzes/{courseId}
    [HttpGet("{courseId:int}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResponseDto>), 200)]
    public async Task<IActionResult> GetByCourse(int courseId)
        => Ok(await _service.GetByCourseIdAsync(courseId));

    // POST /api/quizzes
    [HttpPost]
    [ProducesResponseType(typeof(QuizResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateQuizDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateQuizAsync(dto);
        return StatusCode(201, created);
    }

    // GET /api/quizzes/{quizId}/questions
    [HttpGet("{quizId:int}/questions")]
    [ProducesResponseType(typeof(IEnumerable<QuestionResponseDto>), 200)]
    public async Task<IActionResult> GetQuestions(int quizId)
        => Ok(await _service.GetQuestionsAsync(quizId));

    // POST /api/questions
    [HttpPost("/api/questions")]
    [ProducesResponseType(typeof(QuestionResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddQuestion([FromBody] CreateQuestionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.AddQuestionAsync(dto);
        return StatusCode(201, created);
    }

    // POST /api/quizzes/{quizId}/submit
    [HttpPost("{quizId:int}/submit")]
    [ProducesResponseType(typeof(ResultResponseDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Submit(int quizId, [FromBody] SubmitQuizDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _service.SubmitQuizAsync(quizId, dto);
            return StatusCode(201, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}