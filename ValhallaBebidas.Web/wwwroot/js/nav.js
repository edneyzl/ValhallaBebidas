/* ════════════════════════════════════════════════════════
   nav.js — Valhalla Bebidas (MVC)
   Responsável: dropdowns desktop e mobile + estado da nav
   Carrega em: todas as páginas via _Layout
   Depende de: auth.js (isLogado, nomeUser)
════════════════════════════════════════════════════════ */

const navGuest = document.querySelector('.nav__guest');
const navUser = document.getElementById('nav-user');
const userBtn = document.getElementById('userBtn');
const userNameEl = document.getElementById('userName');
const dropDesktop = document.getElementById('dropdown-desktop');

const menuBtn = document.getElementById('menuBtn');
const dropMobile = document.getElementById('dropdown-mobile');
const userNameMobile = document.getElementById('userNameMobile');

/* ── Aplica estado auth ── */
if (isLogado) {
    navGuest.style.display = 'none';
    navUser.style.display = 'block';
    userNameEl.textContent = nomeUser;
    userNameMobile.textContent = nomeUser;
} else {
    navGuest.style.display = 'flex';
    navUser.style.display = 'none';
}

document.querySelectorAll('.dropdown-auth').forEach(el => {
    el.style.display = isLogado ? 'flex' : 'none';
});

document.querySelectorAll('.dropdown-guest').forEach(el => {
    el.style.display = isLogado ? 'none' : 'flex';
});

document.querySelectorAll('.nav__dropdown-divider.dropdown-auth').forEach(el => {
    el.style.display = isLogado ? 'block' : 'none';
});

document.querySelectorAll('.nav__dropdown-divider.dropdown-guest').forEach(el => {
    el.style.display = isLogado ? 'none' : 'block';
});

/* ── Dropdown desktop ── */
userBtn?.addEventListener('click', (e) => {
    e.stopPropagation();
    const isOpen = dropDesktop.classList.toggle('open');
    userBtn.classList.toggle('open', isOpen);
});

document.addEventListener('click', (e) => {
    if (!dropDesktop?.contains(e.target) && !userBtn?.contains(e.target)) {
        dropDesktop?.classList.remove('open');
        userBtn?.classList.remove('open');
    }
});

/* ── Dropdown mobile ── */
menuBtn?.addEventListener('click', (e) => {
    e.stopPropagation();
    dropMobile.classList.toggle('open');
});

document.addEventListener('click', (e) => {
    if (!dropMobile?.contains(e.target) && !menuBtn?.contains(e.target)) {
        dropMobile?.classList.remove('open');
    }
});

dropMobile?.querySelectorAll('.nav__dropdown-item').forEach(item => {
    item.addEventListener('click', () => dropMobile.classList.remove('open'));
});