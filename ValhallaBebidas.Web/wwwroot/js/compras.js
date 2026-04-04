/* ════════════════════════════════════════════════════════
   meus-pedidos.js — Valhalla Bebidas (MVC — API real)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

let pedidos = [];
let filtroAtivo = 'todos';
const formatar = v => `R$ ${v.toFixed(2).replace('.', ',')}`;

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

const statusConfig = {
    Pendente: { classe: 'pedido__status--pendente', label: 'Pendente' },
    Confirmado: { classe: 'pedido__status--confirmado', label: 'Confirmado' },
    Cancelado: { classe: 'pedido__status--cancelado', label: 'Cancelado' },
};

/* ── Busca pedidos do Web Proxy ── */
async function carregarPedidos() {
    try {
        const response = await fetch('/Pedidos/MeusPedidosApi', { credentials: 'include' });
        if (response.status === 401) { window.location.href = '/Auth/Login'; return; }
        if (!response.ok) throw new Error('Erro ao carregar pedidos');
        const pedidosArray = await response.json();
        pedidos = pedidosArray.filter(p => p && p.id !== undefined);
        renderizar();
    } catch (err) {
        console.error(err);
        const vazio = document.getElementById('pedidosVazio');
        if (vazio) {
            vazio.style.display = 'flex';
            vazio.innerHTML = '<p style="color:#e05c5c">Erro ao carregar pedidos. Tente novamente.</p>';
        }
    }
}

/* ── Filtros ── */
document.querySelectorAll('.pedidos__filtro').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.pedidos__filtro').forEach(b =>
            b.classList.toggle('pedidos__filtro--active', b === btn)
        );
        filtroAtivo = btn.dataset.status;
        renderizar();
    });
});

/* ── Cancelar pedido via endpoint protegido ── */
async function cancelarPedido(id) {
    if (!confirm('Deseja realmente cancelar este pedido?')) return;
    try {
        const response = await fetch(`/Pedidos/Cancelar/${id}`, {
            method: 'PUT',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
        });
        if (!response.ok) {
            alert('Erro ao cancelar pedido. Tente novamente.');
            return;
        }
        const pedido = pedidos.find(p => p.id === id);
        if (pedido) pedido.status = 'Cancelado';
        renderizar();
    } catch {
        alert('Erro de conexão. Tente novamente.');
    }
}

/* ── Monta URL do pedido com id do cliente ── */
async function renderizar() {
    const lista = document.getElementById('pedidosLista');
    const vazio = document.getElementById('pedidosVazio');
    const countEl = document.getElementById('pedidosCount');

    const filtrados = filtroAtivo === 'todos'
        ? pedidos
        : pedidos.filter(p => p.status === filtroAtivo);

    if (countEl) countEl.textContent = `${pedidos.length} pedido${pedidos.length !== 1 ? 's' : ''}`;

    if (filtrados.length === 0) {
        if (lista) lista.innerHTML = '';
        if (vazio) vazio.style.display = 'flex';
        return;
    }

    if (vazio) vazio.style.display = 'none';
    if (!lista) return;

    lista.innerHTML = filtrados.map(pedido => {
        const pedidoStatus = typeof pedido.status === 'string' ? pedido.status : getStatusLabel(pedido.status);
        const { classe, label } = statusConfig[pedidoStatus] || { classe: '', label: pedidoStatus };
        const podeCancelar = pedidoStatus === 'Pendente';

        return `
      <div class="pedido__card">
        <div class="pedido__card-topo">
          <div>
            <p class="pedido__card-id">#${pedido.id}</p>
            <p class="pedido__card-data">${new Date(pedido.dataPedido).toLocaleDateString('pt-BR')}</p>
          </div>
          <span class="pedido__status ${classe}">
            <span class="pedido__status-dot"></span>${label}
          </span>
        </div>
        <ul class="pedido__itens">
          ${(pedido.itens || []).map(item => {
            const img = imagemSegura(item.fotoProduto || item.imagem);
            const nome = escapar(item.nomeProduto || item.nome || 'Produto');
            const qt = Number(item.quantidade) || 0;
            const preco = Number(item.subtotal) || (Number(item.precoUnitario) * item.quantidade);
            return `
            <li class="pedido__item">
              <div class="pedido__item-img">
                ${img ? `<img src="${img}" alt="${nome}" />` : `<span>🍺</span>`}
              </div>
              <p class="pedido__item-nome">${nome}</p>
              <span class="pedido__item-qty">${qt}x</span>
              <p class="pedido__item-preco">${formatar(preco)}</p>
            </li>
          `;
          }).join('')}
          `).join('')}
        </ul>
        <div class="pedido__card-rodape">
          <div>
            <p class="pedido__total-label">Total do pedido</p>
            <p class="pedido__total-valor">${formatar(pedido.valorTotal)}</p>
          </div>
          <div class="pedido__card-acoes">
            <a href="/Pedidos/${pedido.id}" class="pedido__btn pedido__btn--outline">Ver detalhes</a>
            ${podeCancelar ? `<button type="button" class="pedido__btn pedido__btn--cancelar" onclick="cancelarPedido(${pedido.id})">Cancelar</button>` : ''}
          </div>
        </div>
      </div>
    `;
    }).join('');
}

function getStatusLabel(status) {
    if (typeof status === 'number') {
        const map = { 0: 'Pendente', 1: 'Confirmado', 2: 'Cancelado' };
        return map[status] || 'Desconhecido';
    }
    return status;
}

carregarPedidos();
