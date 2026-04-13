/* ════════════════════════════════════════════════════════
   auth.js — Valhalla Bebidas (MVC — Session segura)
   Responsável: busca estado de auth da Session do servidor
   Carrega em: todas as páginas via _Layout

   Correções:
   - não apaga mais o carrinho automaticamente
   - evita estado inconsistente antes da sessão carregar
   - mantém navbar/cart sincronizados com a session
════════════════════════════════════════════════════════ */

let isLogado = false;
let nomeUser = 'Usuário';
let authJaCarregada = false;

/* ── Busca session no servidor ── */
async function carregarSession() {
    try {
        const res = await fetch('/api/session', {
            method: 'GET',
            credentials: 'include'
        });

        if (!res.ok) {
            throw new Error('Falha ao carregar sessão');
        }

        const data = await res.json();

        isLogado = !!data.isLogado;
        nomeUser = data.nome || 'Usuário';

        aplicarEstadoAuth();
    } catch {
        isLogado = false;
        nomeUser = 'Usuário';
        aplicarEstadoAuth();
    }
}

/* ── Aplica estado na UI ── */
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
        if (userNameEl) userNameEl.textContent = 'Usuário';
        if (userNameMob) userNameMob.textContent = 'Usuário';
        if (cartBtn) cartBtn.style.display = 'none';

        /* IMPORTANTE:
           não apagar o carrinho aqui.
           Isso fazia o localStorage resetar ao navegar.
        */
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

    authJaCarregada = true;

    window.dispatchEvent(new CustomEvent('authCarregado', {
        detail: {
            isLogado,
            nomeUser
        }
    }));
}

/* ── Logout ── */
function logout() {
    /* aqui sim pode limpar o carrinho, se esse for o comportamento desejado */
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

/* ── Helpers opcionais ── */
window.authState = {
    get isLogado() {
        return isLogado;
    },
    get nomeUser() {
        return nomeUser;
    },
    get carregado() {
        return authJaCarregada;
    }
};

/* ── Init ── */
carregarSession();