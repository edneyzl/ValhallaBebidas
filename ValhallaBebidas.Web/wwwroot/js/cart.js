/* ════════════════════════════════════════════════════════
   cart.js — Valhalla Bebidas
   Carrinho lateral global
════════════════════════════════════════════════════════ */

(() => {
    const cartSidebar = document.getElementById('cart');
    const cartOverlayEl = document.getElementById('cartOverlay');
    const cartCloseBtn = document.getElementById('cartClose');
    const cartOpenBtn = document.getElementById('cartBtn');
    const cartCountEl = document.getElementById('cartCount');
    const cartItemsEl = document.getElementById('cartItems');
    const cartTotalEl = document.getElementById('cartTotal');

    let authResolvida = false;

    if (cartSidebar) cartSidebar.style.display = 'none';

    function escapar(str) {
        if (typeof str !== 'string') return '';
        const el = document.createElement('div');
        el.textContent = str;
        return el.innerHTML;
    }

    function imagemSegura(url) {
        if (!url || typeof url !== 'string') return '';
        if (url.startsWith('javascript:') || url.startsWith('data:')) return '';
        return url;
    }

    function salvarCarrinho(itens) {
        localStorage.setItem('carrinho', JSON.stringify(itens));
    }

    function carregarCarrinho() {
        try {
            const raw = localStorage.getItem('carrinho');
            if (!raw) return [];

            const parsed = JSON.parse(raw);
            if (!Array.isArray(parsed)) return [];

            return parsed.map(item => ({
                id: String(item.id),
                nome: item.nome || '',
                imagem: item.imagem || '',
                preco: Number(item.preco) || 0,
                quantidade: Number(item.quantidade) || 0
            })).filter(i => i.quantidade > 0);
        } catch {
            return [];
        }
    }

    function atualizarContador(itens) {
        const total = itens.reduce((acc, item) => acc + item.quantidade, 0);
        if (cartCountEl) cartCountEl.textContent = String(total);
    }

    function atualizarTotal(itens) {
        const total = itens.reduce((acc, item) => acc + item.preco * item.quantidade, 0);
        if (cartTotalEl) {
            cartTotalEl.textContent = `R$ ${total.toFixed(2).replace('.', ',')}`;
        }
    }

    function renderizarItens(itens) {
        if (!cartItemsEl) return;

        if (itens.length === 0) {
            cartItemsEl.innerHTML = `
                <div class="cart__empty">
                    <p>Seu carrinho está vazio.</p>
                </div>`;
            return;
        }

        cartItemsEl.innerHTML = itens.map(item => `
            <div class="cart__item" data-id="${escapar(String(item.id))}">
                ${imagemSegura(item.imagem)
                ? `<img class="cart__item-img" src="${imagemSegura(item.imagem)}" alt="${escapar(item.nome)}" onerror="this.style.display='none'" />`
                : `<div class="cart__item-img"><span>🍺</span></div>`}

                <div class="cart__item-info">
                    <h3>${escapar(item.nome)}</h3>
                    <p>R$ ${(item.preco * item.quantidade).toFixed(2).replace('.', ',')}</p>
                </div>

                <div class="cart__item-qty">
                    <button type="button" class="cart__qty-menos" data-id="${escapar(String(item.id))}">−</button>
                    <span>${item.quantidade}</span>
                    <button type="button" class="cart__qty-mais" data-id="${escapar(String(item.id))}">+</button>
                </div>
            </div>
        `).join('');

        cartItemsEl.querySelectorAll('.cart__qty-mais').forEach(btn => {
            btn.addEventListener('click', () => alterarQtd(btn.dataset.id, 1));
        });

        cartItemsEl.querySelectorAll('.cart__qty-menos').forEach(btn => {
            btn.addEventListener('click', () => alterarQtd(btn.dataset.id, -1));
        });
    }

    function atualizarUI() {
        const itens = carregarCarrinho();
        renderizarItens(itens);
        atualizarContador(itens);
        atualizarTotal(itens);
    }

    function alterarQtd(id, delta) {
        const itens = carregarCarrinho();
        const index = itens.findIndex(i => String(i.id) === String(id));
        if (index === -1) return;

        itens[index].quantidade += delta;

        if (itens[index].quantidade <= 0) {
            itens.splice(index, 1);
        }

        salvarCarrinho(itens);
        atualizarUI();
    }

    function adicionarAoCarrinho(produto) {
        if (!produto || !produto.id) return;

        const itens = carregarCarrinho();
        const qtd = Number(produto.quantidade) || 1;
        const index = itens.findIndex(i => String(i.id) === String(produto.id));

        if (index !== -1) {
            itens[index].quantidade += qtd;
        } else {
            itens.push({
                id: String(produto.id),
                nome: produto.nome || '',
                imagem: produto.imagem || '',
                preco: Number(produto.preco) || 0,
                quantidade: qtd
            });
        }

        salvarCarrinho(itens);
        atualizarUI();
        abrirCarrinho();
    }

    function abrirCarrinho() {
        if (!cartSidebar || !cartOverlayEl) return;

        cartSidebar.style.display = 'flex';

        requestAnimationFrame(() => {
            cartSidebar.classList.add('open');
            cartOverlayEl.classList.add('open');
            document.body.style.overflow = 'hidden';
        });
    }

    function fecharCarrinho() {
        if (!cartSidebar || !cartOverlayEl) return;

        cartSidebar.classList.remove('open');
        cartOverlayEl.classList.remove('open');
        document.body.style.overflow = '';

        cartSidebar.addEventListener('transitionend', () => {
            if (!cartSidebar.classList.contains('open')) {
                cartSidebar.style.display = 'none';
            }
        }, { once: true });
    }

    function aplicarEstadoAuth() {
        authResolvida = true;

        if (cartOpenBtn) {
            cartOpenBtn.style.display = isLogado ? 'flex' : 'none';
        }

        if (isLogado) {
            atualizarUI();
        } else {
            /* NÃO apagar localStorage aqui */
            renderizarItens([]);
            atualizarContador([]);
            atualizarTotal([]);
        }
    }

    cartOpenBtn?.addEventListener('click', abrirCarrinho);
    cartCloseBtn?.addEventListener('click', fecharCarrinho);
    cartOverlayEl?.addEventListener('click', fecharCarrinho);

    window.addEventListener('authCarregado', aplicarEstadoAuth);

    document.addEventListener('DOMContentLoaded', () => {
        setTimeout(() => {
            if (!authResolvida && typeof isLogado !== 'undefined') {
                aplicarEstadoAuth();
            }
        }, 150);
    });

    window.adicionarAoCarrinho = adicionarAoCarrinho;
    window.carregarCarrinho = carregarCarrinho;
    window.salvarCarrinho = salvarCarrinho;
})();