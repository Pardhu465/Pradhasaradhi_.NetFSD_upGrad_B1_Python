
if (!localStorage.getItem('userId')) window.location.href = 'index.html';

document.getElementById('userName').textContent = localStorage.getItem('userName') || 'Student';

const API = '/api';
const userId = localStorage.getItem('userId');

async function loadDashboard() {
  try {
    // ── Total courses from API ────────────────────────────
    const courses = await fetch(`${API}/Courses`).then(r => r.json());
    document.getElementById('totalCourses').textContent = courses.length;

    // ── Completed course ids for this user ───────────────
    const completedCourseIds = await fetch(`${API}/users/${userId}/completed-courses`).then(r => r.ok ? r.json() : []);

    // ── Quiz results for this user ───────────────────────
    const results = await fetch(`${API}/results/${userId}`).then(r => r.json());
    document.getElementById('quizzesTaken').textContent = results.length;

    const avg = results.length
      ? Math.round(results.reduce((s, r) => s + r.score, 0) / results.length)
      : 0;
    document.getElementById('avgScore').textContent = avg + '%';

    // ── Render recent results ────────────────────────────
    const resultContainer = document.getElementById('resultsContainer');
    if (!results.length) {
      resultContainer.innerHTML = '<p class="muted">No quiz attempts yet. Browse Courses and take a quiz!</p>';
    } else {
      resultContainer.innerHTML = results.slice(0, 5).map(r => `
        <div class="result-item">
          <span class="result-quiz">Quiz #${r.quizId}</span>
          <span class="result-score ${r.score >= 70 ? 'pass' : 'fail'}">${r.score}%</span>
          <span class="result-date">${new Date(r.attemptDate).toLocaleDateString()}</span>
        </div>
      `).join('');
    }

    // ── Render course progress ──────────────────────────
    const progressContainer = document.getElementById('courseProgressContainer');
    if (!courses.length) {
      progressContainer.innerHTML = '<p class="muted">No courses available yet.</p>';
    } else {
      progressContainer.innerHTML = courses.map(c => {
        const completed = completedCourseIds.includes(c.courseId);
        return `
          <div class="course-progress-item">
            <div class="course-progress-title">${c.title}</div>
            <progress value="${completed ? 100 : 0}" max="100"></progress>
            <div class="course-progress-label">${completed ? 'Completed' : 'Not completed'}</div>
          </div>
        `;
      }).join('');
    }
  } catch (err) {
    console.error('Dashboard load error:', err);
    document.getElementById('resultsContainer').innerHTML =
      '<p class="muted">Could not load data. Make sure backend is running.</p>';
    document.getElementById('courseProgressContainer').innerHTML =
      '<p class="muted">Could not load progress.</p>';
  }
}
function logout() { localStorage.clear(); window.location.href = 'index.html'; }

loadDashboard();