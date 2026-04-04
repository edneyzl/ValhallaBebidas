/* ════════════════════════════════════════════════════════
   auth.js — Valhalla Bebidas (MVC)
   Responsável: estado de autenticação e logout
   Carrega em: todas as páginas via _Layout
════════════════════════════════════════════════════════ */

const isLogado = localStorage.getItem('logado') === 'true';
const nomeUser = localStorage.getItem('nomeUser') || 'Usuário';

/* ── Logout ── */
function logout() {
  localStorage.removeItem('logado');
  localStorage.removeItem('nomeUser');
  localStorage.removeItem('carrinho');
  window.location.href = '/';
}

document.getElementById('btn-logout-desktop')?.addEventListener('click', (e) => {
  e.preventDefault();
  logout();
});

document.getElementById('btn-logout-mobile')?.addEventListener('click', (e) => {
  e.preventDefault();
  logout();
});