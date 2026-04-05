/* ════════════════════════════════════════════════════════
   login.js — Valhalla Bebidas (MVC — Session segura)
   O submit agora vai para o Controller via form POST
   O controller valida e cria a Session no servidor
════════════════════════════════════════════════════════ */

/* ── Dropdown mobile ── */
const loginMenuBtn = document.getElementById('loginMenuBtn');
const loginDropMobile = document.getElementById('loginDropMobile');

loginMenuBtn?.addEventListener('click', (e) => {
    e.stopPropagation();
    loginDropMobile?.classList.toggle('open');
});

document.addEventListener('click', (e) => {
    if (!loginDropMobile?.contains(e.target) && !loginMenuBtn?.contains(e.target))
        loginDropMobile?.classList.remove('open');
});

/* ── Erros por campo ── */
function mostrarErroForm(mensagem) {
    // Remove erro anterior se existir
    const existente = document.querySelector('.login__erro');
    if (existente) existente.remove();

    // Cria erro dinâmico dentro do form
    const erro = document.createElement('p');
    erro.className = 'login__erro';
    erro.textContent = mensagem;
    const actions = document.querySelector('.login__actions');
    actions?.insertBefore(erro, actions.firstChild);
}

function limparErroForm() {
    const existente = document.querySelector('.login__erro');
    if (existente) existente.remove();
}

/* ════════════════════════════════════════════════════════
   TOGGLE SENHA — Mostrar / ocultar
════════════════════════════════════════════════════════ */
const toggleSenha = document.getElementById('toggleSenha');

toggleSenha?.addEventListener('click', () => {
    const input = document.getElementById('senha');
    const isPassword = input.type === 'password';
    input.type = isPassword ? 'text' : 'password';

    // Troca o ícone visível
    const eyeOpen = toggleSenha.querySelector('.eye-open');
    const eyeClose = toggleSenha.querySelector('.eye-close');
    if (isPassword) { eyeOpen.style.display = 'none'; eyeClose.style.display = 'block'; }
    else { eyeOpen.style.display = 'block'; eyeClose.style.display = 'none'; }

    // Atualiza aria-label para acessibilidade
    toggleSenha.setAttribute('aria-label', isPassword ? 'Ocultar senha' : 'Mostrar senha');
});

/* ── Validação antes de submeter ── */
const formLogin = document.getElementById('formLogin');

formLogin?.addEventListener('submit', (e) => {
    const email = document.getElementById('email')?.value.trim();
    const senha = document.getElementById('senha')?.value;

    limparErroForm();

    if (!email || !email.includes('@') || !email.includes('.')) {
        mostrarErroForm('Digite um email válido.');
        e.preventDefault();
        return;
    }

    if (!senha || senha.length < 6) {
        mostrarErroForm('Senha deve ter pelo menos 6 caracteres.');
        e.preventDefault();
        return;
    }

    /* Se válido o form submete normalmente para POST /Auth/Login
       O controller trata a autenticação e cria a Session */
});