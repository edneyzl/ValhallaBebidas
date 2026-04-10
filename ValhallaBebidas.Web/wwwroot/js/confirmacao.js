/* ════════════════════════════════════════════════════════
   confirmacao.js — Valhalla Bebidas
════════════════════════════════════════════════════════ */

window.addEventListener('authCarregado', ({ detail }) => {
    if (!detail.isLogado) {
        window.location.href = '/Auth/Login';
        return;
    }

    iniciarConfirmacao();
});

function formatar(v) {
    return `R$ ${Number(v || 0).toFixed(2).replace('.', ',')}`;
}

function escapar(str) {
    const el = document.createElement('div');
    el.textContent = str ?? '';
    return el.innerHTML;
}

async function iniciarConfirmacao() {
    const params = new URLSearchParams(window.location.search);
    const pedidoId = params.get('pedido');

    if (!pedidoId) {
        window.location.href = '/';
        return;
    }

    const numeroEl = document.getElementById('numeroPedido');
    if (numeroEl) numeroEl.textContent = `PEDIDO #${pedidoId}`;

    try {
        const response = await fetch(`/Carrinho/ObterPedidoApi?id=${pedidoId}`, {
            credentials: 'include'
        });

        if (response.status === 401) {
            window.location.href = '/Auth/Login';
            return;
        }

        if (!response.ok) {
            const texto = await response.text();
            console.error('Erro ao carregar pedido:', texto);
            throw new Error('Erro ao carregar pedido');
        }

        const pedido = await response.json();
        console.log('Pedido carregado:', pedido);

        renderizarItens(pedido);
        renderizarInfos(pedido);

    } catch (err) {
        console.error(err);
    }
}

function renderizarItens(pedido) {
    const lista = document.getElementById('confirmacaoItens');
    const totalEl = document.getElementById('confirmacaoTotal');

    if (!lista) {
        console.error('Elemento #confirmacaoItens não encontrado');
        return;
    }

    const itens = pedido.itens || [];

    lista.innerHTML = itens.map(item => {
        const nome = escapar(item.nomeProduto || item.nome || item.produto?.nome || 'Produto');
        const qt = Number(item.quantidade) || 0;
        const preco = Number(item.precoUnitario || item.preco || 0);
        const subtotal = Number(item.subtotal) || (preco * qt);
        const img = item.fotoProduto || item.imagem || item.produto?.fotoProduto || '';

        return `
            <li class="confirmacao__item">
                <div class="confirmacao__item-img">
                    ${img ? `<img src="${img}" alt="${nome}" />` : `<span>🍺</span>`}
                </div>
                <div class="confirmacao__item-info">
                    <p>${nome}</p>
                    <span>${qt}x · ${formatar(preco)}</span>
                </div>
                <p class="confirmacao__item-preco">${formatar(subtotal)}</p>
            </li>
        `;
    }).join('');

    if (totalEl) {
        totalEl.textContent = formatar(pedido.valorTotal);
    }
}

function renderizarInfos(pedido) {
    const statusEl = document.getElementById('confirmacaoStatus');
    const previsaoEl = document.getElementById('confirmacaoPrevisao');
    const enderecoEl = document.getElementById('confirmacaoEndereco');
    const pagamentoEl = document.getElementById('confirmacaoPagamento');

    if (statusEl) {
        statusEl.textContent = typeof pedido.status === 'string' ? pedido.status : 'Pendente';
    }

    if (previsaoEl) {
        const previsao = new Date();
        previsao.setDate(previsao.getDate() + 5);
        previsaoEl.textContent = previsao.toLocaleDateString('pt-BR', {
            day: 'numeric',
            month: 'long',
            year: 'numeric'
        });
    }

    const endereco = pedido.enderecoEntrega || pedido.endereco || null;

    if (enderecoEl) {
        enderecoEl.textContent = endereco
            ? `${endereco.logradouro}, ${endereco.numero} - ${endereco.bairro}, ${endereco.cidade}/${endereco.estado}`
            : 'Endereço cadastrado';
    }

    function formatarPagamento(valor) {
        if (!valor) return 'Cartão';

        valor = valor.toLowerCase();

        if (valor === 'pix') return 'Pix';
        if (valor === 'cartao' || valor === 'cartão') return 'Cartão de crédito';
        if (valor === 'boleto') return 'Boleto';

        return valor.charAt(0).toUpperCase() + valor.slice(1);
    }

    if (pagamentoEl) {
        const metodo =
            pedido.formaPagamento ||
            localStorage.getItem('metodoPagamento');

        pagamentoEl.textContent = formatarPagamento(metodo);
    }
}