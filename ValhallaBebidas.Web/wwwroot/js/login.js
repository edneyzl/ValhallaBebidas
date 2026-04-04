/* ════════════════════════════════════════════════════════
   login.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

/* ── Redireciona se já logado ── */
if (isLogado) window.location.href = '/';

/* ── Dropdown mobile do login ── */
const loginMenuBtn = document.getElementById('loginMenuBtn');
const loginDropMobile = document.getElementById('loginDropMobile');

loginMenuBtn?.addEventListener('click', (e) => {
    e.stopPropagation();
    loginDropMobile?.classList.toggle('open');
});

document.addEventListener('click', (e) => {
    if (!loginDropMobile?.contains(e.target) && !loginMenuBtn?.contains(e.target)) {
        loginDropMobile?.classList.remove('open');
    }
});

loginDropMobile?.querySelectorAll('.nav__dropdown-item').forEach(item =>
    item.addEventListener('click', () => loginDropMobile.classList.remove('open'))
);

/* ── Erros por campo ── */
function mostrarErroInput(id, mensagem) {
    const el = document.getElementById(id);
    if (el) el.textContent = mensagem;
}

function limparErroInput(id) {
    const el = document.getElementById(id);
    if (el) el.textContent = '';
}

/* ── Submit ── */
const formLogin = document.getElementById('formLogin');

formLogin?.addEventListener('submit', async (e) => {
    e.preventDefault();

    const email = document.getElementById('email').value.trim();
    const senha = document.getElementById('senha').value;

    limparErroInput('erroEmail');
    limparErroInput('erroSenha');

    /* Validações frontend */
    if (!email.includes('@') || !email.includes('.')) {
        mostrarErroInput('erroEmail', 'Digite um email válido.');
        return;
    }

    if (senha.length < 6) {
        mostrarErroInput('erroSenha', 'Senha deve ter pelo menos 6 caracteres.');
        return;
    }

    /* ── Simulação ── */
    const usuarioFake = { id: 1, nome: 'João Silva', email, sucesso: true, mensagem: 'Login realizado.' };
    console.log('Payload login:', { email, senha });

    if (usuarioFake.sucesso) {
        localStorage.setItem('logado', 'true');
        localStorage.setItem('nomeUser', usuarioFake.nome.split(' ')[0]);
        window.location.href = '/';
    }

    /* ── API — descomenta quando pronto ── */
    /*
    try {
      const response = await fetch('https://localhost:7001/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ email, senha })
      });
      const data = await response.json();
      if (!response.ok) { mostrarErroInput('erroEmail', data.mensagem); return; }
      localStorage.setItem('logado', 'true');
      localStorage.setItem('nomeUser', data.nome.split(' ')[0]);
      window.location.href = '/';
    } catch { mostrarErroInput('erroEmail', 'Erro de conexão.'); }
    */
});