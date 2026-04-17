using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.DTOs.Questions;

public record CreateQuestionDto(
    [Required] int QuizId,
    [Required][StringLength(500)] string QuestionText,
    [Required] string OptionA,
    [Required] string OptionB,
    [Required] string OptionC,
    [Required] string OptionD,
    [Required][RegularExpression("^[ABCD]$")] string CorrectAnswer
);