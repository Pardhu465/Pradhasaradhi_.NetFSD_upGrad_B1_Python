using System;

namespace ELearningPlatform.Entities
{
    public class CourseCompletion
    {
        public int CourseCompletionId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
