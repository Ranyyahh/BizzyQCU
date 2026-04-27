// TODO (DB): On page load, read the enterprise ID from the URL
//   const params = new URLSearchParams(window.location.search);
//   const id = params.get('id');
//   const enterprise = await fetch(`/api/enterprises/${id}`).then(r => r.json());
//   populateProfile(enterprise);

// ── EXAMPLE DATA (remove when DB is connected) ──
const CHART_DATA = {
    sales: {
        // TODO (DB): replace with real sales data from backend
        labels: ['Day 1', 'Day 2', 'Day 3', 'Day 4', 'Day 5'],
        values: [8, 19, 20, 33, 37],
        color: '#4A6CF7',
        title: 'Daily Sales'
    },
    ratings: {
        // TODO (DB): replace with real ratings data from backend
        labels: ['Day 1', 'Day 2', 'Day 3', 'Day 4', 'Day 5'],
        values: [3.2, 3.8, 4.0, 4.3, 4.6],
        color: '#d4a017',
        title: 'Daily Ratings'
    }
};

function drawChart(canvas, dataset) {
    canvas.style.width = '';
    canvas.style.height = '';
    const wrap = canvas.parentElement;
    const W = wrap.clientWidth || 600;
    const H = wrap.clientHeight || 220;

    canvas.width = W;
    canvas.height = H;
    const ctx = canvas.getContext('2d');
    const pad = { top: 20, right: 20, bottom: 56, left: 44 };
    const chartW = W - pad.left - pad.right;
    const chartH = H - pad.top - pad.bottom;
    const vals = dataset.values;
    const labels = dataset.labels;
    const n = vals.length;
    const minVal = 0;
    const maxVal = Math.ceil(Math.max(...vals) * 1.2);
    const color = dataset.color;

    ctx.clearRect(0, 0, W, H);

    function xPos(i) { return pad.left + (i / (n - 1)) * chartW; }
    function yPos(v) { return pad.top + chartH - ((v - minVal) / (maxVal - minVal)) * chartH; }

    const ySteps = 5;
    ctx.font = '11px DM Sans, sans-serif';
    ctx.fillStyle = '#9aa3bf';
    ctx.textAlign = 'right';
    for (let i = 0; i <= ySteps; i++) {
        const v = minVal + (i / ySteps) * (maxVal - minVal);
        const y = yPos(v);
        ctx.strokeStyle = 'rgba(0,0,0,0.06)';
        ctx.lineWidth = 1;
        ctx.beginPath();
        ctx.moveTo(pad.left, y);
        ctx.lineTo(pad.left + chartW, y);
        ctx.stroke();
        ctx.fillText(Math.round(v), pad.left - 6, y + 4);
    }

    ctx.textAlign = 'center';
    ctx.textBaseline = 'top';
    labels.forEach((lbl, i) => {
        ctx.fillStyle = '#9aa3bf';
        ctx.fillText(lbl, xPos(i), pad.top + chartH + 8);
    });

    ctx.beginPath();
    ctx.moveTo(xPos(0), yPos(vals[0]));
    for (let i = 1; i < n; i++) {
        const x0 = xPos(i - 1), y0 = yPos(vals[i - 1]);
        const x1 = xPos(i), y1 = yPos(vals[i]);
        const cpx = (x0 + x1) / 2;
        ctx.bezierCurveTo(cpx, y0, cpx, y1, x1, y1);
    }
    ctx.lineTo(xPos(n - 1), pad.top + chartH);
    ctx.lineTo(xPos(0), pad.top + chartH);
    ctx.closePath();
    const grad = ctx.createLinearGradient(0, pad.top, 0, pad.top + chartH);
    grad.addColorStop(0, color + '30');
    grad.addColorStop(1, color + '05');
    ctx.fillStyle = grad;
    ctx.fill();

    ctx.beginPath();
    ctx.moveTo(xPos(0), yPos(vals[0]));
    for (let i = 1; i < n; i++) {
        const x0 = xPos(i - 1), y0 = yPos(vals[i - 1]);
        const x1 = xPos(i), y1 = yPos(vals[i]);
        const cpx = (x0 + x1) / 2;
        ctx.bezierCurveTo(cpx, y0, cpx, y1, x1, y1);
    }
    ctx.strokeStyle = color;
    ctx.lineWidth = 2.5;
    ctx.lineJoin = 'round';
    ctx.stroke();

    vals.forEach((v, i) => {
        ctx.beginPath();
        ctx.arc(xPos(i), yPos(v), 4, 0, Math.PI * 2);
        ctx.fillStyle = color;
        ctx.fill();
        ctx.strokeStyle = '#fff';
        ctx.lineWidth = 2;
        ctx.stroke();
    });
}

const canvas = document.getElementById('detailChart');
let activeTab = 'sales';

function renderChart(tab) {
    const dataset = CHART_DATA[tab];
    document.getElementById('chartTitle').textContent = dataset.title;
    drawChart(canvas, dataset);
}

window.addEventListener('resize', () => renderChart(activeTab));

const tabSales = document.getElementById('tabSales');
const tabRatings = document.getElementById('tabRatings');

tabSales.addEventListener('click', () => {
    if (activeTab === 'sales') return;
    activeTab = 'sales';
    tabSales.classList.add('active');
    tabRatings.classList.remove('active');
    renderChart('sales');
});

tabRatings.addEventListener('click', () => {
    if (activeTab === 'ratings') return;
    activeTab = 'ratings';
    tabRatings.classList.add('active');
    tabSales.classList.remove('active');
    renderChart('ratings');
});

// ── DELETE MODAL ──
const modalBackdrop = document.getElementById('modalBackdrop');
const deleteBtn = document.getElementById('deleteBtn');
const cancelDelete = document.getElementById('cancelDelete');
const confirmDelete = document.getElementById('confirmDelete');

deleteBtn.addEventListener('click', () => {
    const name = document.getElementById('profileName').textContent;
    document.getElementById('modalEnterpriseName').textContent = name;
    modalBackdrop.classList.add('active');
});

cancelDelete.addEventListener('click', closeModal);

modalBackdrop.addEventListener('click', (e) => {
    if (e.target === modalBackdrop) closeModal();
});

document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && modalBackdrop.classList.contains('active')) closeModal();
});

function closeModal() {
    modalBackdrop.classList.remove('active');
}

confirmDelete.addEventListener('click', () => {
    confirmDelete.textContent = 'Deleting…';
    confirmDelete.disabled = true;
    confirmDelete.style.opacity = '0.75';

    // TODO (DB): replace with real API call:
    //   const id = new URLSearchParams(window.location.search).get('id');
    //   await fetch(`/api/enterprises/${id}`, { method: 'DELETE' });
    //   window.location.href = 'AdminEnterprises.html';
    setTimeout(() => {
        closeModal();
        confirmDelete.textContent = 'Yes, Delete';
        confirmDelete.disabled = false;
        confirmDelete.style.opacity = '';
        alert('Enterprise deleted. (Wire to backend when ready.)');
        window.location.href = 'AdminEnterprises.html';
    }, 1200);
});

// ── PRODUCT SEARCH ──
const productSearchBtn = document.getElementById('productSearchBtn');
const productSearch = document.getElementById('productSearch');

productSearchBtn.addEventListener('click', searchProduct);
productSearch.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') searchProduct();
});

function searchProduct() {
    const query = productSearch.value.trim();
    if (!query) return;
    // TODO (DB): fetch matching product and call populateProduct(result):
    //   const res = await fetch(`/api/enterprises/{id}/products?search=${query}`).then(r => r.json());
    //   if (res.length) populateProduct(res[0]);
    console.log('Product search:', query);
}

// ── VIEW DOCUMENTS / LISTING ──
document.getElementById('viewDocsBtn').addEventListener('click', () => {
    // TODO (DB): navigate to or open documents for this enterprise
    alert('View Documents — wire to backend when ready.');
});

document.getElementById('viewListingBtn').addEventListener('click', () => {
    // TODO (DB): navigate to or open listing request for this enterprise
    alert('View Listing Request — wire to backend when ready.');
});

// ── REMOVE ITEM ──
document.getElementById('removeItemBtn').addEventListener('click', () => {
    // TODO (DB): DELETE /api/enterprises/{id}/products/{productId}
    alert('Remove Item — wire to backend when ready.');
});

// ── HELPER: populate profile from DB record ──
// TODO (DB): call populateProfile(enterprise) after fetching from backend
function populateProfile(enterprise) {
    document.getElementById('profileName').textContent = enterprise.name;
    document.getElementById('profileId').textContent = enterprise.id;
    document.getElementById('profileEmail').textContent = enterprise.email;

    const statusEl = document.getElementById('profileStatus');
    statusEl.textContent = enterprise.status;
    statusEl.className = 'info-value status-' + enterprise.status.toLowerCase();

    const starsEl = document.getElementById('profileStars');
    starsEl.innerHTML = '';
    for (let i = 1; i <= 5; i++) {
        const s = document.createElement('span');
        s.className = 'star' + (i <= enterprise.rating ? ' filled' : '');
        s.textContent = '★';
        starsEl.appendChild(s);
    }

    if (enterprise.profileImg) {
        document.getElementById('profileAvatar').innerHTML =
            `<img src="${enterprise.profileImg}" alt="${enterprise.name}" />`;
    }

    document.getElementById('chartSubtitle').textContent = enterprise.name;

    // TODO (DB): also update CHART_DATA.sales.values and CHART_DATA.ratings.values
    // with real data from the enterprise record, then call renderChart(activeTab)
}

// ── HELPER: populate product panel from DB record ──
// TODO (DB): call populateProduct(product) after product search returns results
function populateProduct(product) {
    document.getElementById('productName').textContent = product.name;
    document.getElementById('productPrice').textContent = '₱' + Number(product.price).toFixed(2);
    if (product.imageUrl) {
        document.getElementById('productImg').src = product.imageUrl;
    }
    // TODO (DB): store product.id somewhere so removeItemBtn can reference it
}

window.addEventListener('load', () => renderChart('sales'));