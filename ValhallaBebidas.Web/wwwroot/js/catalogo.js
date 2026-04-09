/* ════════════════════════════════════════════════════════
   catalogo.js — Valhalla Bebidas (MVC — API real)
   Depende de: auth.js (isLogado) | cart.js (adicionarAoCarrinho)

   Ajuste:
   - filtros por data-categoria-id
   - ordenação por data-ordem
   - restante mantido como estava
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

function normalizar(texto) {
    return (texto || '')
        .toString()
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .trim();
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
        if (grid) {
            grid.innerHTML = `
                <li style="color:#e05c5c;font-size:16px;padding:80px 0;text-align:center;width:100%">
                    Erro ao carregar catálogo. Tente novamente.
                </li>
            `;
        }
    }
}

/* ── Filtros — categoria via data-categoria-id ── */
document.querySelectorAll('.categorias button[data-categoria-id]').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.categorias li').forEach(li => {
            li.className = 'catalogo_filter-btn';
        });

        btn.closest('li').className = 'catalogo_filter-btn-active';

        categoriaAtiva = btn.dataset.categoriaId || 'todos';
        renderizar();
    });
});

/* ── Filtros — ordenação via data-ordem ── */
document.querySelectorAll('.ordenacao button[data-ordem]').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.ordenacao li').forEach(li => {
            li.className = 'catalogo_filter-btn';
        });

        btn.closest('li').className = 'catalogo_filter-btn-active';

        ordemAtiva = btn.dataset.ordem || 'relevancia';
        renderizar();
    });
});

/* ── Busca por texto com debounce ── */
let debounceTimer;
searchInput?.addEventListener('input', (e) => {
    clearTimeout(debounceTimer);

    debounceTimer = setTimeout(() => {
        buscaAtiva = normalizar(e.target.value);
        renderizar();
    }, 300);
});

/* ── Filtrar e ordenar ── */
function filtrarProdutos() {
    let resultado = [...produtos];

    /* Filtra por CategoriaId da API */
    if (categoriaAtiva !== 'todos') {
        const categoriaId = Number(categoriaAtiva);

        resultado = resultado.filter(p => Number(p.categoriaId) === categoriaId);
    }

    /* Busca por nome */
    if (buscaAtiva) {
        resultado = resultado.filter(p =>
            normalizar(p.nome).includes(buscaAtiva)
        );
    }

    /* Ordenação */
    switch (ordemAtiva) {
        case 'menor':
            resultado.sort((a, b) => a.precoVenda - b.precoVenda);
            break;

        case 'maior':
            resultado.sort((a, b) => b.precoVenda - a.precoVenda);
            break;

        case 'nome':
            resultado.sort((a, b) => a.nome.localeCompare(b.nome, 'pt-BR'));
            break;

        case 'relevancia':
        default:
            break;
    }

    return resultado;
}

/* ── Render ── */
function gerarCard(produto) {
    const nomeEscapado = escapar(produto.nome);

    const chaveCategoria = normalizar(produto.nomeCategoria);
    const label = categoriaLabel[chaveCategoria] || produto.nomeCategoria || '';
    const labelEscapado = escapar(label);

    const imgSegura = imagemSegura(produto.fotoProduto);
    const temImg = Boolean(imgSegura) && normalizar(imgSegura) !== 'string';

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
                ${temImg
            ? `<img src="${imgSegura}" alt="${nomeEscapado}" onerror="this.onerror=null;this.src='/img/ExemploBebida.png'" />`
            : `<span style="font-size:52px;opacity:.2">🍺</span>`}
            </div>

            <div>
                <h1>${nomeEscapado}</h1>
                ${labelEscapado ? `<p>${labelEscapado}</p>` : ''}
            </div>

            <h2>R$${Number(produto.precoVenda).toFixed(2).replace('.', ',')}</h2>

            <div class="buttons">
                ${btnAdicionar}
                <a href="/Catalogo/Produto?id=${produto.id}" class="details-button">Ver detalhes</a>
            </div>
        </li>
    `;
}

function renderizar() {
    const produtosFiltrados = filtrarProdutos();

    if (countEl) {
        countEl.textContent =
            `${produtosFiltrados.length} produto${produtosFiltrados.length !== 1 ? 's' : ''} encontrado${produtosFiltrados.length !== 1 ? 's' : ''}`;
    }

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