using System;
using System.Collections.Generic;

namespace ELearningPlatform.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        // Navigation
        public Course Course { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}