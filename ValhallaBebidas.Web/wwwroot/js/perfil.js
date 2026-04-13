/* ════════════════════════════════════════════════════════
   perfil.js — Valhalla Bebidas
   - espera authCarregado
   - carrega dados do cliente
   - trata erro de API sem quebrar quando vier texto/HTML
════════════════════════════════════════════════════════ */

window.addEventListener('authCarregado', ({ detail }) => {
    if (!detail.isLogado) {
        window.location.href = '/Auth/Login';
        return;
    }

    iniciarPerfil(detail);
});

function iniciarPerfil(detail) {
    const perfilNome = document.getElementById('perfilNome');
    const perfilEmail = document.getElementById('perfilEmail');
    const perfilInicial = document.getElementById('perfilInicial');

    const nomeAtual = detail.nome || 'Usuário';
    const emailAtual = detail.email || '';

    if (perfilNome) perfilNome.textContent = nomeAtual;
    if (perfilEmail) perfilEmail.textContent = emailAtual;
    if (perfilInicial) perfilInicial.textContent = nomeAtual.charAt(0).toUpperCase();

    configurarTabs();
    configurarLogout();
    configurarMascaras();
    configurarCep();
    carregarDadosPerfil();
    configurarSalvarDados();
    configurarSalvarEndereco();
    configurarSalvarSenha();
}

async function carregarDadosPerfil() {
    try {
        const sessaoRes = await fetch('/api/session', { credentials: 'include' });
        const sessao = await sessaoRes.json();

        const clienteId = sessao?.userId;
        if (!clienteId) return;

        const response = await fetch(`/api/cliente/${clienteId}`, {
            credentials: 'include'
        });

        if (!response.ok) {
            console.error('Erro ao carregar perfil');
            return;
        }

        const cliente = await response.json();
        console.log('Cliente carregado:', cliente);

        setValue('nome', cliente.nome || '');
        setValue('documento', cliente.documento || '');
        setValue('email', cliente.email || '');
        setValue('telefone', cliente.telefone || '');

        if (cliente.dataNascimento) {
            const data = new Date(cliente.dataNascimento);
            if (!isNaN(data.getTime())) {
                setValue('dataNascimento', data.toLocaleDateString('pt-BR'));
            }
        }

        const endereco = cliente.endereco || {};

        setValue('logradouro', endereco.logradouro || '');
        setValue('numero', endereco.numero ?? '');
        setValue('complemento', endereco.complemento || '');
        setValue('cep', endereco.cep || '');
        setValue('bairro', endereco.bairro || '');
        setValue('cidade', endereco.cidade || '');
        setValue('estado', endereco.estado || '');

        const perfilNome = document.getElementById('perfilNome');
        const perfilEmail = document.getElementById('perfilEmail');
        const perfilInicial = document.getElementById('perfilInicial');

        if (perfilNome) perfilNome.textContent = cliente.nome || 'Usuário';
        if (perfilEmail) perfilEmail.textContent = cliente.email || '';
        if (perfilInicial) perfilInicial.textContent = (cliente.nome || 'U').charAt(0).toUpperCase();

    } catch (err) {
        console.error('Erro ao carregar dados do perfil:', err);
    }
}

function setValue(id, value) {
    const el = document.getElementById(id);
    if (el) el.value = value ?? '';
}

function configurarTabs() {
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
}

function configurarLogout() {
    document.getElementById('btnLogout')?.addEventListener('click', () => {
        window.location.href = '/Auth/Logout';
    });
}

function configurarMascaras() {
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
}

function configurarCep() {
    document.getElementById('cep')?.addEventListener('blur', async (e) => {
        const cep = e.target.value.replace(/\D/g, '');
        if (cep.length !== 8) return;

        try {
            const res = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
            const data = await res.json();
            if (data.erro) return;

            setValue('logradouro', data.logradouro || '');
            setValue('bairro', data.bairro || '');
            setValue('cidade', data.localidade || '');
            setValue('estado', data.uf || '');

            document.getElementById('numero')?.focus();
        } catch { }
    });
}

async function extrairMensagemErro(response, fallback) {
    try {
        const texto = await response.text();
        if (!texto) return fallback;

        try {
            const json = JSON.parse(texto);
            return json.mensagem || json.message || fallback;
        } catch {
            return texto;
        }
    } catch {
        return fallback;
    }
}

function configurarSalvarDados() {
    document.getElementById('btnSalvarDados')?.addEventListener('click', async () => {
        try {
            const sessaoRes = await fetch('/api/session', { credentials: 'include' });
            const sessao = await sessaoRes.json();

            const clienteId = sessao?.userId;
            if (!clienteId) {
                mostrarFeedback('Sessão inválida. Faça login novamente.', true);
                return;
            }

            const nome = document.getElementById('nome')?.value.trim() || '';
            const documento = document.getElementById('documento')?.value.trim() || '';
            const telefone = document.getElementById('telefone')?.value.trim() || '';
            const email = document.getElementById('email')?.value.trim() || '';
            const dataNasc = document.getElementById('dataNascimento')?.value.trim() || '';

            const dataPayload = dataNasc
                ? dataNasc.split('/').reverse().join('-')
                : null;

            const response = await fetch(`/api/cliente/${clienteId}`, {
                method: 'PUT',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    nome,
                    documento,
                    telefone,
                    email,
                    dataNascimento: dataPayload
                })
            });

            if (!response.ok) {
                const mensagem = await extrairMensagemErro(response, 'Erro ao atualizar dados.');
                mostrarFeedback(mensagem, true);
                return;
            }

            mostrarFeedback('Dados atualizados com sucesso!');
        } catch (err) {
            console.error(err);
            mostrarFeedback('Erro de conexão. Tente novamente.', true);
        }
    });
}

function configurarSalvarEndereco() {
    document.getElementById('btnSalvarEndereco')?.addEventListener('click', async () => {
        try {
            const sessaoRes = await fetch('/api/session', { credentials: 'include' });
            const sessao = await sessaoRes.json();

            const clienteId = sessao?.userId;
            if (!clienteId) {
                mostrarFeedback('Sessão inválida.', true);
                return;
            }

            const logradouro = document.getElementById('logradouro')?.value.trim() || '';
            const numero = document.getElementById('numero')?.value.trim() || '';

            if (!logradouro || !numero) {
                mostrarFeedback('Preencha logradouro e número.', true);
                return;
            }

            const response = await fetch(`/api/cliente/${clienteId}/endereco`, {
                method: 'PUT',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    enderecoId: 0,
                    tipoLogradouro: 'Rua',
                    logradouro,
                    numero: parseInt(numero, 10) || 0,
                    complemento: document.getElementById('complemento')?.value.trim() || '',
                    cep: (document.getElementById('cep')?.value || '').replace(/\D/g, ''),
                    bairro: document.getElementById('bairro')?.value.trim() || '',
                    cidade: document.getElementById('cidade')?.value.trim() || '',
                    estado: document.getElementById('estado')?.value.trim() || ''
                })
            });

            if (!response.ok) {
                const mensagem = await extrairMensagemErro(response, 'Erro ao atualizar endereço.');
                mostrarFeedback(mensagem, true);
                return;
            }

            mostrarFeedback('Endereço atualizado com sucesso!');
        } catch (err) {
            console.error(err);
            mostrarFeedback('Erro de conexão.', true);
        }
    });
}

function configurarSalvarSenha() {
    document.getElementById('btnSalvarSenha')?.addEventListener('click', async () => {
        const erroEl = document.getElementById('erroSenha');
        if (erroEl) erroEl.textContent = '';

        const senhaAtual = document.getElementById('senhaAtual')?.value || '';
        const nova = document.getElementById('novaSenha')?.value || '';
        const confirmar = document.getElementById('confirmarSenha')?.value || '';

        if (!nova || nova.length < 6) {
            if (erroEl) erroEl.textContent = 'Mínimo 6 caracteres.';
            return;
        }

        if (nova !== confirmar) {
            if (erroEl) erroEl.textContent = 'Senhas não conferem.';
            return;
        }

        try {
            const sessaoRes = await fetch('/api/session', { credentials: 'include' });
            const sessao = await sessaoRes.json();

            const clienteId = sessao?.userId;
            if (!clienteId) {
                mostrarFeedback('Sessão inválida.', true);
                return;
            }

            const response = await fetch(`/api/cliente/${clienteId}/senha`, {
                method: 'PUT',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    senhaAtual,
                    novaSenha: nova
                })
            });

            if (!response.ok) {
                const mensagem = await extrairMensagemErro(response, 'Erro ao alterar senha.');
                mostrarFeedback(mensagem, true);
                return;
            }

            mostrarFeedback('Senha alterada com sucesso!');

            ['senhaAtual', 'novaSenha', 'confirmarSenha'].forEach(id => {
                const el = document.getElementById(id);
                if (el) el.value = '';
            });
        } catch (err) {
            console.error(err);
            mostrarFeedback('Erro de conexão.', true);
        }
    });
}

function mostrarFeedback(mensagem, erro = false) {
    const toast = document.createElement('div');
    toast.textContent = mensagem;
    toast.style.cssText = `
        position:fixed;
        bottom:32px;
        right:32px;
        background:#161616;
        border:1px solid ${erro ? '#e05c5c' : '#D6BD77'};
        color:#fff;
        padding:16px 24px;
        border-radius:12px;
        font-size:14px;
        font-family:Sora,sans-serif;
        z-index:9999;
        opacity:0;
        transition:opacity .3s ease;
    `;

    document.body.appendChild(toast);
    requestAnimationFrame(() => toast.style.opacity = '1');

    setTimeout(() => {
        toast.style.opacity = '0';
        setTimeout(() => toast.remove(), 300);
    }, 2500);
}