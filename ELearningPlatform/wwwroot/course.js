const API = '/api';
const userId = localStorage.getItem('userId');

if (!userId)
  window.location.href = 'index.html';

let allCourses = [];
let completedCourseIds = [];
let selectedCourseId = null;

async function loadCourses() {
  try {
    allCourses = await fetch(`${API}/Courses`, {
      cache: "no-store"
    }).then(r => r.json());

    completedCourseIds = await fetch(`${API}/users/${userId}/completed-courses`, {
      cache: "no-store"
    }).then(r => r.ok ? r.json() : []);

    renderCourses(allCourses);
  } catch {
    document.getElementById('courseGrid').innerHTML =
      '<p class="muted">Failed to load courses. Check backend.</p>';
  }
}

function renderCourses(courses) {
  const grid = document.getElementById('courseGrid');

  if (!courses.length) {
    grid.innerHTML = '<p class="muted">No courses available yet.</p>';
    return;
  }

  grid.innerHTML = courses.map(c => {
    const completed = completedCourseIds.includes(c.courseId);
    return `
      <div class="course-card">
        <div class="course-card-icon">📖</div>
        <h4>${c.title}</h4>
        <p>${c.description || 'Click to view lessons'}</p>
        <div class="course-card-meta">
          <button class="complete-btn ${completed ? 'completed' : ''}" type="button" onclick="completeCourse(event, ${c.courseId})">
            ${completed ? 'Completed' : 'Mark Complete'}
          </button>
          <span class="view-lessons" onclick="openCourse(${c.courseId})">View Lessons →</span>
        </div>
        <progress value="${completed ? 100 : 0}" max="100"></progress>
        <p class="course-progress-text">${completed ? '100% complete' : '0% complete'}</p>
      </div>
    `;
  }).join('');
}

async function completeCourse(event, courseId) {
  event.stopPropagation();
  if (completedCourseIds.includes(courseId)) return;

  try {
    const response = await fetch(`${API}/users/${userId}/completed-courses/${courseId}`, {
      method: 'POST'
    });

    if (!response.ok) {
      const data = await response.json().catch(() => null);
      alert(data?.message || 'Unable to mark course complete.');
      return;
    }

    completedCourseIds.push(courseId);
    renderCourses(allCourses);
  } catch {
    alert('Unable to mark course complete. Please try again.');
  }
}

function filterCourses() {
  const q = document.getElementById('searchBox').value.toLowerCase();

  renderCourses(allCourses.filter(c =>
    c.title.toLowerCase().includes(q) ||
    (c.description || '').toLowerCase().includes(q)
  ));
}

async function openCourse(courseId) {
  selectedCourseId = courseId;

  document.getElementById('courseGrid').style.display = 'none';
  document.getElementById('lessonPanel').style.display = 'block';

  const course = allCourses.find(c => c.courseId === courseId);
  document.getElementById('lessonCourseTitle').textContent =
    course ? course.title : '';

  const lessonList = document.getElementById('lessonList');
  lessonList.innerHTML = '<p class="muted">Loading lessons…</p>';

  try {
    const lessons = await fetch(
      `${API}/Courses/${courseId}/lessons`,
      { cache: "no-store" }
    ).then(r => r.json());

    console.log("LESSONS FROM API:", lessons);

    if (!lessons.length) {
      lessonList.innerHTML = '<p class="muted">No lessons for this course yet.</p>';
      return;
    }

    setTimeout(() => {
      lessonList.innerHTML = '';
    
      for (let i = 0; i < lessons.length; i++) {
        const l = lessons[i];
    
        const div = document.createElement('div');
        div.className = 'lesson-item';
    
        div.innerHTML = `
          <span class="lesson-num">${i + 1}</span>
          <div class="lesson-info">
            <strong>${l.title}</strong>
            <p>${l.content || ''}</p>
          </div>
        `;
    
        lessonList.appendChild(div);
      }
    }, 0);

  } catch (err) {
    console.error(err);
    lessonList.innerHTML = '<p class="muted">Could not load lessons.</p>';
  }
  console.log("LESSONS:", lessons);
console.log("lessonList:", document.getElementById('lessonList'));
}

function closeLesson() {
  selectedCourseId = null;
  document.getElementById('lessonPanel').style.display = 'none';
  document.getElementById('courseGrid').style.display = 'grid';
}

function goToQuiz() {
  if (selectedCourseId) {
    localStorage.setItem('quizCourseId', selectedCourseId);
    window.location.href = 'Quiz.html';
  }
}

function logout() { localStorage.clear(); window.location.href = 'index.html'; }
loadCourses();