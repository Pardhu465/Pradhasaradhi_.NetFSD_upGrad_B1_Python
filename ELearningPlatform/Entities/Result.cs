using System;
using System.Collections.Generic;
namespace ELearningPlatform.Entities
{
    public class Result
    {
        public int ResultId { get; set; }

        public int UserId { get; set; }

        public int QuizId { get; set; }

        public int Score { get; set; }

        public DateTime AttemptDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;

        public Quiz Quiz { get; set; } = null!;
    }
}