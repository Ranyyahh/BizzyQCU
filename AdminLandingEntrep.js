// ============================================================
// AdminEnterprises.js
// ============================================================
// EXAMPLE DATA — remove this array and use loadEnterprises()
// once the backend is ready.
//
// TODO (DB): delete the EXAMPLE_ENTERPRISES array below and
// uncomment loadEnterprises() at the bottom of this file.
// Each object shape must match what the API returns:
//   { id, name, rating, profileImg (optional), detailUrl (optional) }
// ============================================================

const EXAMPLE_ENTERPRISES = [
    { id: 'BQCU0110', name: 'Seresa Enterprise',    rating: 4 },
    { id: 'BQCU0111', name: 'Linaya Enterprise',    rating: 3 },
    { id: 'BQCU0112', name: 'Coco House Enterprise',rating: 5 },
    { id: 'BQCU0113', name: 'Tazyo Enterprise',     rating: 2 },
    { id: 'BQCU0114', name: 'Bloom & Co',           rating: 4 },
    { id: 'BQCU0115', name: "Nana's Kitchen",       rating: 5 },
    { id: 'BQCU0116', name: 'GreenLeaf Trading',    rating: 3 },
    { id: 'BQCU0117', name: 'SunRise Goods',        rating: 4 },
    { id: 'BQCU0118', name: 'Metro Crafts',         rating: 5 },
];

// ── DOM REFS ──
const searchInput = document.getElementById('searchInput');
const grid        = document.getElementById('enterprisesGrid');
const countBadge  = document.getElementById('countBadge');

// ── RENDER SINGLE CARD ──
// Builds one .enterprise-card element from a data object.
// Works whether data comes from the example array or the DB.
function renderCard(enterprise) {
    // Stars
    const stars = Array.from({ length: 5 }, (_, i) =>
        `<span class="star${i < enterprise.rating ? ' filled' : ''}">★</span>`
    ).join('');

    // Avatar: use profileImg from DB if available, else SVG placeholder
    const avatar = enterprise.profileImg
        ? `<img src="${enterprise.profileImg}" alt="${enterprise.name}" />`
        : `<svg width="44" height="44" viewBox="0 0 24 24" fill="none"
               stroke="#1a1f4e" stroke-width="1.5"
               stroke-linecap="round" stroke-linejoin="round">
               <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
               <circle cx="12" cy="7" r="4"/>
           </svg>`;

    // Detail link: use detailUrl from DB if provided, else build from ID
    // TODO (DB): once backend supplies a detailUrl or consistent ID format,
    // update the href below accordingly.
    const href = enterprise.detailUrl || `EnterpriseDetail.html?id=${enterprise.id}`;

    const card = document.createElement('div');
    card.className    = 'enterprise-card';
    card.dataset.name = enterprise.name;
    card.dataset.id   = enterprise.id;

    card.innerHTML = `
        <div class="card-avatar">${avatar}</div>
        <div class="card-name">${enterprise.name}</div>
        <div class="card-id">${enterprise.id}</div>
        <div class="card-stars">${stars}</div>
        <a href="${"AdminEditEntrep.html"}" class="view-btn">View Account</a>
    `;

    return card;
}

// ── RENDER ALL CARDS ──
// Clears the grid and re-renders every enterprise in the array.
function renderCards(enterprises) {
    grid.innerHTML = '';

    if (enterprises.length === 0) {
        showEmptyState();
        return;
    }

    enterprises.forEach(enterprise => grid.appendChild(renderCard(enterprise)));
    updateCount(enterprises.length);
}

// ── COUNT BADGE ──
function updateCount(n) {
    countBadge.textContent = n + (n === 1 ? ' Enterprise' : ' Enterprises');
}

// ── EMPTY STATE ──
function showEmptyState(query = '') {
    grid.innerHTML = '';
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.innerHTML = `
        <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor"
             stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="11" cy="11" r="8"/>
            <line x1="21" y1="21" x2="16.65" y2="16.65"/>
        </svg>
        <p>${query ? `No enterprises found for "<strong>${query}</strong>"` : 'No enterprises found.'}</p>
    `;
    grid.appendChild(empty);
    updateCount(0);
}

// ── SEARCH FILTER ──
// Filters the currently rendered cards by name or ID.
// TODO (DB): replace client-side filter with a backend search call:
//   const results = await fetch(`/api/enterprises?search=${query}`).then(r => r.json());
//   renderCards(results);
searchInput.addEventListener('input', function () {
    const query   = this.value.trim().toLowerCase();
    const cards   = grid.querySelectorAll('.enterprise-card');
    let   visible = 0;

    cards.forEach(card => {
        const match = card.dataset.name.toLowerCase().includes(query)
                   || card.dataset.id.toLowerCase().includes(query);
        card.style.display = match ? '' : 'none';
        if (match) visible++;
    });

    updateCount(visible);

    const empty = grid.querySelector('.empty-state');
    if (visible === 0 && !empty) showEmptyState(this.value);
    if (visible > 0  &&  empty)  empty.remove();
});

// ── LOAD ENTERPRISES ──
// TODO (DB): uncomment and use this function instead of renderCards(EXAMPLE_ENTERPRISES):
//
// async function loadEnterprises() {
//     try {
//         const data = await fetch('/api/enterprises').then(r => r.json());
//         renderCards(data);
//     } catch (err) {
//         console.error('Failed to load enterprises:', err);
//         showEmptyState();
//     }
// }
// loadEnterprises();

// ── INIT with example data (remove when DB is connected) ──
renderCards(EXAMPLE_ENTERPRISES);