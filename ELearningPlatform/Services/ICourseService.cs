using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.DTOs.Lessons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllAsync();
    Task<CourseResponseDto?>             GetByIdAsync(int id);
    Task<CourseResponseDto>              CreateAsync(CreateCourseDto dto);
    Task<CourseResponseDto?>             UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool>                           DeleteAsync(int id);
    Task<IEnumerable<LessonResponseDto>> GetLessonsAsync(int courseId);
    Task<LessonResponseDto>              AddLessonAsync(CreateLessonDto dto);
    Task<LessonResponseDto?>             UpdateLessonAsync(int id, UpdateLessonDto dto);
    Task<bool>                           DeleteLessonAsync(int id);
}