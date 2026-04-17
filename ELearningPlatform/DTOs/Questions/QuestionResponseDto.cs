namespace ELearningPlatform.DTOs.Questions;

public record QuestionResponseDto(
    int QuestionId,
    int QuizId,
    string QuestionText,
    string OptionA,
    string OptionB,
    string OptionC,
    string OptionD,
    string CorrectAnswer
);