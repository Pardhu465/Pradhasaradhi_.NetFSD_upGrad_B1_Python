using AutoMapper;
using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.DTOs.Lessons;
using ELearningPlatform.Entities;
using ELearningPlatform.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPlatform.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepo;
    private readonly IRepository<Lesson> _lessonRepo;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepo,
                         IRepository<Lesson> lessonRepo,
                         IMapper mapper)
    {
        _courseRepo = courseRepo;
        _lessonRepo = lessonRepo;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
    {
        var courses = await _courseRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<CourseResponseDto>>(courses);
    }

    public async Task<CourseResponseDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepo.GetByIdAsync(id);
        return course == null ? null : _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = _mapper.Map<Course>(dto);
        var created = await _courseRepo.AddAsync(course);
        return _mapper.Map<CourseResponseDto>(created);
    }

    public async Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _courseRepo.GetByIdAsync(id);
        if (course == null) return null;
        _mapper.Map(dto, course);
        await _courseRepo.UpdateAsync(course);
        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _courseRepo.GetByIdAsync(id);
        if (course == null) return false;
        await _courseRepo.DeleteAsync(course);
        return true;
    }

    public async Task<IEnumerable<LessonResponseDto>> GetLessonsAsync(int courseId)
    {
        var lessons = await _lessonRepo.GetAllAsync();
        var filtered = lessons.Where(l => l.CourseId == courseId)
                               .OrderBy(l => l.OrderIndex);
        return _mapper.Map<IEnumerable<LessonResponseDto>>(filtered);
    }

    public async Task<LessonResponseDto> AddLessonAsync(CreateLessonDto dto)
    {
        var lesson = _mapper.Map<Lesson>(dto);
        var created = await _lessonRepo.AddAsync(lesson);
        return _mapper.Map<LessonResponseDto>(created);
    }

    public async Task<LessonResponseDto?> UpdateLessonAsync(int id, UpdateLessonDto dto)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);
        if (lesson == null) return null;
        _mapper.Map(dto, lesson);
        await _lessonRepo.UpdateAsync(lesson);
        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task<bool> DeleteLessonAsync(int id)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);
        if (lesson == null) return false;
        await _lessonRepo.DeleteAsync(lesson);
        return true;
    }
}