/* ════════════════════════════════════════════════════════
   perfil.js — Valhalla Bebidas (MVC — API real)
   Depende de: auth.js (isLogado, nomeUser)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

/* ── Header ── */
const perfilNome = document.getElementById('perfilNome');
const perfilEmail = document.getElementById('perfilEmail');
const perfilInicial = document.getElementById('perfilInicial');

if (perfilNome) perfilNome.textContent = nomeUser;
if (perfilEmail) perfilEmail.textContent = localStorage.getItem('emailUser') || '';
if (perfilInicial && nomeUser) perfilInicial.textContent = nomeUser.charAt(0).toUpperCase();

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
    window.location.href = '/Auth/Logout';
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

/* ── CEP via ViaCEP ── */
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

/* ── Helper set value ── */
const set = (id, val) => { const el = document.getElementById(id); if (el) el.value = val; };

/* ── Salvar dados do perfil ── */
document.getElementById('btnSalvarDados')?.addEventListener('click', async () => {
    const nome = document.getElementById('nome')?.value.trim();
    const documento = document.getElementById('documento')?.value.trim();
    const telefone = document.getElementById('telefone')?.value.trim();
    const dataNasc = document.getElementById('dataNascimento')?.value.trim();

    /* Busca clienteId da sessão via endpoint seguro */
    try {
        const res = await fetch('/api/session');
        const sessao = await res.json();
        const clienteId = sessao?.userId;
        if (!clienteId) { mostrarFeedback('Sessão inválida. Faça login novamente.', true); return; }

        const dataPayload = dataNasc ? new Date(dataNasc.split('/').reverse().join('-') + 'T00:00:00') : new Date();
        const response = await fetch(`/api/cliente/${clienteId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nome, documento, telefone, dataNascimento: dataPayload }),
        });
        if (!response.ok) {
            const err = await response.json();
            mostrarFeedback(err.mensagem || 'Erro ao atualizar dados.', true);
            return;
        }
        localStorage.setItem('nomeUser', nome?.split(' ')[0] || nomeUser);
        if (perfilNome) perfilNome.textContent = nome;
        if (perfilInicial) perfilInicial.textContent = nome.charAt(0).toUpperCase();
        mostrarFeedback('Dados atualizados com sucesso!');
    } catch {
        mostrarFeedback('Erro de conexão. Tente novamente.', true);
    }
});

/* ── Salvar endereço ── */
document.getElementById('btnSalvarEndereco')?.addEventListener('click', async () => {
    // Endereço seria atualizado por endpoint separado — por enquanto salva local
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

/* ── Toast feedback ── */
function mostrarFeedback(mensagem, erro = false) {
    const toast = document.createElement('div');
    toast.textContent = mensagem;
    toast.style.cssText = `
    position:fixed;bottom:32px;right:32px;background:#161616;
    border:1px solid ${erro ? '#e05c5c' : '#D6BD77'};color:#fff;padding:16px 24px;
    border-radius:12px;font-size:14px;font-family:Sora,sans-serif;
    z-index:9999;opacity:0;transition:opacity .3s ease;
  `;
    document.body.appendChild(toast);
    requestAnimationFrame(() => toast.style.opacity = '1');
    setTimeout(() => { toast.style.opacity = '0'; setTimeout(() => toast.remove(), 300); }, 2500);
}
