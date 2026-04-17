using System;
using System.Collections.Generic;
namespace ELearningPlatform.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public int OrderIndex { get; set; } = 1;

        // Navigation
        public Course Course { get; set; } = null!;
    }
}