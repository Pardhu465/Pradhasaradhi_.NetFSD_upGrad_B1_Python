using AutoMapper;
using ELearningPlatform.DTOs.Auth;
using ELearningPlatform.DTOs.Courses;
using ELearningPlatform.DTOs.Lessons;
using ELearningPlatform.DTOs.Quizzes;
using ELearningPlatform.DTOs.Questions;
using ELearningPlatform.DTOs.Results;
using ELearningPlatform.Entities;

namespace ELearningPlatform.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserResponseDto>();
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Course
        CreateMap<Course, CourseResponseDto>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();

        // Lesson
        CreateMap<Lesson, LessonResponseDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>();

        // Quiz & Question
        CreateMap<Quiz, QuizResponseDto>();
        CreateMap<CreateQuizDto, Quiz>();
        CreateMap<Question, QuestionResponseDto>();
        CreateMap<CreateQuestionDto, Question>();

        // Result
        CreateMap<Result, ResultResponseDto>();
    }
}
