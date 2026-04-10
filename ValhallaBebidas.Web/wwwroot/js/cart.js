/* ════════════════════════════════════════════════════════
   cart.js — Valhalla Bebidas (MVC)
   Responsável: carrinho lateral
   Carrega em: todas as páginas via _Layout
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

const cart = document.getElementById('cart');
const cartOverlay = document.getElementById('cartOverlay');
const cartClose = document.getElementById('cartClose');
const cartBtn = document.getElementById('cartBtn');
const cartCountEl = document.getElementById('cartCount');
const cartItemsEl = document.getElementById('cartItems');
const cartTotalEl = document.getElementById('cartTotal');

if (cart) cart.style.display = 'none';
if (cartBtn) cartBtn.style.display = isLogado ? 'flex' : 'none';
if (typeof isLogado !== 'undefined' && !isLogado) {
    localStorage.removeItem('carrinho');
}

/* ── Abrir / fechar ── */
function abrirCarrinho() {
    if (!cart || !cartOverlay) return;
    cart.style.display = 'flex';
    requestAnimationFrame(() => {
        cart.classList.add('open');
        cartOverlay.classList.add('open');
        document.body.style.overflow = 'hidden';
    });
}

function fecharCarrinho() {
    if (!cart || !cartOverlay) return;
    cart.classList.remove('open');
    cartOverlay.classList.remove('open');
    document.body.style.overflow = '';
    cart.addEventListener('transitionend', () => {
        if (!cart.classList.contains('open')) cart.style.display = 'none';
    }, { once: true });
}

cartBtn?.addEventListener('click', abrirCarrinho);
cartClose?.addEventListener('click', fecharCarrinho);
cartOverlay?.addEventListener('click', fecharCarrinho);

/* ── Sanitização XSS ── */
function escapar(str) {
    if (typeof str !== 'string') return '';
    const el = document.createElement('div');
    el.textContent = str;
    return el.innerHTML;
}

function imagemSegura(url) {
    if (!url || url.startsWith('javascript:') || url.startsWith('data:')) return '';
    return url;
}

/* ── localStorage ── */
function salvarCarrinho(itens) {
    try {
        localStorage.setItem('carrinho', JSON.stringify(itens));
    } catch (e) {
        console.error('Erro ao salvar carrinho:', e);
    }
}

function carregarCarrinho() {
    try {
        const raw = localStorage.getItem('carrinho');
        if (!raw) return [];
        const parsed = JSON.parse(raw);
        if (!Array.isArray(parsed)) return [];
        // Normaliza itens: garante propriedades e tipos corretos
        return parsed.map(item => ({
            id: item.id,
            nome: item.nome || '',
            imagem: item.imagem || '',
            preco: typeof item.preco === 'number' ? item.preco : Number(item.preco) || 0,
            quantidade: Number(item.quantidade) || 0
        })).filter(i => i.quantidade > 0);
    } catch (e) {
        console.error('Erro ao carregar carrinho:', e);
        return [];
    }
}

/* ── UI ── */
function atualizarContador(itens) {
    const total = itens.reduce((acc, item) => acc + (Number(item.quantidade) || 0), 0);
    if (cartCountEl) cartCountEl.textContent = String(total);
}

function atualizarTotal(itens) {
    const total = itens.reduce((acc, item) => acc + (Number(item.preco) || 0) * (Number(item.quantidade) || 0), 0);
    if (cartTotalEl) cartTotalEl.textContent = `R$ ${total.toFixed(2).replace('.', ',')}`;
}

function renderizarItens(itens) {
    if (!cartItemsEl) return;

    if (!Array.isArray(itens) || itens.length === 0) {
        cartItemsEl.innerHTML = `<div class="cart__empty"><p>Seu carrinho está vazio.</p></div>`;
        return;
    }

    cartItemsEl.innerHTML = itens.map(item => `
    <div class="cart__item" data-id="${escapar(String(item.id))}">
      ${imagemSegura(item.imagem) ? `<img class="cart__item-img" src="${imagemSegura(item.imagem)}" alt="${escapar(item.nome)}" onerror="this.style.display='none'" />` : ''}
      <div class="cart__item-info">
        <h3>${escapar(item.nome)}</h3>
        <p>R$ ${((Number(item.preco) || 0) * (Number(item.quantidade) || 0)).toFixed(2).replace('.', ',')}</p>
      </div>
      <div class="cart__item-qty">
        <button type="button" class="cart__qty-menos" data-id="${escapar(String(item.id))}">−</button>
        <span class="cart__qty-value" data-id="${escapar(String(item.id))}">${Number(item.quantidade) || 0}</span>
        <button type="button" class="cart__qty-mais" data-id="${escapar(String(item.id))}">+</button>
      </div>
    </div>
  `).join('');

    // Delegação de eventos alternativa: remove listeners duplicados
    cartItemsEl.querySelectorAll('.cart__qty-mais').forEach(btn => {
        btn.removeEventListener('click', maisHandler);
        btn.addEventListener('click', maisHandler);
    });
    cartItemsEl.querySelectorAll('.cart__qty-menos').forEach(btn => {
        btn.removeEventListener('click', menosHandler);
        btn.addEventListener('click', menosHandler);
    });
}

function maisHandler(e) {
    const id = e.currentTarget?.dataset?.id;
    if (!id) return;
    alterarQtd(id, 1);
}

function menosHandler(e) {
    const id = e.currentTarget?.dataset?.id;
    if (!id) return;
    alterarQtd(id, -1);
}

/* ── Lógica de quantidade ── */
function alterarQtd(id, delta) {
    let itens = carregarCarrinho();
    // Normaliza comparação convertendo ambos para string (cobre ids numéricos e string)
    const index = itens.findIndex(i => String(i.id) === String(id));
    if (index === -1) return;
    const atual = Number(itens[index].quantidade) || 0;
    const nova = atual + Number(delta);
    if (nova <= 0) {
        itens.splice(index, 1);
    } else {
        itens[index].quantidade = nova;
    }
    salvarCarrinho(itens);
    renderizarItens(itens);
    atualizarContador(itens);
    atualizarTotal(itens);
}

/* ── Adicionar produto ao carrinho ── */
function adicionarAoCarrinho(produto) {
    if (!produto || !produto.id) return;
    let itens = carregarCarrinho();

    // Garante tipos corretos
    const produtoId = produto.id;
    const qtd = Number(produto.quantidade) || 1;
    const preco = typeof produto.preco === 'number' ? produto.preco : Number(produto.preco) || 0;

    const index = itens.findIndex(i => String(i.id) === String(produtoId));
    if (index !== -1) {
        itens[index].quantidade = (Number(itens[index].quantidade) || 0) + qtd;
    } else {
        itens.push({
            id: produtoId,
            nome: produto.nome || '',
            imagem: produto.imagem || '',
            preco: preco,
            quantidade: qtd
        });
    }

    salvarCarrinho(itens);
    renderizarItens(itens);
    atualizarContador(itens);
    atualizarTotal(itens);
    abrirCarrinho();
}

/* ── Init ── */
if (isLogado) {
    const itensIniciais = carregarCarrinho();
    renderizarItens(itensIniciais);
    atualizarContador(itensIniciais);
    atualizarTotal(itensIniciais);
}

/* ── Exports para uso externo (opcional) ── */
/* Se você adiciona produtos a partir de outros scripts, chame:
   adicionarAoCarrinho({ id: 123, nome: 'Produto', preco: 10.5, quantidade: 1, imagem: '/img/...' })
*/
window.adicionarAoCarrinho = adicionarAoCarrinho;
window.carregarCarrinho = carregarCarrinho;
window.salvarCarrinho = salvarCarrinho;
