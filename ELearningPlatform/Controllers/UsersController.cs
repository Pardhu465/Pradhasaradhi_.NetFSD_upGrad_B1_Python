using Microsoft.AspNetCore.Mvc;
using ELearningPlatform.DTOs.Auth;
using ELearningPlatform.DTOs.Results;
using ELearningPlatform.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IQuizService _quizService;

    public UsersController(IUserService service, IQuizService quizService)
    {
        _service     = service;
        _quizService = quizService;
    }

    // POST /api/users/register
    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (success, message) = await _service.RegisterAsync(dto);
        return success
            ? StatusCode(201, new { message })
            : BadRequest(new { message });
    }

    // POST /api/users/login
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.LoginAsync(dto);
        return result == null
            ? BadRequest(new { message = "Invalid email or password." })
            : Ok(result);
    }

    // GET /api/users/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    // PUT /api/users/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    // GET /api/users/{userId}/completed-courses
    [HttpGet("{userId:int}/completed-courses")]
    [ProducesResponseType(typeof(IEnumerable<int>), 200)]
    public async Task<IActionResult> GetCompletedCourses(int userId)
        => Ok(await _service.GetCompletedCourseIdsAsync(userId));

    // POST /api/users/{userId}/completed-courses/{courseId}
    [HttpPost("{userId:int}/completed-courses/{courseId:int}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CompleteCourse(int userId, int courseId)
    {
        var created = await _service.AddCourseCompletionAsync(userId, courseId);
        return created ? StatusCode(201) : BadRequest(new { message = "Course already completed." });
    }

    // GET /api/results/{userId}
    [HttpGet("/api/results/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ResultResponseDto>), 200)]
    public async Task<IActionResult> GetResults(int userId)
        => Ok(await _quizService.GetResultsAsync(userId));
}