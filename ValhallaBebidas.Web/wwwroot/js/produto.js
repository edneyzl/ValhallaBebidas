/* ════════════════════════════════════════════════════════
   produto.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado) | cart.js (adicionarAoCarrinho)
════════════════════════════════════════════════════════ */

const produtosMock = [
    { id: '1', nome: 'Brahma Duplo Malte 350ml', preco: 4.90, categoria: 'Cervejas', ean: '7891149107001', descricao: 'Cerveja premium com duplo malte.', estoque: 150, estoqueMin: 20, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '2', nome: 'Heineken Long Neck 330ml', preco: 8.50, categoria: 'Cervejas', ean: '7891149107067', descricao: 'Cerveja holandesa levemente amarga.', estoque: 80, estoqueMin: 15, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '3', nome: 'Corona Extra 355ml', preco: 9.90, categoria: 'Cervejas', ean: '7891149107002', descricao: 'Cerveja mexicana leve e refrescante.', estoque: 60, estoqueMin: 10, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '4', nome: 'Red Bull Energy 250ml', preco: 12.90, categoria: 'Energéticos', ean: '7891149107003', descricao: 'Energético premium com taurina e cafeína.', estoque: 200, estoqueMin: 30, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '5', nome: 'Monster Energy 473ml', preco: 10.90, categoria: 'Energéticos', ean: '7891149107004', descricao: 'Energético sabor marcante e longa duração.', estoque: 120, estoqueMin: 20, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '6', nome: 'Coca-Cola 2L', preco: 8.90, categoria: 'Refrigerantes', ean: '7891149107005', descricao: 'O refrigerante mais famoso do mundo.', estoque: 300, estoqueMin: 50, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '7', nome: 'Pepsi Black 2L', preco: 7.50, categoria: 'Refrigerantes', ean: '7891149107006', descricao: 'Versão sem açúcar da Pepsi.', estoque: 180, estoqueMin: 30, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '8', nome: 'Del Valle Uva 1L', preco: 6.90, categoria: 'Sucos', ean: '7891149107008', descricao: 'Suco de uva com polpa natural.', estoque: 90, estoqueMin: 15, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '9', nome: 'Minute Maid Laranja 1L', preco: 7.20, categoria: 'Sucos', ean: '7891149107009', descricao: 'Suco de laranja fonte de vitamina C.', estoque: 75, estoqueMin: 15, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '10', nome: 'Crystal sem Gás 1,5L', preco: 3.50, categoria: 'Águas', ean: '7891149107010', descricao: 'Água mineral natural sem gás.', estoque: 500, estoqueMin: 80, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '11', nome: 'Schin Pilsen 473ml', preco: 4.20, categoria: 'Cervejas', ean: '7891149107011', descricao: 'Cerveja Pilsen suave e refrescante.', estoque: 220, estoqueMin: 40, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '12', nome: 'Guaraná Antarctica 2L', preco: 7.90, categoria: 'Refrigerantes', ean: '7891149107012', descricao: 'O refrigerante mais brasileiro do Brasil.', estoque: 260, estoqueMin: 40, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '13', nome: 'Johnnie Walker Red Label 1L', preco: 89.90, categoria: 'Destilados', ean: '7891149107013', descricao: 'Whisky escocês blend suave.', estoque: 40, estoqueMin: 5, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '14', nome: 'Casillero del Diablo Tinto', preco: 54.90, categoria: 'Vinhos', ean: '7891149107014', descricao: 'Vinho tinto chileno encorpado.', estoque: 60, estoqueMin: 8, imagem: '', dataCadastro: 'Jan 2026' },
    { id: '15', nome: 'Gelo Britânico 5kg', preco: 18.90, categoria: 'Gelos', ean: '7891149107015', descricao: 'Gelo cristalino de alta pureza.', estoque: 100, estoqueMin: 20, imagem: '', dataCadastro: 'Jan 2026' },
];

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

/* ── Carregar ── */
function carregarProduto(produto) {
    document.title = `${produto.nome} - Valhalla Bebidas`;
    if (produto.imagem) produtoImg.innerHTML = `<img src="${produto.imagem}" alt="${produto.nome}" />`;
    produtoCategoria.textContent = produto.categoria;
    produtoNome.textContent = produto.nome;
    produtoEan.textContent = `EAN: ${produto.ean}`;
    produtoDescricao.textContent = produto.descricao;
    produtoPreco.textContent = `R$ ${produto.preco.toFixed(2).replace('.', ',')}`;

    const temEstoque = produto.estoque > 0;
    produtoEstoque.innerHTML = `<span class="produto__estoque-dot"></span>${temEstoque ? `Em estoque (${produto.estoque} un.)` : 'Sem estoque'}`;
    if (!temEstoque) produtoEstoque.classList.add('produto__estoque--esgotado');

    detalheCategoria.textContent = produto.categoria;
    detalheEan.textContent = produto.ean;
    detalheEstoqueMin.textContent = `${produto.estoqueMin} unidades`;
    detalheData.textContent = produto.dataCadastro;

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
            adicionarAoCarrinho({ id: produto.id, nome: produto.nome, preco: produto.preco, imagem: produto.imagem });
        }
    });
}

const produto = produtosMock.find(p => p.id === produtoId);
if (!produto) window.location.href = '/Catalogo';
else carregarProduto(produto);