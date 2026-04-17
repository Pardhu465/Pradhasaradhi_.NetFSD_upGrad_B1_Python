


function calculateScore(questions, answers) {
  if (!questions || !questions.length) return 0;
  const correct = questions.filter(q =>
    answers[q.questionId] &&
    answers[q.questionId].toUpperCase() === q.correctAnswer.toUpperCase()
  ).length;
  return Math.round((correct / questions.length) * 100);
}

function getGrade(score) {
  if (score >= 90) return 'A';
  if (score >= 80) return 'B';
  if (score >= 70) return 'C';
  if (score >= 60) return 'D';
  return 'F';
}

function filterQuestions(questions, keyword) {
  if (!keyword) return questions;
  const kw = keyword.toLowerCase();
  return questions.filter(q => q.questionText.toLowerCase().includes(kw));
}


if (typeof module !== 'undefined') {
  module.exports = { calculateScore, getGrade, filterQuestions };
}