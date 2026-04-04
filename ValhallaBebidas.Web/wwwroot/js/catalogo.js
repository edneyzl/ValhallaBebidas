/* ════════════════════════════════════════════════════════
   catalogo.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado) | cart.js (adicionarAoCarrinho)
════════════════════════════════════════════════════════ */

const produtosMock = [
    { id: '1', nome: 'Brahma Duplo Malte 350ml', preco: 4.90, categoria: 'cervejas', imagem: '' },
    { id: '2', nome: 'Heineken Long Neck 330ml', preco: 8.50, categoria: 'cervejas', imagem: '' },
    { id: '3', nome: 'Corona Extra 355ml', preco: 9.90, categoria: 'cervejas', imagem: '' },
    { id: '4', nome: 'Red Bull Energy 250ml', preco: 12.90, categoria: 'energeticos', imagem: '' },
    { id: '5', nome: 'Monster Energy 473ml', preco: 10.90, categoria: 'energeticos', imagem: '' },
    { id: '6', nome: 'Coca-Cola 2L', preco: 8.90, categoria: 'refrigerantes', imagem: '' },
    { id: '7', nome: 'Pepsi Black 2L', preco: 7.50, categoria: 'refrigerantes', imagem: '' },
    { id: '8', nome: 'Del Valle Uva 1L', preco: 6.90, categoria: 'sucos', imagem: '' },
    { id: '9', nome: 'Minute Maid Laranja 1L', preco: 7.20, categoria: 'sucos', imagem: '' },
    { id: '10', nome: 'Crystal sem Gás 1,5L', preco: 3.50, categoria: 'aguas', imagem: '' },
    { id: '11', nome: 'Schin Pilsen 473ml', preco: 4.20, categoria: 'cervejas', imagem: '' },
    { id: '12', nome: 'Guaraná Antarctica 2L', preco: 7.90, categoria: 'refrigerantes', imagem: '' },
    { id: '13', nome: 'Johnnie Walker Red Label 1L', preco: 89.90, categoria: 'destilados', imagem: '' },
    { id: '14', nome: 'Casillero del Diablo Tinto', preco: 54.90, categoria: 'vinhos', imagem: '' },
    { id: '15', nome: 'Gelo Britânico 5kg', preco: 18.90, categoria: 'gelos', imagem: '' },
];

let categoriaAtiva = 'todos';
let ordemAtiva = 'relevancia';
let buscaAtiva = '';

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

/* ── Busca ── */
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
    let resultado = [...produtosMock];
    if (categoriaAtiva !== 'todos') resultado = resultado.filter(p => p.categoria === categoriaAtiva);
    if (buscaAtiva) resultado = resultado.filter(p => p.nome.toLowerCase().includes(buscaAtiva));
    switch (ordemAtiva) {
        case 'menor-preco': resultado.sort((a, b) => a.preco - b.preco); break;
        case 'maior-preco': resultado.sort((a, b) => b.preco - a.preco); break;
        case 'nome': resultado.sort((a, b) => a.nome.localeCompare(b.nome)); break;
    }
    return resultado;
}

/* ── Render ── */
function gerarCard(produto) {
    const label = categoriaLabel[produto.categoria] || produto.categoria;
    const temImg = Boolean(produto.imagem);

    const btnAdicionar = isLogado
        ? `<a href="#" class="market-button"
           onclick="event.preventDefault(); adicionarAoCarrinho({ id:'${produto.id}', nome:'${produto.nome}', preco:${produto.preco}, imagem:'${produto.imagem}' })">
           Adicionar ao carrinho
       </a>`
        : `<a href="/Auth/Login" class="market-button" style="opacity:.5">Fazer login</a>`;

    return `
    <li class="product-card page-anim-prep">
      <div class="product-img">
        ${temImg ? `<img src="${produto.imagem}" alt="${produto.nome}" />` : `<span style="font-size:52px;opacity:.2">🍺</span>`}
      </div>
      <div>
        <h1>${produto.nome}</h1>
        <p>${label}</p>
      </div>
      <h2>R$${produto.preco.toFixed(2).replace('.', ',')}</h2>
      <div class="buttons">
        ${btnAdicionar}
        <a href="/Catalogo/Produto?id=${produto.id}" class="details-button">Ver detalhes</a>
      </div>
    </li>
  `;
}

function renderizar() {
    const produtos = filtrarProdutos();
    if (countEl) countEl.textContent = `${produtos.length} produto${produtos.length !== 1 ? 's' : ''} encontrado${produtos.length !== 1 ? 's' : ''}`;
    if (!grid) return;
    grid.innerHTML = produtos.length === 0
        ? `<li style="color:#404040;font-size:16px;padding:80px 0;text-align:center;width:100%">Nenhum produto encontrado.</li>`
        : produtos.map(gerarCard).join('');
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

mostrarSkeleton();
setTimeout(renderizar, 800);