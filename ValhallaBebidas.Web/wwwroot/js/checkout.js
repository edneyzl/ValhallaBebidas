/* ════════════════════════════════════════════════════════
   checkout.js — Valhalla Bebidas (MVC)
════════════════════════════════════════════════════════ */

window.addEventListener('authCarregado', ({ detail }) => {
    if (!detail.isLogado) window.location.href = '/Auth/Login';
});

const itens = JSON.parse(localStorage.getItem('carrinho') || '[]');
if (itens.length === 0) window.location.href = '/Catalogo';

function escapar(str) {
    const el = document.createElement('div');
    el.textContent = str ?? '';
    return el.innerHTML;
}

function formatar(valor) {
    return `R$ ${Number(valor).toFixed(2).replace('.', ',')}`;
}

function renderizarResumo() {
    const lista = document.getElementById('resumoLista');
    const subtotal = itens.reduce((acc, i) => acc + (Number(i.preco) * Number(i.quantidade)), 0);

    if (lista) {
        lista.innerHTML = itens.map(item => `
            <li class="checkout__resumo-item">
                <div class="checkout__resumo-item-img">
                    ${item.imagem ? `<img src="${item.imagem}" alt="${escapar(item.nome)}" />` : `<span>🍺</span>`}
                </div>
                <div class="checkout__resumo-item-info">
                    <p>${escapar(item.nome)}</p>
                    <span>${item.quantidade}x · ${formatar(item.preco)} cada</span>
                </div>
                <p class="checkout__resumo-item-preco">${formatar(item.preco * item.quantidade)}</p>
            </li>
        `).join('');
    }

    const subEl = document.getElementById('resumoSubtotal');
    const totEl = document.getElementById('resumoTotal');

    if (subEl) subEl.textContent = formatar(subtotal);
    if (totEl) totEl.textContent = formatar(subtotal);
}

renderizarResumo();

/* ── pagamento ── */
let metodoSelecionado = localStorage.getItem('metodoPagamento') || 'cartao';

const paineis = {
    cartao: document.getElementById('painelCartao'),
    pix: document.getElementById('painelPix'),
    boleto: document.getElementById('painelBoleto'),
};

document.querySelectorAll('.checkout__metodo').forEach(btn => {
    btn.classList.toggle('checkout__metodo--active', btn.dataset.metodo === metodoSelecionado);
});

Object.entries(paineis).forEach(([key, painel]) => {
    if (painel) painel.style.display = key === metodoSelecionado ? 'flex' : 'none';
});

document.querySelectorAll('.checkout__metodo').forEach(btn => {
    btn.addEventListener('click', () => {
        metodoSelecionado = btn.dataset.metodo;
        localStorage.setItem('metodoPagamento', metodoSelecionado);

        document.querySelectorAll('.checkout__metodo').forEach(b => {
            b.classList.toggle('checkout__metodo--active', b === btn);
        });

        Object.entries(paineis).forEach(([key, painel]) => {
            if (painel) painel.style.display = key === metodoSelecionado ? 'flex' : 'none';
        });
    });
});

/* ── máscaras ── */
document.getElementById('cep')?.addEventListener('input', e => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 5) v = v.slice(0, 5) + '-' + v.slice(5);
    e.target.value = v.slice(0, 9);
});

document.getElementById('estado')?.addEventListener('input', e => {
    e.target.value = e.target.value.toUpperCase().replace(/[^A-Z]/g, '');
});

/* ── buscar CEP ── */
document.getElementById('cep')?.addEventListener('blur', async e => {
    const cep = e.target.value.replace(/\D/g, '');

    if (cep.length !== 8) return;

    try {
        const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);

        if (!response.ok) {
            console.warn('CEP não encontrado.');
            return;
        }

        const data = await response.json();

        if (data.erro) {
            console.warn('CEP inválido.');
            return;
        }

        const logradouroEl = document.getElementById('logradouro');
        const bairroEl = document.getElementById('bairro');
        const cidadeEl = document.getElementById('cidade');
        const estadoEl = document.getElementById('estado');

        if (logradouroEl && !logradouroEl.value.trim()) logradouroEl.value = data.logradouro || '';
        if (bairroEl && !bairroEl.value.trim()) bairroEl.value = data.bairro || '';
        if (cidadeEl && !cidadeEl.value.trim()) cidadeEl.value = data.localidade || '';
        if (estadoEl && !estadoEl.value.trim()) estadoEl.value = data.uf || '';

        console.log('CEP encontrado:', data);
    } catch (err) {
        console.error('Erro ao buscar CEP:', err);
    }
});

/* ── validação ── */
function obterEntrega() {
    return {
        logradouro: document.getElementById('logradouro')?.value.trim() || '',
        numero: document.getElementById('numero')?.value.trim() || '',
        complemento: document.getElementById('complemento')?.value.trim() || '',
        bairro: document.getElementById('bairro')?.value.trim() || '',
        cidade: document.getElementById('cidade')?.value.trim() || '',
        estado: document.getElementById('estado')?.value.trim() || '',
        cep: document.getElementById('cep')?.value.replace(/\D/g, '') || ''
    };
}

function validarEntrega(entrega) {
    if (!entrega.logradouro) return 'Informe o logradouro.';
    if (!entrega.numero) return 'Informe o número.';
    if (!entrega.bairro) return 'Informe o bairro.';
    if (!entrega.cidade) return 'Informe a cidade.';
    if (!entrega.estado || entrega.estado.length !== 2) return 'Informe a UF.';
    if (!entrega.cep || entrega.cep.length !== 8) return 'Informe um CEP válido.';
    return null;
}

/* ── submit ── */
document.getElementById('btnConfirmar')?.addEventListener('click', async () => {
    const entrega = obterEntrega();
    const erroEntrega = validarEntrega(entrega);

    if (erroEntrega) {
        alert(erroEntrega);
        return;
    }

    const payload = {
        itens: itens.map(i => ({
            produtoId: Number(i.id),
            quantidade: Number(i.quantidade)
        })),
        entrega
    };

    console.log('Payload checkout:', payload);

    try {
        const response = await fetch('/Carrinho/CheckoutApi', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

        const data = await response.json();

        if (response.status === 401) {
            window.location.href = '/Auth/Login';
            return;
        }

        if (!response.ok) {
            alert(data.mensagem || 'Erro ao processar pedido.');
            return;
        }

        localStorage.setItem('ultimoPedido', localStorage.getItem('carrinho') || '[]');
        localStorage.removeItem('carrinho');
        window.location.href = `/Carrinho/Confirmacao?pedido=${data.pedidoId}`;
    } catch (err) {
        console.error(err);
        alert('Erro de conexão. Tente novamente.');
    }
});