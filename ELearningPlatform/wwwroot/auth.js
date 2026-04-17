
/* ── Tab switch ──────────────────────────────────────────── */
function switchTab(tab) {
  document.getElementById('loginSection').style.display    = tab==='login'    ? 'block' : 'none';
  document.getElementById('registerSection').style.display = tab==='register' ? 'block' : 'none';
  document.getElementById('loginTab').classList.toggle('active',    tab==='login');
  document.getElementById('registerTab').classList.toggle('active', tab==='register');
  hideMsg();
}

/* ── Message helpers ─────────────────────────────────────── */
function showMsg(text, type='error') {
  const el = document.getElementById('authMsg');
  el.textContent = text; el.className = 'auth-msg ' + type;
  el.style.display = 'block';
}
function hideMsg() {
  const el = document.getElementById('authMsg');
  if (el) el.style.display = 'none';
}

/* ── REGISTER ────────────────────────────────────────────── */
async function registerUser() {
  const fullName = document.getElementById('regName').value.trim();
  const email    = document.getElementById('regEmail').value.trim();
  const password = document.getElementById('regPass').value;
  if (!fullName || !email || !password) { showMsg('Please fill in all fields.'); return; }
  if (password.length < 6) { showMsg('Password must be at least 6 characters.'); return; }
  try {
    const res  = await fetch('/api/users/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ fullName, email, password })
    });
    const data = await res.json();
    if (res.ok || res.status === 201) {
      showMsg('Account created! Please login.', 'success');
      setTimeout(() => switchTab('login'), 1500);
    } else {
      showMsg(data.message || 'Registration failed.');
    }
  } catch { showMsg('Server error. Is the backend running?'); }
}

/* ── LOGIN ───────────────────────────────────────────────── */
async function loginUser() {
  const email    = document.getElementById('loginEmail').value.trim();
  const password = document.getElementById('loginPass').value;
  if (!email || !password) { showMsg('Enter email and password.'); return; }
  try {
    const res  = await fetch('/api/users/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    const data = await res.json();
    if (res.ok) {
      localStorage.setItem('userId',    data.userId);
      localStorage.setItem('userName',  data.fullName);
      localStorage.setItem('userEmail', data.email);
      window.location.href = 'Dashboard.html';
    } else {
      showMsg(data.message || 'Invalid credentials.');
    }
  } catch { showMsg('Server error. Is the backend running?'); }
}

/* ── AUTH GUARD — runs on every page ─────────────────────── */
/* Put this at top of dashboard.js, course.js, quiz.js, profile.js */
function requireLogin() {
  if (!localStorage.getItem('userId')) {
    window.location.href = 'index.html';
    return false;
  }
  return true;
}

/* ── LOGOUT ──────────────────────────────────────────────── */
function logout() {
  localStorage.clear();
  window.location.href = 'index.html';
}

/* ── Auto-redirect if already logged in ──────────────────── */
(function() {
  const onAuthPage = window.location.pathname.endsWith('index.html') ||
                     window.location.pathname === '/' ||
                     window.location.pathname.endsWith('/');
  if (onAuthPage && localStorage.getItem('userId')) {
    window.location.href = 'Dashboard.html';
  }
})();