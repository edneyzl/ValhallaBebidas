/* ════════════════════════════════════════════════════════
   perfil.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado, nomeUser)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

const usuarioMock = {
    nome: nomeUser,
    email: localStorage.getItem('emailUser') || 'usuario@email.com',
    documento: '123.456.789-00',
    telefone: '(11) 99999-9999',
    dataNascimento: '01/01/1990',
    endereco: {
        logradouro: 'Rua das Bebidas', numero: '123', complemento: 'Apto 45',
        bairro: 'Centro', cep: '01310-100', cidade: 'São Paulo', estado: 'SP',
    }
};

/* ── Header ── */
const perfilNome = document.getElementById('perfilNome');
const perfilEmail = document.getElementById('perfilEmail');
const perfilInicial = document.getElementById('perfilInicial');
if (perfilNome) perfilNome.textContent = usuarioMock.nome;
if (perfilEmail) perfilEmail.textContent = usuarioMock.email;
if (perfilInicial) perfilInicial.textContent = usuarioMock.nome.charAt(0).toUpperCase();

/* ── Preenche campos ── */
const set = (id, val) => { const el = document.getElementById(id); if (el) el.value = val; };
set('nome', usuarioMock.nome);
set('email', usuarioMock.email);
set('documento', usuarioMock.documento);
set('telefone', usuarioMock.telefone);
set('dataNascimento', usuarioMock.dataNascimento);
set('logradouro', usuarioMock.endereco.logradouro);
set('numero', usuarioMock.endereco.numero);
set('complemento', usuarioMock.endereco.complemento);
set('bairro', usuarioMock.endereco.bairro);
set('cep', usuarioMock.endereco.cep);
set('cidade', usuarioMock.endereco.cidade);
set('estado', usuarioMock.endereco.estado);

/* ── Tabs ── */
const tabs = {
    dados: document.getElementById('tabDados'),
    endereco: document.getElementById('tabEndereco'),
    senha: document.getElementById('tabSenha'),
};

document.querySelectorAll('.perfil__nav-item[data-tab]').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.perfil__nav-item[data-tab]').forEach(b =>
            b.classList.toggle('perfil__nav-item--active', b === btn)
        );
        Object.entries(tabs).forEach(([key, el]) => {
            if (el) el.style.display = key === btn.dataset.tab ? 'block' : 'none';
        });
    });
});

/* ── Logout ── */
document.getElementById('btnLogout')?.addEventListener('click', () => {
    localStorage.removeItem('logado');
    localStorage.removeItem('nomeUser');
    localStorage.removeItem('carrinho');
    window.location.href = '/';
});

/* ── Máscaras ── */
document.getElementById('dataNascimento')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 2) v = v.slice(0, 2) + '/' + v.slice(2);
    if (v.length > 5) v = v.slice(0, 5) + '/' + v.slice(5);
    e.target.value = v.slice(0, 10);
});
document.getElementById('telefone')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 0) v = '(' + v;
    if (v.length > 3) v = v.slice(0, 3) + ') ' + v.slice(3);
    if (v.length > 10) v = v.slice(0, 10) + '-' + v.slice(10);
    e.target.value = v.slice(0, 15);
});
document.getElementById('cep')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 5) v = v.slice(0, 5) + '-' + v.slice(5);
    e.target.value = v.slice(0, 9);
});
document.getElementById('estado')?.addEventListener('input', (e) => {
    e.target.value = e.target.value.toUpperCase().replace(/[^A-Z]/g, '');
});

/* ── CEP ── */
document.getElementById('cep')?.addEventListener('blur', async (e) => {
    const cep = e.target.value.replace(/\D/g, '');
    if (cep.length !== 8) return;
    try {
        const res = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
        const data = await res.json();
        if (data.erro) return;
        set('logradouro', data.logradouro || '');
        set('bairro', data.bairro || '');
        set('cidade', data.localidade || '');
        set('estado', data.uf || '');
        document.getElementById('numero')?.focus();
    } catch { /* silencia */ }
});

/* ── Salvar dados ── */
document.getElementById('btnSalvarDados')?.addEventListener('click', () => {
    const nome = document.getElementById('nome')?.value.trim();
    localStorage.setItem('nomeUser', nome?.split(' ')[0] || nomeUser);
    mostrarFeedback('Dados atualizados com sucesso!');
});

/* ── Salvar endereço ── */
document.getElementById('btnSalvarEndereco')?.addEventListener('click', () => {
    mostrarFeedback('Endereço atualizado com sucesso!');
});

/* ── Salvar senha ── */
document.getElementById('btnSalvarSenha')?.addEventListener('click', () => {
    const nova = document.getElementById('novaSenha')?.value;
    const confirmar = document.getElementById('confirmarSenha')?.value;
    const erroEl = document.getElementById('erroSenha');
    if (erroEl) erroEl.textContent = '';
    if (!nova || nova.length < 6) { if (erroEl) erroEl.textContent = 'Mínimo 6 caracteres.'; return; }
    if (nova !== confirmar) { if (erroEl) erroEl.textContent = 'Senhas não conferem.'; return; }
    mostrarFeedback('Senha alterada com sucesso!');
    ['senhaAtual', 'novaSenha', 'confirmarSenha'].forEach(id => { const el = document.getElementById(id); if (el) el.value = ''; });
});

/* ── Toast ── */
function mostrarFeedback(mensagem) {
    const toast = document.createElement('div');
    toast.textContent = mensagem;
    toast.style.cssText = `
    position:fixed;bottom:32px;right:32px;background:#161616;
    border:1px solid #D6BD77;color:#fff;padding:16px 24px;
    border-radius:12px;font-size:14px;font-family:Sora,sans-serif;
    z-index:9999;opacity:0;transition:opacity .3s ease;
  `;
    document.body.appendChild(toast);
    requestAnimationFrame(() => toast.style.opacity = '1');
    setTimeout(() => { toast.style.opacity = '0'; setTimeout(() => toast.remove(), 300); }, 2500);
}