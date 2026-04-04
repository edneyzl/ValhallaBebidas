/* ════════════════════════════════════════════════════════
   carrinho.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

const lista = document.getElementById('carrinhoLista');
const vazio = document.getElementById('carrinhoVazio');
const count = document.getElementById('carrinhoCount');
const resumoSubtotal = document.getElementById('resumoSubtotal');
const resumoTotal = document.getElementById('resumoTotal');
const btnCheckout = document.getElementById('btnCheckout');

function carregarCarrinho() { return JSON.parse(localStorage.getItem('carrinho') || '[]'); }
function salvarCarrinho(itens) { localStorage.setItem('carrinho', JSON.stringify(itens)); }

function alterarQtd(id, delta) {
    let itens = carregarCarrinho();
    const index = itens.findIndex(i => i.id === id);
    if (index === -1) return;
    itens[index].quantidade += delta;
    if (itens[index].quantidade <= 0) itens.splice(index, 1);
    salvarCarrinho(itens);
    renderizar();
}

function removerItem(id) {
    salvarCarrinho(carregarCarrinho().filter(i => i.id !== id));
    renderizar();
}

function atualizarTotais(itens) {
    const subtotal = itens.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
    const formatar = v => `R$ ${v.toFixed(2).replace('.', ',')}`;
    if (resumoSubtotal) resumoSubtotal.textContent = formatar(subtotal);
    if (resumoTotal) resumoTotal.textContent = formatar(subtotal);
    if (count) {
        const total = itens.reduce((acc, i) => acc + i.quantidade, 0);
        count.textContent = `${total} ${total === 1 ? 'item' : 'itens'}`;
    }
}

function renderizar() {
    const itens = carregarCarrinho();
    if (itens.length === 0) {
        if (lista) lista.innerHTML = '';
        if (vazio) vazio.style.display = 'flex';
        if (btnCheckout) btnCheckout.style.pointerEvents = 'none';
        atualizarTotais([]);
        return;
    }
    if (vazio) vazio.style.display = 'none';
    if (btnCheckout) btnCheckout.style.pointerEvents = 'auto';

    if (lista) {
        lista.innerHTML = itens.map(item => {
            const subtotal = (item.preco * item.quantidade).toFixed(2).replace('.', ',');
            const preco = item.preco.toFixed(2).replace('.', ',');
            return `
        <li class="carrinho__item" data-id="${item.id}">
          <div class="carrinho__item-produto">
            <div class="carrinho__item-img">
              ${item.imagem ? `<img src="${item.imagem}" alt="${item.nome}" />` : `<span>🍺</span>`}
            </div>
            <div class="carrinho__item-info">
              <p class="carrinho__item-nome">${item.nome}</p>
              <button type="button" class="carrinho__item-remover" onclick="removerItem('${item.id}')">Remover</button>
            </div>
          </div>
          <p class="carrinho__item-preco">R$ ${preco}</p>
          <div class="carrinho__item-qty">
            <button type="button" class="carrinho__qty-btn" onclick="alterarQtd('${item.id}', -1)">−</button>
            <span class="carrinho__qty-num">${item.quantidade}</span>
            <button type="button" class="carrinho__qty-btn" onclick="alterarQtd('${item.id}', 1)">+</button>
          </div>
          <p class="carrinho__item-subtotal">R$ ${subtotal}</p>
        </li>
      `;
        }).join('');
    }
    atualizarTotais(itens);
}

/* ── Botão checkout — atualiza href com rota MVC ── */
if (btnCheckout) btnCheckout.href = '/Carrinho/Checkout';

renderizar();