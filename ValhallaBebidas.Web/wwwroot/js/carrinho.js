/* ════════════════════════════════════════════════════════
   carrinho.js — Valhalla Bebidas (MVC)
   Página Meu Carrinho
   Regra:
   - renderiza sempre do localStorage
   - auth só controla redirecionamento, não a renderização
════════════════════════════════════════════════════════ */

const lista = document.getElementById('carrinhoLista');
const vazio = document.getElementById('carrinhoVazio');
const count = document.getElementById('carrinhoCount');
const resumoSubtotal = document.getElementById('resumoSubtotal');
const resumoTotal = document.getElementById('resumoTotal');
const btnCheckout = document.getElementById('btnCheckout');

/* ── storage ── */
function carregarCarrinho() {
    try {
        const raw = localStorage.getItem('carrinho');
        if (!raw) return [];

        const itens = JSON.parse(raw);
        if (!Array.isArray(itens)) return [];

        return itens
            .map(item => ({
                id: String(item.id),
                nome: item.nome || '',
                imagem: item.imagem || '',
                preco: Number(item.preco) || 0,
                quantidade: Number(item.quantidade) || 0
            }))
            .filter(item => item.quantidade > 0);
    } catch (e) {
        console.error('Erro ao carregar carrinho:', e);
        return [];
    }
}

function salvarCarrinho(itens) {
    localStorage.setItem('carrinho', JSON.stringify(itens));
}

function formatarMoeda(valor) {
    return `R$ ${Number(valor).toFixed(2).replace('.', ',')}`;
}

function escaparHtml(texto) {
    const div = document.createElement('div');
    div.textContent = texto ?? '';
    return div.innerHTML;
}

/* ── ações ── */
function alterarQtd(id, delta) {
    const itens = carregarCarrinho();
    const index = itens.findIndex(i => String(i.id) === String(id));

    if (index === -1) return;

    itens[index].quantidade += Number(delta);

    if (itens[index].quantidade <= 0) {
        itens.splice(index, 1);
    }

    salvarCarrinho(itens);
    renderizar();
}

function removerItem(id) {
    const itens = carregarCarrinho().filter(i => String(i.id) !== String(id));
    salvarCarrinho(itens);
    renderizar();
}

/* ── totais ── */
function atualizarTotais(itens) {
    const subtotal = itens.reduce((acc, i) => acc + (i.preco * i.quantidade), 0);

    if (resumoSubtotal) resumoSubtotal.textContent = formatarMoeda(subtotal);
    if (resumoTotal) resumoTotal.textContent = formatarMoeda(subtotal);

    if (count) {
        const totalItens = itens.reduce((acc, i) => acc + i.quantidade, 0);
        count.textContent = `${totalItens} ${totalItens === 1 ? 'item' : 'itens'}`;
    }
}

/* ── render ── */
function renderizar() {
    if (!lista) {
        console.error('Elemento #carrinhoLista não encontrado');
        return;
    }

    const itens = carregarCarrinho();
    console.log('Carrinho carregado:', itens);

    if (itens.length === 0) {
        lista.innerHTML = '';
        if (vazio) vazio.style.display = 'flex';

        if (btnCheckout) {
            btnCheckout.style.pointerEvents = 'none';
            btnCheckout.style.opacity = '0.5';
        }

        atualizarTotais([]);
        return;
    }

    if (vazio) vazio.style.display = 'none';

    if (btnCheckout) {
        btnCheckout.style.pointerEvents = 'auto';
        btnCheckout.style.opacity = '1';
        btnCheckout.href = '/Carrinho/Checkout';
    }

    lista.innerHTML = itens.map(item => {
        const subtotal = item.preco * item.quantidade;

        return `
            <li class="carrinho__item" data-id="${escaparHtml(String(item.id))}">
                <div class="carrinho__item-produto">
                    <div class="carrinho__item-img">
                        ${item.imagem
                ? `<img src="${escaparHtml(item.imagem)}" alt="${escaparHtml(item.nome)}" />`
                : `<span>🍺</span>`}
                    </div>

                    <div class="carrinho__item-info">
                        <p class="carrinho__item-nome">${escaparHtml(item.nome)}</p>
                        <button type="button"
                                class="carrinho__item-remover"
                                data-action="remover"
                                data-id="${escaparHtml(String(item.id))}">
                            Remover
                        </button>
                    </div>
                </div>

                <p class="carrinho__item-preco">${formatarMoeda(item.preco)}</p>

                <div class="carrinho__item-qty">
                    <button type="button"
                            class="carrinho__qty-btn"
                            data-action="menos"
                            data-id="${escaparHtml(String(item.id))}">
                        −
                    </button>

                    <span class="carrinho__qty-num">${item.quantidade}</span>

                    <button type="button"
                            class="carrinho__qty-btn"
                            data-action="mais"
                            data-id="${escaparHtml(String(item.id))}">
                        +
                    </button>
                </div>

                <p class="carrinho__item-subtotal">${formatarMoeda(subtotal)}</p>
            </li>
        `;
    }).join('');

    atualizarTotais(itens);
}

/* ── eventos ── */
lista?.addEventListener('click', (e) => {
    const btn = e.target.closest('[data-action]');
    if (!btn) return;

    const action = btn.dataset.action;
    const id = btn.dataset.id;
    if (!id) return;

    if (action === 'mais') {
        alterarQtd(id, 1);
        return;
    }

    if (action === 'menos') {
        alterarQtd(id, -1);
        return;
    }

    if (action === 'remover') {
        removerItem(id);
    }
});

/* ── init ── */
document.addEventListener('DOMContentLoaded', () => {
    renderizar();

    /* auth só depois */
    window.addEventListener('authCarregado', ({ detail }) => {
        if (!detail.isLogado) {
            window.location.href = '/Auth/Login';
        }
    });
});

/* exports */
window.removerItem = removerItem;
window.alterarQtd = alterarQtd;
window.renderizarCarrinhoPagina = renderizar;