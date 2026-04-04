/* ════════════════════════════════════════════════════════
   catalogo.js — Valhalla Bebidas (MVC — API real)
   Depende de: auth.js (isLogado) | cart.js (adicionarAoCarrinho)
════════════════════════════════════════════════════════ */

let produtos = [];
let categoriaAtiva = 'todos';
let ordemAtiva = 'relevancia';
let buscaAtiva = '';

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

/* ── Elementos ── */
const grid = document.querySelector('.products');
const searchInput = document.getElementById('search-input');
const countEl = document.querySelector('.product__title p');

/* ── Labels de categoria ── */
const categoriaLabel = {
    refrigerantes: 'Refrigerantes',
    destilados: 'Destilados',
    cervejas: 'Cervejas',
    energeticos: 'Energéticos',
    aguas: 'Águas',
    sucos: 'Sucos',
    vinhos: 'Vinhos',
    gelos: 'Gelos',
};

/* ── Busca dados da API ── */
async function carregarProdutos() {
    mostrarSkeleton();
    try {
        const response = await fetch('/api/produto');
        if (!response.ok) throw new Error('Erro ao carregar produtos');
        produtos = await response.json();
        renderizar();
    } catch (err) {
        console.error(err);
        if (grid) grid.innerHTML = '<li style="color:#e05c5c;font-size:16px;padding:80px 0;text-align:center;width:100%">Erro ao carregar catálogo. Tente novamente.</li>';
    }
}

/* ── Filtros — categoria ── */
document.querySelectorAll('.catalogo_filter-btn button, .catalogo_filter-btn-active button').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.catalogo_filter-btn, .catalogo_filter-btn-active').forEach(li => {
            li.className = 'catalogo_filter-btn';
        });
        btn.closest('li').className = 'catalogo_filter-btn-active';

        const texto = btn.textContent.trim().toLowerCase();
        const mapa = {
            'todos': 'todos', 'refrigerantes': 'refrigerantes', 'destilados': 'destilados',
            'cervejas': 'cervejas', 'energéticos': 'energeticos', 'águas': 'aguas',
            'sucos': 'sucos', 'vinhos': 'vinhos', 'gelos': 'gelos',
            'relevância': 'relevancia', 'menor preço': 'menor-preco',
            'maior preço': 'maior-preco', 'nome a-z': 'nome',
        };

        if (mapa[texto] !== undefined) {
            if (['relevancia', 'menor-preco', 'maior-preco', 'nome'].includes(mapa[texto])) {
                ordemAtiva = mapa[texto];
            } else {
                categoriaAtiva = mapa[texto];
            }
        }
        renderizar();
    });
});

/* ── Busca por texto com debounce ── */
let debounceTimer;
searchInput?.addEventListener('input', (e) => {
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
        buscaAtiva = e.target.value.toLowerCase().trim();
        renderizar();
    }, 300);
});

/* ── Filtrar e ordenar ── */
function filtrarProdutos() {
    let resultado = [...produtos];

    /* Filtra pelo nome da categoria (campo NomeCategoria da API) */
    if (categoriaAtiva !== 'todos') {
        resultado = resultado.filter(p =>
            (p.nomeCategoria || '').toLowerCase() === categoriaAtiva
        );
    }

    if (buscaAtiva) {
        resultado = resultado.filter(p => p.nome.toLowerCase().includes(buscaAtiva));
    }

    switch (ordemAtiva) {
        case 'menor-preco': resultado.sort((a, b) => a.precoVenda - b.precoVenda); break;
        case 'maior-preco': resultado.sort((a, b) => b.precoVenda - a.precoVenda); break;
        case 'nome': resultado.sort((a, b) => a.nome.localeCompare(b.nome)); break;
    }
    return resultado;
}

/* ── Render ── */
function gerarCard(produto) {
    const nomeEscapado = escapar(produto.nome);
    const label = categoriaLabel[produto.nomeCategoria?.toLowerCase()] || produto.nomeCategoria || '';
    const labelEscapado = escapar(label);
    const imgSegura = imagemSegura(produto.fotoProduto);
    const temImg = Boolean(imgSegura);

    /* Dados do produto como JSON seguro em attribute data- */
    const dadosProduto = JSON.stringify({
        id: produto.id,
        nome: produto.nome,
        preco: produto.precoVenda,
        imagem: produto.fotoProduto || '',
    }).replace(/"/g, '&quot;');

    const btnAdicionar = isLogado
        ? `<a href="#" class="market-button"
               onclick="event.preventDefault(); adicionarAoCarrinho(JSON.parse(&quot;${dadosProduto}&quot;))">
               Adicionar ao carrinho
           </a>`
        : `<a href="/Auth/Login" class="market-button" style="opacity:.5">Fazer login</a>`;

    return `
    <li class="product-card page-anim-prep">
      <div class="product-img">
        ${temImg ? `<img src="${imgSegura}" alt="${nomeEscapado}" />` : `<span style="font-size:52px;opacity:.2">🍺</span>`}
      </div>
      <div>
        <h1>${nomeEscapado}</h1>
        ${labelEscapado ? `<p>${labelEscapado}</p>` : ''}
      </div>
      <h2>R$${produto.precoVenda.toFixed(2).replace('.', ',')}</h2>
      <div class="buttons">
        ${btnAdicionar}
        <a href="/Catalogo/Produto?id=${produto.id}" class="details-button">Ver detalhes</a>
      </div>
    </li>
  `;
}

function renderizar() {
    const produtosFiltrados = filtrarProdutos();
    if (countEl) countEl.textContent = `${produtosFiltrados.length} produto${produtosFiltrados.length !== 1 ? 's' : ''} encontrado${produtosFiltrados.length !== 1 ? 's' : ''}`;
    if (!grid) return;
    grid.innerHTML = produtosFiltrados.length === 0
        ? `<li style="color:#404040;font-size:16px;padding:80px 0;text-align:center;width:100%">Nenhum produto encontrado.</li>`
        : produtosFiltrados.map(gerarCard).join('');
}

/* ── Skeleton ── */
function mostrarSkeleton() {
    if (!grid) return;
    grid.innerHTML = Array(6).fill('').map(() => `
    <li class="product-card">
      <div class="product-img" style="background:linear-gradient(90deg,#1b1b1b 25%,#232320 50%,#1b1b1b 75%);background-size:200% 100%;animation:skeleton-pulse 1.5s infinite;border-radius:20px;"></div>
      <div style="height:24px;width:60%;background:#1b1b1b;border-radius:6px;margin-top:8px;"></div>
      <div style="height:16px;width:40%;background:#1b1b1b;border-radius:6px;margin-top:6px;"></div>
    </li>
  `).join('');
}

carregarProdutos();
