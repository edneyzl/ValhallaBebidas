/* ════════════════════════════════════════════════════════
   compras.js — Valhalla Bebidas
   Página protegida pelo AuthFilter no servidor
════════════════════════════════════════════════════════ */

let pedidos = [];
let filtroAtivo = 'todos';

const formatar = (v) => `R$ ${Number(v || 0).toFixed(2).replace('.', ',')}`;

function escapar(str) {
    if (typeof str !== 'string') return '';
    const el = document.createElement('div');
    el.textContent = str;
    return el.innerHTML;
}

function getStatusLabel(status) {
    if (typeof status === 'number') {
        const map = {
            0: 'Pendente',
            1: 'Confirmado',
            2: 'Cancelado'
        };
        return map[status] || 'Desconhecido';
    }

    return status || 'Desconhecido';
}

const statusConfig = {
    Pendente: { classe: 'pedido__status--pendente', label: 'Pendente' },
    Confirmado: { classe: 'pedido__status--confirmado', label: 'Confirmado' },
    Cancelado: { classe: 'pedido__status--cancelado', label: 'Cancelado' }
};

document.addEventListener('DOMContentLoaded', () => {
    configurarFiltros();
    carregarPedidos();
});

function configurarFiltros() {
    document.querySelectorAll('.pedidos__filtro').forEach(btn => {
        btn.addEventListener('click', () => {
            document.querySelectorAll('.pedidos__filtro').forEach(b =>
                b.classList.toggle('pedidos__filtro--active', b === btn)
            );

            filtroAtivo = btn.dataset.status;
            renderizar();
        });
    });
}

async function carregarPedidos() {
    const lista = document.getElementById('pedidosLista');
    const vazio = document.getElementById('pedidosVazio');
    const countEl = document.getElementById('pedidosCount');

    try {
        const response = await fetch('/Pedidos/MeusPedidosApi', {
            credentials: 'include'
        });

        if (response.status === 401) {
            window.location.href = '/Auth/Login';
            return;
        }

        const texto = await response.text();
        console.log('Resposta raw pedidos:', texto);

        if (!response.ok) {
            throw new Error(texto || 'Erro ao carregar pedidos');
        }

        const data = texto ? JSON.parse(texto) : [];

        pedidos = Array.isArray(data)
            ? data.filter(p => p && p.id !== undefined)
            : [];

        console.log('Pedidos carregados:', pedidos);

        renderizar();
    } catch (err) {
        console.error('Erro ao carregar pedidos:', err);

        if (lista) lista.innerHTML = '';
        if (countEl) countEl.textContent = '0 pedidos';

        if (vazio) {
            vazio.style.display = 'flex';
            vazio.innerHTML = '<p style="color:#e05c5c">Erro ao carregar pedidos.</p>';
        }
    }
}

async function cancelarPedido(id) {
    if (!confirm('Deseja cancelar este pedido?')) return;

    try {
        const response = await fetch(`/Pedidos/Cancelar/${id}`, {
            method: 'POST',
            credentials: 'include'
        });

        const texto = await response.text();

        if (!response.ok) {
            console.error('Erro ao cancelar:', texto);
            alert(texto || 'Erro ao cancelar pedido.');
            return;
        }

        const pedido = pedidos.find(p => p.id === id);
        if (pedido) pedido.status = 2;

        renderizar();
    } catch (err) {
        console.error(err);
        alert('Erro de conexão.');
    }
}

function renderizar() {
    const lista = document.getElementById('pedidosLista');
    const vazio = document.getElementById('pedidosVazio');
    const countEl = document.getElementById('pedidosCount');

    if (!lista) {
        console.error('Elemento #pedidosLista não encontrado');
        return;
    }

    const filtrados = filtroAtivo === 'todos'
        ? pedidos
        : pedidos.filter(p => getStatusLabel(p.status) === filtroAtivo);

    if (countEl) {
        countEl.textContent = `${pedidos.length} pedido${pedidos.length !== 1 ? 's' : ''}`;
    }

    if (filtrados.length === 0) {
        lista.innerHTML = '';
        if (vazio) vazio.style.display = 'flex';
        return;
    }

    if (vazio) vazio.style.display = 'none';

    lista.innerHTML = filtrados.map(pedido => {
        const statusTexto = getStatusLabel(pedido.status);
        const statusInfo = statusConfig[statusTexto] || {
            classe: '',
            label: statusTexto
        };

        const podeCancelar = statusTexto === 'Pendente';

        return `
            <div class="pedido__card">
                <div class="pedido__card-topo">
                    <div>
                        <p class="pedido__card-id">Pedido #${pedido.id}</p>
                        <p class="pedido__card-data">${new Date(pedido.dataPedido).toLocaleDateString('pt-BR')}</p>
                    </div>

                    <span class="pedido__status ${statusInfo.classe}">
                        <span class="pedido__status-dot"></span>
                        ${statusInfo.label}
                    </span>
                </div>

                <ul class="pedido__itens">
                    ${(pedido.itens || []).map(item => {
            const nome = escapar(item.nomeProduto || item.nome || 'Produto');
            const qt = Number(item.quantidade) || 0;
            const precoUnit = Number(item.precoUnitario || item.preco || 0);
            const subtotal = Number(item.subtotal) || (precoUnit * qt);

            return `
                            <li class="pedido__item">
                                <div class="pedido__item-img">
                                    <span>🍺</span>
                                </div>
                                <p class="pedido__item-nome">${nome}</p>
                                <span class="pedido__item-qty">${qt}x</span>
                                <p class="pedido__item-preco">${formatar(subtotal)}</p>
                            </li>
                        `;
        }).join('')}
                </ul>

                <div class="pedido__card-rodape">
                    <div>
                        <p class="pedido__total-label">Total do pedido</p>
                        <p class="pedido__total-valor">${formatar(pedido.valorTotal)}</p>
                    </div>

                    <div class="pedido__card-acoes">
                        ${podeCancelar ? `
                            <button type="button"
                                    class="pedido__btn pedido__btn--cancelar"
                                    onclick="cancelarPedido(${pedido.id})">
                                Cancelar
                            </button>
                        ` : ''}
                    </div>
                </div>
            </div>
        `;
    }).join('');
}

window.cancelarPedido = cancelarPedido;