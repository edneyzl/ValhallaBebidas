/* ════════════════════════════════════════════════════════
   cadastro.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

if (isLogado) window.location.href = '/';

/* ── Dropdown mobile ── */
const cadastroMenuBtn = document.getElementById('cadastroMenuBtn');
const cadastroDropMobile = document.getElementById('cadastroDropMobile');

cadastroMenuBtn?.addEventListener('click', (e) => {
    e.stopPropagation();
    cadastroDropMobile?.classList.toggle('open');
});

document.addEventListener('click', (e) => {
    if (!cadastroDropMobile?.contains(e.target) && !cadastroMenuBtn?.contains(e.target))
        cadastroDropMobile?.classList.remove('open');
});

cadastroDropMobile?.querySelectorAll('.nav__dropdown-item').forEach(item =>
    item.addEventListener('click', () => cadastroDropMobile.classList.remove('open'))
);

/* ── Máscaras ── */
document.getElementById('dataNascimento')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 2) v = v.slice(0, 2) + '/' + v.slice(2);
    if (v.length > 5) v = v.slice(0, 5) + '/' + v.slice(5);
    e.target.value = v.slice(0, 10);
});

document.getElementById('documento')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length <= 11) {
        if (v.length > 3) v = v.slice(0, 3) + '.' + v.slice(3);
        if (v.length > 7) v = v.slice(0, 7) + '.' + v.slice(7);
        if (v.length > 11) v = v.slice(0, 11) + '-' + v.slice(11);
        e.target.value = v.slice(0, 14);
    } else {
        if (v.length > 2) v = v.slice(0, 2) + '.' + v.slice(2);
        if (v.length > 6) v = v.slice(0, 6) + '.' + v.slice(6);
        if (v.length > 10) v = v.slice(0, 10) + '/' + v.slice(10);
        if (v.length > 15) v = v.slice(0, 15) + '-' + v.slice(15);
        e.target.value = v.slice(0, 18);
    }
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
        document.getElementById('logradouro').value = data.logradouro || '';
        document.getElementById('bairro').value = data.bairro || '';
        document.getElementById('cidade').value = data.localidade || '';
        document.getElementById('estado').value = data.uf || '';
        document.getElementById('numero')?.focus();
        limparErro('erroCep');
    } catch { mostrarErro('erroCep', 'Erro ao buscar CEP.'); }
});

/* ── Erros ── */
function mostrarErro(id, msg) { const el = document.getElementById(id); if (el) el.textContent = msg; }
function limparErro(id) { const el = document.getElementById(id); if (el) el.textContent = ''; }
function limparTodosErros() { document.querySelectorAll('.cadastro__input-erro').forEach(el => el.textContent = ''); }

function calcularIdade(dataNasc) {
    const [dia, mes, ano] = dataNasc.split('/').map(Number);
    const nasc = new Date(ano, mes - 1, dia);
    const hoje = new Date();
    let idade = hoje.getFullYear() - nasc.getFullYear();
    const m = hoje.getMonth() - nasc.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;
    return idade;
}

function validarFormulario(d) {
    let ok = true;
    limparTodosErros();
    if (d.nome.length < 3) { mostrarErro('erroNome', 'Mínimo 3 caracteres.'); ok = false; }
    if (!/^\d{2}\/\d{2}\/\d{4}$/.test(d.dataNascimento)) { mostrarErro('erroDataNascimento', 'Formato DD/MM/YYYY.'); ok = false; }
    else if (calcularIdade(d.dataNascimento) < 18) { mostrarErro('erroDataNascimento', 'Mínimo 18 anos.'); ok = false; }
    const docLimpo = d.documento.replace(/\D/g, '');
    if (docLimpo.length !== 11 && docLimpo.length !== 14) { mostrarErro('erroDocumento', 'CPF ou CNPJ inválido.'); ok = false; }
    const telLimpo = d.telefone.replace(/\D/g, '');
    if (telLimpo.length < 10 || telLimpo.length > 11) { mostrarErro('erroTelefone', 'Telefone inválido.'); ok = false; }
    if (!d.email.includes('@') || !d.email.includes('.')) { mostrarErro('erroEmail', 'Email inválido.'); ok = false; }
    if (d.senha.length < 6) { mostrarErro('erroSenha', 'Mínimo 6 caracteres.'); ok = false; }
    if (d.senha !== d.confirmarSenha) { mostrarErro('erroConfirmarSenha', 'Senhas não conferem.'); ok = false; }
    if (d.cep.replace(/\D/g, '').length !== 8) { mostrarErro('erroCep', 'CEP inválido.'); ok = false; }
    if (!d.logradouro.trim()) { mostrarErro('erroLogradouro', 'Informe o logradouro.'); ok = false; }
    if (!d.numero.trim()) { mostrarErro('erroNumero', 'Informe o número.'); ok = false; }
    if (!d.bairro.trim()) { mostrarErro('erroBairro', 'Informe o bairro.'); ok = false; }
    if (!d.cidade.trim()) { mostrarErro('erroCidade', 'Informe a cidade.'); ok = false; }
    if (d.estado.length !== 2) { mostrarErro('erroEstado', 'UF inválida.'); ok = false; }
    return ok;
}

/* ── Toast ── */
function mostrarToast() {
    const toast = document.getElementById('toast');
    if (!toast) return;
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 2000);
}

/* ── Submit — deixa o form submeter ao servidor (POST /Auth/Cadastro) ── */
document.getElementById('formCadastro')?.addEventListener('submit', (e) => {
    const dados = {
        nome: document.getElementById('nome').value.trim(),
        dataNascimento: document.getElementById('dataNascimento').value.trim(),
        documento: document.getElementById('documento').value.trim(),
        telefone: document.getElementById('telefone').value.trim(),
        email: document.getElementById('email').value.trim(),
        senha: document.getElementById('senha').value,
        confirmarSenha: document.getElementById('confirmarSenha').value,
        logradouro: document.getElementById('logradouro').value.trim(),
        cep: document.getElementById('cep').value.trim(),
        numero: document.getElementById('numero').value.trim(),
        complemento: document.getElementById('complemento').value.trim(),
        bairro: document.getElementById('bairro').value.trim(),
        cidade: document.getElementById('cidade').value.trim(),
        estado: document.getElementById('estado').value.trim(),
    };

    if (!validarFormulario(dados)) {
        e.preventDefault();
        return;
    }

    /* Válido — permite submit normal ao servidor.
       O AuthController valida no server-side e chama a API real.
       Se a API rejeitar (email duplicado, etc.) o ViewBag.Erro aparece na view. */
    console.log('Submit cadastro:', dados);
    mostrarToast();
});