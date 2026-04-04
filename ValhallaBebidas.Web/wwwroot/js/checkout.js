/* ════════════════════════════════════════════════════════
   checkout.js — Valhalla Bebidas (MVC)
   Depende de: auth.js (isLogado)
════════════════════════════════════════════════════════ */

if (!isLogado) window.location.href = '/Auth/Login';

const itens = JSON.parse(localStorage.getItem('carrinho') || '[]');
if (itens.length === 0) window.location.href = '/Catalogo';

/* ── Resumo ── */
function renderizarResumo() {
    const lista = document.getElementById('resumoLista');
    const subtotal = itens.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
    const formatar = v => `R$ ${v.toFixed(2).replace('.', ',')}`;
    if (lista) lista.innerHTML = itens.map(item => `
    <li class="checkout__resumo-item">
      <div class="checkout__resumo-item-img">
        ${item.imagem ? `<img src="${item.imagem}" alt="${item.nome}" />` : `<span>🍺</span>`}
      </div>
      <div class="checkout__resumo-item-info">
        <p>${item.nome}</p>
        <span>${item.quantidade}x · ${formatar(item.preco)} cada</span>
      </div>
      <p class="checkout__resumo-item-preco">${formatar(item.preco * item.quantidade)}</p>
    </li>
  `).join('');
    const subtotalEl = document.getElementById('resumoSubtotal');
    const totalEl = document.getElementById('resumoTotal');
    if (subtotalEl) subtotalEl.textContent = formatar(subtotal);
    if (totalEl) totalEl.textContent = formatar(subtotal);
}
renderizarResumo();

/* ── Métodos de pagamento ── */
const paineis = {
    cartao: document.getElementById('painelCartao'),
    pix: document.getElementById('painelPix'),
    boleto: document.getElementById('painelBoleto'),
};

document.querySelectorAll('.checkout__metodo').forEach(btn => {
    btn.addEventListener('click', () => {
        const metodo = btn.dataset.metodo;
        document.querySelectorAll('.checkout__metodo').forEach(b =>
            b.classList.toggle('checkout__metodo--active', b === btn)
        );
        Object.entries(paineis).forEach(([key, painel]) => {
            if (painel) painel.style.display = key === metodo ? 'flex' : 'none';
        });
    });
});

/* ── Máscaras ── */
document.getElementById('numeroCartao')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '').slice(0, 16);
    e.target.value = v.replace(/(.{4})/g, '$1 ').trim();
});
document.getElementById('validade')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 2) v = v.slice(0, 2) + '/' + v.slice(2);
    e.target.value = v.slice(0, 5);
});
document.getElementById('cvv')?.addEventListener('input', (e) => {
    e.target.value = e.target.value.replace(/\D/g, '').slice(0, 4);
});
document.getElementById('cep')?.addEventListener('input', (e) => {
    let v = e.target.value.replace(/\D/g, '');
    if (v.length > 5) v = v.slice(0, 5) + '-' + v.slice(5);
    e.target.value = v.slice(0, 9);
});
document.getElementById('estado')?.addEventListener('input', (e) => {
    e.target.value = e.target.value.toUpperCase().replace(/[^A-Z]/g, '');
});

/* ── CEP ── */
document.getElementById('cep')?.addEventListener('blur', async (e) => {
    const cep = e.target.value.replace(/\D/g, '');
    if (cep.length !== 8) return;
    try {
        const res = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
        const data = await res.json();
        if (data.erro) return;
        document.getElementById('logradouro').value = data.logradouro || '';
        document.getElementById('bairro').value = data.bairro || '';
        document.getElementById('cidade').value = data.localidade || '';
        document.getElementById('estado').value = data.uf || '';
        document.getElementById('numero')?.focus();
    } catch { /* silencia */ }
});

/* ── Submit ── */
document.getElementById('btnConfirmar')?.addEventListener('click', async () => {
    const payload = {
        itens: itens.map(i => ({ produtoId: i.id, quantidade: i.quantidade, nome: i.nome, preco: i.preco, imagem: i.imagem })),
        entrega: {
            logradouro: document.getElementById('logradouro')?.value.trim(),
            numero: document.getElementById('numero')?.value.trim(),
            complemento: document.getElementById('complemento')?.value.trim(),
            bairro: document.getElementById('bairro')?.value.trim(),
            cidade: document.getElementById('cidade')?.value.trim(),
            estado: document.getElementById('estado')?.value.trim(),
            cep: document.getElementById('cep')?.value.replace(/\D/g, ''),
        },
    };

    /* ── Simulação ── */
    console.log('Payload checkout:', payload);
    localStorage.setItem('ultimoPedido', localStorage.getItem('carrinho'));
    localStorage.removeItem('carrinho');
    window.location.href = '/Carrinho/Confirmacao';

    /* ── API MVC ── */
    /*
    try {
      const response = await fetch('/Carrinho/CheckoutApi', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      const data = await response.json();
      if (!response.ok) { alert(data.mensagem || 'Erro.'); return; }
      localStorage.setItem('ultimoPedido', localStorage.getItem('carrinho'));
      localStorage.removeItem('carrinho');
      window.location.href = `/Carrinho/Confirmacao?pedido=${data.pedidoId}`;
    } catch { alert('Erro de conexão.'); }
    */
});