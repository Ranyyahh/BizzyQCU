const logoutBtn = document.getElementById('logoutBtn');
const modalBackdrop = document.getElementById('modalBackdrop');
const cancelLogout = document.getElementById('cancelLogout');
const confirmLogout = document.getElementById('confirmLogout');
const panelUsers = document.getElementById('panelUsers');
const panelEnterprises = document.getElementById('panelEnterprises');

logoutBtn.addEventListener('click', () => {
    modalBackdrop.classList.add('active');
    document.body.style.overflow = 'hidden';
});

cancelLogout.addEventListener('click', closeModal);

modalBackdrop.addEventListener('click', (e) => {
    if (e.target === modalBackdrop) closeModal();
});
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && modalBackdrop.classList.contains('active')) {
        closeModal();
    }
});

function closeModal() {
    modalBackdrop.classList.remove('active');
    document.body.style.overflow = '';
}
confirmLogout.addEventListener('click', () => {
    confirmLogout.textContent = 'Logging out…';
    confirmLogout.disabled = true;
    confirmLogout.style.opacity = '0.75';

    setTimeout(() => {
        alert('You have been logged out.');
        closeModal();
        confirmLogout.textContent = 'Yes, Log Out';
        confirmLogout.disabled = false;
        confirmLogout.style.opacity = '';
    }, 1200);
});

panelUsers.addEventListener('click', (e) => {
    e.preventDefault();
    navigateTo('users');
});

panelEnterprises.addEventListener('click', (e) => {
    e.preventDefault();
    navigateTo('enterprises');
});

function navigateTo(destination) {
    const panel = destination === 'users' ? panelUsers : panelEnterprises;
    panel.style.opacity = '0.7';
    panel.style.transition = 'opacity 0.3s ease';

    setTimeout(() => {
        alert(`Navigating to: ${destination.toUpperCase()} dashboard`);
        panel.style.opacity = '';
    }, 280);
}
document.addEventListener('mousemove', (e) => {
    const cx = window.innerWidth / 2;
    const cy = window.innerHeight / 2;
    const dx = (e.clientX - cx) / cx;
    const dy = (e.clientY - cy) / cy;

    const leftImg = document.querySelector('.panel-image-left');
    const rightImg = document.querySelector('.panel-image-right');

    if (leftImg) leftImg.style.transform = `translate(${dx * -8}px, ${dy * -6}px) scale(1.04)`;
    if (rightImg) rightImg.style.transform = `translate(${dx * 8}px, ${dy * -6}px) scale(1.04)`;
});