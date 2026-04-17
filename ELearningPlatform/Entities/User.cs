using System;
using System.Collections.Generic;

namespace ELearningPlatform.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public ICollection<Result> Results { get; set; } = new List<Result>();

        public ICollection<CourseCompletion> CourseCompletions { get; set; } = new List<CourseCompletion>();
    }
}