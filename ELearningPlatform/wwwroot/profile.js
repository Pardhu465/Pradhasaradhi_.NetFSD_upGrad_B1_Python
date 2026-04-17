
const userId = localStorage.getItem('userId');
if (!userId) window.location.href = 'index.html';

async function loadProfile() {
  try {
    const user = await fetch('/api/users/' + userId).then(r => r.json());
    document.getElementById('pName').textContent   = user.fullName;
    document.getElementById('pEmail').textContent  = user.email;
    document.getElementById('pJoined').textContent =
      'Member since ' + new Date(user.createdAt).toLocaleDateString();
    document.getElementById('editName').value  = user.fullName;
    document.getElementById('editEmail').value = user.email;

    const completions = await fetch(`/api/users/${userId}/completed-courses`).then(r => r.ok ? r.json() : []);
    document.getElementById('completedCount').textContent = completions.length;
  } catch {
    document.getElementById('pName').textContent = 'Could not load profile.';
  }
}

function editProfile() {
  document.getElementById('profileView').style.display = 'none';
  document.getElementById('profileEdit').style.display = 'block';
}

function cancelEdit() {
  document.getElementById('profileEdit').style.display = 'none';
  document.getElementById('profileView').style.display = 'block';
}

async function saveProfile() {
  const fullName = document.getElementById('editName').value.trim();
  if (!fullName) { showProfileMsg('Name cannot be empty.', 'error'); return; }
  try {
    const res  = await fetch('/api/users/' + userId, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ fullName })
    });
    const data = await res.json();
    if (res.ok) {
      localStorage.setItem('userName', data.fullName);
      document.getElementById('pName').textContent = data.fullName;
      cancelEdit();
      showProfileMsg('Profile updated successfully!', 'success');
    } else {
      showProfileMsg('Update failed.', 'error');
    }
  } catch { showProfileMsg('Server error.', 'error'); }
}

function showProfileMsg(text, type) {
  const el = document.getElementById('profileMsg');
  el.textContent = text; el.className = 'auth-msg ' + type;
  el.style.display = 'block';
  setTimeout(() => el.style.display = 'none', 3000);
}

function logout() { localStorage.clear(); window.location.href = 'index.html'; }

loadProfile();