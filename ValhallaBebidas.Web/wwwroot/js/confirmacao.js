/* ════════════════════════════════════════════════════════
   confirmacao.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

const params = new URLSearchParams(window.location.search);
const pedidoId = params.get('pedido') || Math.floor(Math.random() * 900000 + 100000);
const numeroEl = document.getElementById('numeroPedido');
if (numeroEl) numeroEl.textContent = `Pedido #${pedidoId}`;

const itens = JSON.parse(localStorage.getItem('ultimoPedido') || '[]');
const lista = document.getElementById('confirmacaoItens');
const formatar = v => `R$ ${v.toFixed(2).replace('.', ',')}`;

if (itens.length > 0 && lista) {
    lista.innerHTML = itens.map(item => `
    <li class="confirmacao__item">
      <div class="confirmacao__item-img">
        ${item.imagem ? `<img src="${item.imagem}" alt="${item.nome}" />` : `<span>🍺</span>`}
      </div>
      <div class="confirmacao__item-info">
        <p>${item.nome}</p>
        <span>${item.quantidade}x · ${formatar(item.preco)}</span>
      </div>
      <p class="confirmacao__item-preco">${formatar(item.preco * item.quantidade)}</p>
    </li>
  `).join('');
    const total = itens.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
    const totalEl = document.getElementById('confirmacaoTotal');
    if (totalEl) totalEl.textContent = formatar(total);
}

const previsao = new Date();
previsao.setDate(previsao.getDate() + 5);
const previsaoEl = document.getElementById('confirmacaoPrevisao');
if (previsaoEl) previsaoEl.textContent = previsao.toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' });

/* ── Método de pagamento (lido do localStorage) ── */
const metodoLabels = {
    cartao: 'Cartão de crédito',
    pix: 'Pix',
    boleto: 'Boleto bancário',
};
const metodo = localStorage.getItem('metodoPagamento') || 'cartao';
const pagEl = document.getElementById('confirmacaoPagamento');
if (pagEl) pagEl.textContent = metodoLabels[metodo] || metodo;

const endEl = document.getElementById('confirmacaoEndereco');
if (endEl) endEl.textContent = 'Endereço cadastrado';

/* Limpa dados da sessão */
localStorage.removeItem('metodoPagamento');
localStorage.removeItem('ultimoPedido');

