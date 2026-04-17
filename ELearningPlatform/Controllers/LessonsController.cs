using Microsoft.AspNetCore.Mvc;
using ELearningPlatform.DTOs.Lessons;
using ELearningPlatform.Services;
using System.Threading.Tasks;

namespace ELearningPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ICourseService _service;
    public LessonsController(ICourseService service) => _service = service;

    // POST /api/lessons
    [HttpPost]
    [ProducesResponseType(typeof(LessonResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.AddLessonAsync(dto);
        return StatusCode(201, created);
    }

    // PUT /api/lessons/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(LessonResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateLessonAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    // DELETE /api/lessons/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteLessonAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("course/{courseId:int}")]
public async Task<IActionResult> GetByCourse(int courseId)
{
    return Ok(await _service.GetLessonsAsync(courseId));
}
}