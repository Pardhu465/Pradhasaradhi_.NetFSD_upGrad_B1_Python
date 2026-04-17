using Microsoft.AspNetCore.Mvc;
using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.DTOs.Lessons;
using ELearningPlatform.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Controllers;



[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;
    public CoursesController(ICourseService service) => _service = service;

    // GET /api/courses
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseResponseDto>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    // GET /api/courses/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CourseResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _service.GetByIdAsync(id);
        return course == null ? NotFound(new { message = "Course not found." }) : Ok(course);
    }

    // POST /api/courses
    [HttpPost]
    [ProducesResponseType(typeof(CourseResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, created);
    }

    // PUT /api/courses/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CourseResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    // DELETE /api/courses/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // GET /api/courses/{courseId}/lessons
    [HttpGet("{courseId:int}/lessons")]
    [ProducesResponseType(typeof(IEnumerable<LessonResponseDto>), 200)]
    public async Task<IActionResult> GetLessons(int courseId)
        => Ok(await _service.GetLessonsAsync(courseId));
}