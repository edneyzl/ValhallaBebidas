📌 1. Levantamento de Requisitos (Refinado com EF Core)
🏗️ Arquitetura (baseado no seu projeto)

Você está usando um padrão muito próximo de:

Domain → Entidades + Interfaces (regras de negócio)

Application → DTOs + Services (casos de uso)

Infrastructure → EF Core (DbContext, Repositories)

👉 Isso é praticamente DDD + Clean Architecture

📋 Requisitos Funcionais (ajustados)
🧑 Cliente

CRUD completo de cliente

Relacionamento 1:N com Endereço

Persistência via EF Core (DbSet<Cliente>)

📍 Endereço

Relacionado a Cliente

Pode ter múltiplos por cliente

Navegação:

Cliente.Enderecos

Endereco.ClienteId

🍺 Produto

CRUD de produtos

Relacionamento com Categoria

Controle de estoque (campo sugerido: QuantidadeEstoque)

🏷️ Categoria

CRUD de categorias

Relacionamento:

Categoria -> Produtos

📦 Pedido

Criar pedido com:

Cliente

Lista de itens

Relacionamentos:

Pedido -> Cliente

Pedido -> Itens

Campos comuns:

Data

Status

ValorTotal (calculado)

🧾 ItemPedido

Relacionamento:

ItemPedido -> Pedido

ItemPedido -> Produto

Campos:

Quantidade

PreçoUnitário

Subtotal

🔄 Movimentação (estoque)

Entrada e saída de produtos

Atualiza estoque automaticamente

Tipos:

Entrada

Saída

👨‍💼 Funcionário

Cadastro básico

Possível uso futuro:

Responsável por pedidos

Auditoria

🧠 Regras de Negócio (IMPORTANTE)

Aqui entra o diferencial do seu sistema:

Pedido deve ter pelo menos 1 item

Estoque não pode ficar negativo

Valor total do pedido = soma dos itens

Cliente deve existir para criar pedido

Produto deve existir para ser adicionado ao pedido

⚙️ Requisitos Técnicos (EF Core)

Uso de DbContext

Mapeamentos:

Fluent API ou Data Annotations

Migrations para versionamento do banco

Lazy/Eager Loading:

.Include() para carregar relacionamentos

Padrão Repository (já iniciado nas interfaces)

🧱 2. Wireframe (Atualizado com visão de API + UI)

Como você está usando ASP.NET Core + EF, provavelmente terá:

👉 Backend (API REST)
👉 Frontend (futuro: React, Angular ou Razor)

🌐 Estrutura de API (REST)
Clientes
GET    /api/clientes
GET    /api/clientes/{id}
POST   /api/clientes
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}
Produtos
GET    /api/produtos
POST   /api/produtos
PUT    /api/produtos/{id}
DELETE /api/produtos/{id}
Pedidos
GET    /api/pedidos
GET    /api/pedidos/{id}
POST   /api/pedidos
Movimentações
POST /api/movimentacoes
🖥️ Wireframe (Mais realista com fluxo)
📦 Criar Pedido (Fluxo completo)
[Selecionar Cliente]
        ↓
[Adicionar Produtos]
        ↓
[Definir Quantidade]
        ↓
[Visualizar Total]
        ↓
[Salvar Pedido]
🧾 Tela (mais refinada)
+--------------------------------------------------+
| Novo Pedido                                      |
+--------------------------------------------------+
| Cliente: [Dropdown com busca]                    |
+--------------------------------------------------+

| Produto     | Qtd | Preço | Subtotal | Remover   |
|--------------------------------------------------|
| Heineken    |  5  | 8,00  | 40,00    | [X]       |
|--------------------------------------------------|

[ + Adicionar Produto ]

+--------------------------------------------------+
| Total: R$ 40,00                                  |
+--------------------------------------------------+

[ Cancelar ]                        [ Salvar Pedido ]
