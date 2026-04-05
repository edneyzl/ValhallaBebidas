/* ════════════════════════════════════════════════════════
   produto.js — Valhalla Bebidas (MVC — API real)
   Depende de: auth.js (isLogado) | cart.js (adicionarAoCarrinho)
════════════════════════════════════════════════════════ */

/* ── URL param ── */
const params = new URLSearchParams(window.location.search);
const produtoId = params.get('id');

if (!produtoId) window.location.href = '/Catalogo';

/* ── Elementos ── */
const produtoImg = document.getElementById('produtoImg');
const produtoCategoria = document.getElementById('produtoCategoria');
const produtoNome = document.getElementById('produtoNome');
const produtoEan = document.getElementById('produtoEan');
const produtoDescricao = document.getElementById('produtoDescricao');
const produtoPreco = document.getElementById('produtoPreco');
const produtoEstoque = document.getElementById('produtoEstoque');
const btnAdicionar = document.getElementById('btnAdicionar');
const qtyValor = document.getElementById('qtyValor');
const qtyMenos = document.getElementById('qtyMenos');
const qtyMais = document.getElementById('qtyMais');
const detalheCategoria = document.getElementById('detalheCategoria');
const detalheEan = document.getElementById('detalheEan');
const detalheEstoqueMin = document.getElementById('detalheEstoqueMin');
const detalheData = document.getElementById('detalheData');

/* ── Quantidade ── */
let quantidade = 1;
qtyMenos?.addEventListener('click', () => { if (quantidade > 1) { quantidade--; qtyValor.textContent = quantidade; } });
qtyMais?.addEventListener('click', () => { quantidade++; qtyValor.textContent = quantidade; });

/* ── Carrega produto pela API ── */
async function carregarProdutoProduto() {
    try {
        const response = await fetch(`/api/produto/${produtoId}`);
        if (!response.ok) { window.location.href = '/Catalogo'; return; }
        const produto = await response.json();

        document.title = `${produto.nome} - Valhalla Bebidas`;
        const imagem = produto.fotoProduto || '';
        if (imagem) produtoImg.innerHTML = `<img src="${imagem}" alt="${produto.nome}" />`;
        produtoCategoria.textContent = produto.nomeCategoria;
        produtoNome.textContent = produto.nome;
        produtoEan.textContent = `EAN: ${produto.eanCodBarras}`;
        produtoDescricao.textContent = produto.descricao;
        produtoPreco.textContent = `R$ ${produto.precoVenda.toFixed(2).replace('.', ',')}`;

        const temEstoque = produto.quantidadeEstoque > 0;
        produtoEstoque.innerHTML = `<span class="produto__estoque-dot"></span>${temEstoque ? `Em estoque (${produto.quantidadeEstoque} un.)` : 'Sem estoque'}`;
        if (!temEstoque) produtoEstoque.classList.add('produto__estoque--esgotado');

        detalheCategoria.textContent = produto.nomeCategoria;
        detalheEan.textContent = produto.eanCodBarras;
        detalheEstoqueMin.textContent = `${produto.quantidadeMinimo} unidades`;
        detalheData.textContent = produto.dataCadastro.toLocaleString ? new Date(produto.dataCadastro).toLocaleDateString('pt-BR') : produto.dataCadastro;

        if (!isLogado) {
            btnAdicionar.textContent = 'Faça login para comprar';
            btnAdicionar.disabled = true;
            return;
        }

        if (!temEstoque) {
            btnAdicionar.textContent = 'Sem estoque';
            btnAdicionar.disabled = true;
            return;
        }

        btnAdicionar.addEventListener('click', () => {
            for (let i = 0; i < quantidade; i++) {
                adicionarAoCarrinho({ id: produto.id, nome: produto.nome, preco: produto.precoVenda, imagem: produto.fotoProduto || '' });
            }
        });
    } catch {
        window.location.href = '/Catalogo';
    }
}

carregarProdutoProduto();
