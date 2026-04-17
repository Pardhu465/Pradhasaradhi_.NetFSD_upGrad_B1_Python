
if (!localStorage.getItem('userId')) window.location.href = 'index.html';

const API      = '/api';
const courseId = parseInt(localStorage.getItem('quizCourseId'));
const userId   = parseInt(localStorage.getItem('userId'));

let currentQuiz = null;
let questions   = [];
let userAnswers = {};  // { questionId: 'A'|'B'|'C'|'D' }

async function loadQuiz() {
  const container = document.getElementById('quizContainer');
  if (!courseId) {
    container.innerHTML = '<p class="muted text-center">No course selected. <a href="Courses.html">Go back</a></p>';
    return;
  }
  try {
    // Step 1: Get quizzes for this course
    const quizzes = await fetch(`${API}/quizzes/${courseId}`).then(r => r.json());
    if (!quizzes.length) {
      container.innerHTML = '<p class="muted text-center">No quiz for this course yet.</p>'; return;
    }
    currentQuiz = quizzes[0];

    // Step 2: Get questions for this quiz
    questions = await fetch(`${API}/quizzes/${currentQuiz.quizId}/questions`).then(r => r.json());
    if (!questions.length) {
      container.innerHTML = '<p class="muted text-center">Quiz has no questions yet.</p>'; return;
    }
    renderQuiz();
  } catch (err) {
    container.innerHTML = '<p class="muted text-center">Failed to load quiz. Check backend.</p>';
  }
}

function renderQuiz() {
  const container = document.getElementById('quizContainer');
  container.innerHTML = `
    <div class="quiz-header">
      <h2 id="quiz">${currentQuiz.title}</h2>
      <p>${questions.length} Questions — Answer all before submitting</p>
    </div>
    <div id="quizMessage" style="display:none"></div>`;

  // Render questions
  questions.forEach((q, index) => {
    const block = document.createElement('div');
    block.classList.add('question-block');
    block.innerHTML = `
      <div class="question-header">
        <h4>${index + 1}. ${q.questionText}</h4>
      </div>`;

    ['A', 'B', 'C', 'D'].forEach(opt => {
      const label = document.createElement('label');
      label.className = 'option-label';
      label.innerHTML = `
        <input type='radio' name='q_${q.questionId}' value='${opt}'
               onchange='userAnswers[${q.questionId}]="${opt}"'>
        <span>${opt}. ${q['option'+opt]}</span>`;
      block.appendChild(label);
    });
    container.appendChild(block);
  });

  // Submit button
  const btnWrap = document.createElement('div');
  btnWrap.id = 'SubmitButton';
  btnWrap.innerHTML = '<button class="submit-quiz-btn" onclick="submitQuiz()">Submit Quiz 🎯</button>';
  container.appendChild(btnWrap);
}

async function submitQuiz() {
  // Validate: all questions answered
  const unanswered = questions.filter(q => !userAnswers[q.questionId]);
  if (unanswered.length) {
    alert(`Please answer all ${questions.length} questions before submitting.`); return;
  }

  // Calculate score using quizLogic.js (pure function)
  const score = calculateScore(questions, userAnswers);

  // Submit to backend
  try {
    await fetch(`${API}/quizzes/${currentQuiz.quizId}/submit`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ userId: userId, score: score })
    });
  } catch { /* Show result even if save fails */ }

  showResult(score);
}

function showResult(score) {
  document.getElementById('quizContainer').style.display = 'none';
  const resultDiv = document.getElementById('quizResult');
  resultDiv.style.display = 'block';

  const grade = getGrade(score);
  const pass  = score >= 70;

  resultDiv.innerHTML = `
    <h3>${pass ? '🎉 Congratulations!' : '📝 Quiz Complete'}</h3>
    <div class='score-circle ${pass ? 'pass' : 'fail'}'>${score}%</div>
    <p>Grade: <strong>${grade}</strong></p>
    <p>${pass ? 'You passed! Great work.' : 'Keep practicing — you can do it!'}</p>
    <div class='result-actions'>
      <a href='Courses.html' class='btn-secondary'>Back to Courses</a>
      <a href='Dashboard.html' class='btn-primary'>View Dashboard</a>
    </div>`;
}

function logout() { localStorage.clear(); window.location.href = 'index.html'; }

loadQuiz();