/* ════════════════════════════════════════════════════════
   auth.js — Valhalla Bebidas (MVC — Session segura)
   Responsável: busca estado de auth da Session do servidor
   Carrega em: todas as páginas via _Layout
════════════════════════════════════════════════════════ */

/* Estado inicial vazio — será preenchido pelo servidor */
let isLogado = false;
let nomeUser = 'Usuário';

/* ════════════════════════════════════════════════════════
   BUSCA SESSION — GET /api/session
   Substitui o localStorage.getItem('logado')
════════════════════════════════════════════════════════ */
async function carregarSession() {
    try {
        const res = await fetch('/api/session', { credentials: 'include' });
        const data = await res.json();

        isLogado = data.isLogado;
        nomeUser = data.nome || 'Usuário';

        /* Aplica estado na UI após confirmar com servidor */
        aplicarEstadoAuth();

    } catch {
        /* Se falhar, trata como deslogado */
        isLogado = false;
        aplicarEstadoAuth();
    }
}

/* ════════════════════════════════════════════════════════
   APLICA ESTADO — chamado após carregar a session
════════════════════════════════════════════════════════ */
function aplicarEstadoAuth() {
    const navGuest = document.querySelector('.nav__guest');
    const navUser = document.getElementById('nav-user');
    const userNameEl = document.getElementById('userName');
    const userNameMob = document.getElementById('userNameMobile');
    const cartBtn = document.getElementById('cartBtn');

    if (isLogado) {
        if (navGuest) navGuest.style.display = 'none';
        if (navUser) navUser.style.display = 'block';
        if (userNameEl) userNameEl.textContent = nomeUser;
        if (userNameMob) userNameMob.textContent = nomeUser;
        if (cartBtn) cartBtn.style.display = 'flex';
    } else {
        if (navGuest) navGuest.style.display = 'flex';
        if (navUser) navUser.style.display = 'none';
        if (cartBtn) cartBtn.style.display = 'none';
        /* Limpa carrinho se não logado */
        localStorage.removeItem('carrinho');
    }

    /* Itens de auth no mobile */
    document.querySelectorAll('.dropdown-auth').forEach(el =>
        el.style.display = isLogado ? 'flex' : 'none'
    );
    document.querySelectorAll('.dropdown-guest').forEach(el =>
        el.style.display = isLogado ? 'none' : 'flex'
    );
    document.querySelectorAll('.nav__dropdown-divider.dropdown-auth').forEach(el =>
        el.style.display = isLogado ? 'block' : 'none'
    );
    document.querySelectorAll('.nav__dropdown-divider.dropdown-guest').forEach(el =>
        el.style.display = isLogado ? 'none' : 'block'
    );

    /* Inicializa carrinho se logado */
    if (isLogado && typeof inicializarCarrinho === 'function') {
        inicializarCarrinho();
    }

    /* Dispara evento para outros scripts saberem que auth carregou */
    window.dispatchEvent(new CustomEvent('authCarregado', { detail: { isLogado, nomeUser } }));
}

/* ════════════════════════════════════════════════════════
   LOGOUT — redireciona para o controller MVC
════════════════════════════════════════════════════════ */
function logout() {
    localStorage.removeItem('carrinho');
    window.location.href = '/Auth/Logout';
}

document.getElementById('btn-logout-desktop')?.addEventListener('click', (e) => {
    e.preventDefault();
    logout();
});

document.getElementById('btn-logout-mobile')?.addEventListener('click', (e) => {
    e.preventDefault();
    logout();
});

/* ════════════════════════════════════════════════════════
   INIT — executa imediatamente
════════════════════════════════════════════════════════ */
carregarSession();