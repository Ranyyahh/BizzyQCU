// TODO (DB): On page load, fetch users and enterprises from
// the backend and call renderUsers(data) / renderEnterprises(data).
//
// async function loadAll() {
//     const [users, enterprises] = await Promise.all([
//         fetch('/api/users/pending').then(r => r.json()),
//         fetch('/api/enterprises/pending').then(r => r.json())
//     ]);
//     renderUsers(users);
//     renderEnterprises(enterprises);
// }
// loadAll();
// ============================================================

// ── EXAMPLE DATA (remove when DB is connected) ──
const EXAMPLE_USERS = [
    { id: 1, name: 'Lucas Nathan', email: 'mariel.delacruz@gmail.com', section: 'SBIT2E', studentNo: '24-1798', status: 'approved' },
    { id: 2, name: 'Jack Daniels', email: 'jack.daniels@gmail.com', section: 'SBEI4A', studentNo: '22-4918', status: 'pending' },
    { id: 3, name: 'Samuel Winston', email: 'samuel.winston@gmail.com', section: 'SBEE3B', studentNo: '23-5871', status: 'denied' },
    { id: 4, name: 'Kathleen Crystal', email: 'kathleen.crystal@gmail.com', section: 'SBIT4A', studentNo: '22-2219', status: 'denied' },
];

const EXAMPLE_ENTERPRISES = [
    { id: 1, name: 'Seresa Enterprise', email: 'seresa@gmail.com', businessType: 'Food & Beverage', enterpriseNo: 'BQCU0110', status: 'approved' },
    { id: 2, name: 'Linaya Enterprise', email: 'linaya@gmail.com', businessType: 'Retail', enterpriseNo: 'BQCU0111', status: 'pending' },
    { id: 3, name: 'Tazyo Enterprise', email: 'tazyo@gmail.com', businessType: 'Services', enterpriseNo: 'BQCU0113', status: 'denied' },
];

const usersBody = document.getElementById('usersTableBody');
const enterprisesBody = document.getElementById('enterprisesTableBody');
const searchInput = document.getElementById('searchInput');
const tabUsers = document.getElementById('tabUsers');
const tabEnterprises = document.getElementById('tabEnterprises');
const panelUsers = document.getElementById('panelUsers');
const panelEnterprises = document.getElementById('panelEnterprises');

let activeTab = 'users';

function statusBadge(id, status, type) {
    if (status === 'approved') {
        return `<span class="status-badge status-approved">Approved <span class="status-icon">✔</span></span>`;
    }
    if (status === 'denied') {
        return `<span class="status-badge status-denied">Denied <span class="status-icon">✕</span></span>`;
    }
    return `
        <div class="pending-actions">
            <span class="pending-label">Approve</span>
            <button class="approve-icon-btn" onclick="handleApprove(${id}, '${type}')">✔</button>
            <button class="deny-icon-btn"    onclick="handleDeny(${id}, '${type}')">✕</button>
        </div>`;
}

// ── RENDER USER ROW ──
function renderUserRow(user) {
    const tr = document.createElement('tr');
    tr.dataset.name = user.name.toLowerCase();
    tr.dataset.id = user.id;
    tr.innerHTML = `
        <td class="td-name">${user.name}</td>
        <td>${user.email}</td>
        <td>${user.section}</td>
        <td>${user.studentNo}</td>
        <td>${statusBadge(user.id, user.status, 'user')}</td>
    `;
    return tr;
}

// ── RENDER ENTERPRISE ROW ──
function renderEnterpriseRow(enterprise) {
    const tr = document.createElement('tr');
    tr.dataset.name = enterprise.name.toLowerCase();
    tr.dataset.id = enterprise.id;
    tr.innerHTML = `
        <td class="td-name">${enterprise.name}</td>
        <td>${enterprise.email}</td>
        <td>${enterprise.businessType}</td>
        <td>${enterprise.enterpriseNo}</td>
        <td>${statusBadge(enterprise.id, enterprise.status, 'enterprise')}</td>
    `;
    return tr;
}

// ── RENDER ALL USERS ──
function renderUsers(users) {
    usersBody.innerHTML = '';
    if (!users.length) {
        usersBody.innerHTML = '<tr class="empty-row"><td colspan="5">No users found.</td></tr>';
        return;
    }
    users.forEach(u => usersBody.appendChild(renderUserRow(u)));
}

// ── RENDER ALL ENTERPRISES ──
function renderEnterprises(enterprises) {
    enterprisesBody.innerHTML = '';
    if (!enterprises.length) {
        enterprisesBody.innerHTML = '<tr class="empty-row"><td colspan="5">No enterprises found.</td></tr>';
        return;
    }
    enterprises.forEach(e => enterprisesBody.appendChild(renderEnterpriseRow(e)));
}

// ── APPROVE / DENY HANDLERS ──
// TODO (DB): replace alerts with real API calls:
//   await fetch(`/api/${type}s/${id}/approve`, { method: 'PATCH' });
//   then re-fetch and re-render the table.
function handleApprove(id, type) {
    console.log(`Approve ${type} ID:`, id);
    // TODO (DB): PATCH /api/${type}s/${id}/approve
    // For now update example data locally
    updateLocalStatus(id, type, 'approved');
}

function handleDeny(id, type) {
    console.log(`Deny ${type} ID:`, id);
    // TODO (DB): PATCH /api/${type}s/${id}/deny
    updateLocalStatus(id, type, 'denied');
}

function updateLocalStatus(id, type, newStatus) {
    if (type === 'user') {
        const user = EXAMPLE_USERS.find(u => u.id === id);
        if (user) { user.status = newStatus; renderUsers(EXAMPLE_USERS); }
    } else {
        const ent = EXAMPLE_ENTERPRISES.find(e => e.id === id);
        if (ent) { ent.status = newStatus; renderEnterprises(EXAMPLE_ENTERPRISES); }
    }
}

tabUsers.addEventListener('click', () => {
    activeTab = 'users';
    tabUsers.classList.add('active');
    tabEnterprises.classList.remove('active');
    panelUsers.classList.remove('hidden');
    panelEnterprises.classList.add('hidden');
    searchInput.placeholder = 'Search Name';
    searchInput.value = '';
    filterTable();
});

tabEnterprises.addEventListener('click', () => {
    activeTab = 'enterprises';
    tabEnterprises.classList.add('active');
    tabUsers.classList.remove('active');
    panelEnterprises.classList.remove('hidden');
    panelUsers.classList.add('hidden');
    searchInput.placeholder = 'Search Enterprise';
    searchInput.value = '';
    filterTable();
});

// ── SEARCH FILTER ──
// TODO (DB): replace client-side filter with backend search query
searchInput.addEventListener('input', filterTable);

function filterTable() {
    const query = searchInput.value.trim().toLowerCase();
    const body = activeTab === 'users' ? usersBody : enterprisesBody;
    const rows = body.querySelectorAll('tr:not(.empty-row)');

    let visible = 0;
    rows.forEach(row => {
        const match = row.dataset.name.includes(query);
        row.style.display = match ? '' : 'none';
        if (match) visible++;
    });

    let empty = body.querySelector('.empty-row');
    if (visible === 0 && rows.length > 0) {
        if (!empty) {
            const cols = activeTab === 'users' ? 5 : 5;
            empty = document.createElement('tr');
            empty.className = 'empty-row';
            empty.innerHTML = `<td colspan="${cols}">No results for "<strong>${searchInput.value}</strong>"</td>`;
            body.appendChild(empty);
        }
    } else if (empty && visible > 0) {
        empty.remove();
    }
}

renderUsers(EXAMPLE_USERS);
renderEnterprises(EXAMPLE_ENTERPRISES);