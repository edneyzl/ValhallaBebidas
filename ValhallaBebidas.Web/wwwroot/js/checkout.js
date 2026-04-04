/* ════════════════════════════════════════════════════════
   checkout.js — Valhalla Bebidas (MVC — seguro)
   Proteção: feita no servidor via AuthFilter
   CSRF: token lido do cookie __RequestVerificationToken
════════════════════════════════════════════════════════ */

/* ── Aguarda auth carregar ── */
window.addEventListener('authCarregado', ({ detail }) => {
    if (!detail.isLogado) window.location.href = '/Auth/Login';
});

const itens = JSON.parse(localStorage.getItem('carrinho') || '[]');
if (itens.length === 0) window.location.href = '/Catalogo';

/* ── Resumo ── */
function renderizarResumo() {
    const lista = document.getElementById('resumoLista');
    const subtotal = itens.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
    const formatar = v => `R$ ${v.toFixed(2).replace('.', ',')}`;

    if (lista) lista.innerHTML = itens.map(item => `
    <li class="checkout__resumo-item">
      <div class="checkout__resumo-item-img">${item.imagem ? `<img src="${item.imagem}" alt="${item.nome}" />` : `<span>🍺</span>`}</div>
      <div class="checkout__resumo-item-info">
        <p>${escapar(item.nome)}</p>
        <span>${item.quantidade}x · ${formatar(item.preco)} cada</span>
      </div>
      <p class="checkout__resumo-item-preco">${formatar(item.preco * item.quantidade)}</p>
    </li>
  `).join('');

    const subEl = document.getElementById('resumoSubtotal');
    const totEl = document.getElementById('resumoTotal');
    if (subEl) subEl.textContent = formatar(subtotal);
    if (totEl) totEl.textContent = formatar(subtotal);
}
renderizarResumo();

/* ── Métodos de pagamento ── */
let metodoSelecionado = localStorage.getItem('metodoPagamento') || 'cartao';
const paineis = {
    cartao: document.getElementById('painelCartao'),
    pix: document.getElementById('painelPix'),
    boleto: document.getElementById('painelBoleto'),
};

document.querySelectorAll('.checkout__metodo').forEach(btn => {
    if (btn.dataset.metodo === metodoSelecionado) {
        btn.classList.add('checkout__metodo--active');
    } else {
        btn.classList.remove('checkout__metodo--active');
    }
});
Object.entries(paineis).forEach(([key, painel]) => {
    if (painel) painel.style.display = key === metodoSelecionado ? 'flex' : 'none';
});

document.querySelectorAll('.checkout__metodo').forEach(btn => {
    btn.addEventListener('click', () => {
        metodoSelecionado = btn.dataset.metodo;
        localStorage.setItem('metodoPagamento', metodoSelecionado);
        document.querySelectorAll('.checkout__metodo').forEach(b =>
            b.classList.toggle('checkout__metodo--active', b === btn)
        );
        Object.entries(paineis).forEach(([key, painel]) => {
            if (painel) painel.style.display = key === metodoSelecionado ? 'flex' : 'none';
        });
    });
});

/* ── Máscaras ── */
document.getElementById('numeroCartao')?.addEventListener('input', (e) => {
    e.target.value = e.target.value.replace(/\D/g, '').slice(0, 16).replace(/(.{4})/g, '$1 ').trim();
});
document.getElementById('validade')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 2) v = v.slice(0, 2) + '/' + v.slice(2);
    e.target.value = v.slice(0, 5);
});
document.getElementById('cvv')?.addEventListener('input', (e) => {
    e.target.value = e.target.value.replace(/\D/g, '').slice(0, 4);
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
    } catch { /* silencia */ }
});

/* ── Sanitização XSS ── */
function escapar(str) {
    const el = document.createElement('div');
    el.textContent = str;
    return el.innerHTML;
}

/* ── Submit seguro com CSRF token ── */
document.getElementById('btnConfirmar')?.addEventListener('click', async () => {
    const payload = {
        itens: itens.map(i => ({
            produtoId: i.id,
            quantidade: i.quantidade,
            nome: i.nome,
            preco: i.preco,
            imagem: i.imagem || '',
        })),
        entrega: {
            logradouro: document.getElementById('logradouro')?.value.trim(),
            numero: document.getElementById('numero')?.value.trim(),
            complemento: document.getElementById('complemento')?.value.trim(),
            bairro: document.getElementById('bairro')?.value.trim(),
            cidade: document.getElementById('cidade')?.value.trim(),
            estado: document.getElementById('estado')?.value.trim(),
            cep: document.getElementById('cep')?.value.replace(/\D/g, ''),
        },
    };

    /* ── Lê CSRF token do cookie ── */
    const csrfToken = document.cookie
        .split(';')
        .find(c => c.trim().startsWith('XSRF-TOKEN='))
        ?.split('=')[1] ?? '';

    try {
        const response = await fetch('/Carrinho/CheckoutApi', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken,
            },
            body: JSON.stringify(payload),
        });

        if (response.status === 401) {
            window.location.href = '/Auth/Login';
            return;
        }

        const data = await response.json();

        if (!response.ok) {
            alert(data.mensagem || 'Erro ao processar pedido.');
            return;
        }

        /* Salva itens para a confirmação e limpa o carrinho */
        localStorage.setItem('ultimoPedido', localStorage.getItem('carrinho') || '[]');
        localStorage.removeItem('carrinho');
        window.location.href = `/Carrinho/Confirmacao?pedido=${data.pedidoId}`;

    } catch {
        alert('Erro de conexão. Tente novamente.');
    }
});