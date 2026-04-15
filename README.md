# 🍺 Valhalla Bebidas

> Distribuidora premium de bebidas. Plataforma B2B para parceiros comerciais realizarem pedidos de produtos das maiores marcas do mercado.

---

## 📋 Sobre o Projeto

A **Valhalla Bebidas** é uma aplicação completa para distribuidoras de bebidas, permitindo que parceiros comerciais acessem o catálogo exclusivo, adicionem produtos ao carrinho e realizem pedidos online com preços especiais.

O projeto é **full-stack**, dividido em quatro camadas:

- **Frontend Web** — ASP.NET Core MVC com Razor Views + JavaScript
- **Backend API** — .NET 10 REST API com Clean Architecture
- **Desktop Interno** — Windows Forms com Guna UI2 (gestão administrativa)
- **Banco de Dados** — SQL Server via Entity Framework Core

---

## 🚀 Tecnologias

### Frontend Web (ASP.NET Core MVC)

| Tecnologia | Uso |
|---|---|
| ASP.NET Core MVC | Framework web |
| Razor Views | Templates server-side |
| JavaScript ES6+ | Interações e requisições à API |
| GSAP 3.12 | Animações de scroll e entrada |
| ScrollTrigger | Trigger de animações por scroll |
| Lenis | Scroll suave |
| Sora (Google Fonts) | Tipografia |

### Desktop Interno (Windows Forms + Guna UI2)

| Tecnologia | Uso |
|---|---|
| Windows Forms (.NET) | Framework desktop |
| Guna UI2 | Componentes visuais modernos |
| HttpClient | Comunicação com a API REST |

#### Telas implementadas

| Arquivo | Descrição |
|---|---|
| `FrmLogin` | Autenticação do funcionário |
| `frmPrincipal` | Shell principal com navegação lateral |
| `frmCadastroFuncionario` | Cadastro de novos funcionários |
| `ucDashboard` | Painel com indicadores e métricas |
| `ucClientes` | Listagem e gestão de clientes |
| `ucFuncionarios` | Listagem e gestão de funcionários |
| `ucProdutos` | Listagem e gestão de produtos |
| `ucPedidos` | Acompanhamento e gestão de pedidos |
| `ucMovimentacoes` | Histórico de movimentações de estoque |
| `ucNovoCliente` | Formulário de cadastro de cliente |
| `ucNovoFuncionario` | Formulário de cadastro de funcionário |
| `ucNovoPedido` | Formulário de novo pedido |
| `ucNovoProduto` | Formulário de cadastro de produto |
| `ucNovaMovimentacao` | Formulário de nova movimentação |

### Backend (API — Clean Architecture)

| Tecnologia | Uso |
|---|---|
| .NET 10 | API REST |
| Entity Framework Core 10 | ORM + migrations |
| SQL Server | Banco de dados relacional |
| BCrypt.Net | Hash de senhas |
| Swagger / OpenAPI | Documentação interativa da API |

### Arquitetura

```
API (Controllers)
    ↓
Application (Services + DTOs)
    ↓
Domain (Entities + Enums + Interfaces)
    ↑
Infrastructure (DbContext + Repositories)

Desktop (Windows Forms + Guna UI2)
    → HttpClient → API REST
```

---

## 🗂️ Estrutura do Projeto

```
ValhallaBebidas/
├── ValhallaBebidas.API/
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.Development.json
│
├── ValhallaBebidas.Application/
│   ├── DTOs/
│   └── Services/
│
├── ValhallaBebidas.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
│
├── ValhallaBebidas.Infrastructure/
│   ├── Data/                    # DbContext + Seeder
│   ├── Repositories/
│   └── Migrations/
│
├── ValhallaBebidas.Web/
│   ├── Controllers/             # Razor Controllers + API Proxy
│   ├── Views/                   # Razor Views (.cshtml)
│   ├── wwwroot/                 # CSS, JS, imagens
│   ├── Filters/                 # AuthFilter
│   └── Models/                  # ViewModels
│
└── ValhallaBebidas.UI/          # Desktop WinForms + Guna
    ├── DTO/
    ├── Services/
    ├── Resources/
    ├── FrmLogin.cs
    ├── frmPrincipal.cs
    ├── frmCadastroFuncionario.cs
    ├── ucDashboard.cs
    ├── ucClientes.cs
    ├── ucFuncionarios.cs
    ├── ucProdutos.cs
    ├── ucPedidos.cs
    ├── ucMovimentacoes.cs
    ├── ucNovoCliente.cs
    ├── ucNovoFuncionario.cs
    ├── ucNovoPedido.cs
    ├── ucNovoProduto.cs
    └── ucNovaMovimentacao.cs
```

---

## 📄 Páginas Web

### Landing Page (pública)

| Seção | Descrição |
|---|---|
| **Nav** | Fixo, com estados visitante e logado |
| **Hero** | Título principal + CTA |
| **Brands** | Marquee com marcas parceiras |
| **Stats** | Indicadores da empresa |
| **About** | Sobre + cards de benefícios |
| **Work** | Como funciona em 3 passos |
| **Footer** | Links e redes sociais |

### Login / Cadastro

| Página | Descrição |
|---|---|
| **Login** | Validação via API, sessão server-side |
| **Cadastro** | Formulário completo com endereço via ViaCEP |

### Área autenticada

| Página | Descrição |
|---|---|
| **Catálogo** | Produtos com filtro por categoria, busca e ordenação |
| **Produto** | Informações completas, estoque, adicionar ao carrinho |
| **Carrinho** | Sidebar com itens, quantidades e total |
| **Checkout** | Endereço de entrega + método de pagamento |
| **Confirmação** | Resumo do pedido confirmado |
| **Minhas Compras** | Histórico de pedidos com filtro por status |
| **Perfil** | Dados pessoais, endereço e alteração de senha |

---

## 🎨 Design System

### Paleta de Cores

| Token | Valor | Uso |
|---|---|---|
| `--color-bg` | `#0F0E0C` | Fundo principal |
| `--color-surface` | `#1B1B1B` | Cards e superfícies |
| `--color-gold` | `#D6BD77` | Cor de destaque |
| `--color-gold-hover` | `#E8D08E` | Hover dos elementos dourados |
| `--color-white` | `#FFFFFF` | Textos principais |
| `--color-muted` | `#606060` | Textos secundários |
| `--color-border` | `#404040` | Bordas e divisores |

### Tipografia

- **Web:** Sora (Google Fonts) — pesos 100 a 800
- **Desktop:** Guna UI2 padrão com customizações

---

## 🔄 Fluxo da Aplicação

### Web (cliente B2B)

```
Landing Page (pública)
    ↓
Login / Cadastro  →  POST /api/auth/login-cliente  →  Session server-side
    ↓
Catálogo  →  GET /api/produto
    ↓
Produto   →  GET /api/produto/{id}
    ↓
Carrinho (localStorage)
    ↓
Checkout  →  POST /api/pedido  →  Baixa estoque + registra movimentação
    ↓
Confirmação  →  exibe resumo do pedido
```

### Desktop (funcionário)

```
FrmLogin  →  POST /api/funcionario/login  →  BCrypt validation
    ↓
frmPrincipal (shell com sidebar Guna)
    ├── ucDashboard      →  GET /api/dashboard
    ├── ucProdutos       →  GET/POST/PUT/PATCH/DELETE /api/produto
    ├── ucPedidos        →  GET/PUT /api/pedido
    ├── ucClientes       →  GET /api/cliente
    ├── ucMovimentacoes  →  GET/POST /api/movimentacao
    └── ucFuncionarios   →  GET/POST/PUT/DELETE /api/funcionario
```

---

## 📦 Como Rodar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/) ou LocalDB
- Visual Studio 2022+ (recomendado para o Windows Forms)

### 1. Clone o repositório

```bash
git clone https://github.com/RodrigolsBento/ValhallaBebidas.git
cd ValhallaBebidas
```

### 2. Configure a conexão

Edite `ValhallaBebidas.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "ValhallaBebidasConnection": "Server=(localdb)\\mssqllocaldb;Database=ValhallaBebidasDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Para SQL Server local, substitua por:

```
"Server=localhost;Database=ValhallaBebidasDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 3. Aplique as migrations

```bash
dotnet ef migrations add InitialCreate \
  --project ValhallaBebidas.Infrastructure \
  --startup-project ValhallaBebidas.API

dotnet ef database update \
  --project ValhallaBebidas.Infrastructure \
  --startup-project ValhallaBebidas.API
```

### 4. Inicie os projetos

```bash
# Terminal 1 — API (http://localhost:5101)
dotnet run --project ValhallaBebidas.API

# Terminal 2 — Web MVC
dotnet run --project ValhallaBebidas.Web
```

Para o **Desktop**: abra `ValhallaBebidas.slnx` no Visual Studio e execute `ValhallaBebidas.UI`.

### 5. Credenciais padrão

| Tipo | Login | Senha |
|---|---|---|
| Funcionário (admin) | `admin` | `adminValhalla` |

### 6. Swagger

Com a API rodando, acesse: `http://localhost:5101/`

---

## 🗄️ Modelagem do Banco

```
Cliente
├── Id, NomeCliente, Email, SenhaHash (BCrypt)
├── Documento (CPF/CNPJ), Telefone, DataNascimento
├── Status (bool — soft delete)
├── EnderecoId → Endereco
└── Pedidos (ICollection)

Endereco
└── Id, Logradouro, Numero, Complemento, Cep, Bairro, Cidade, Estado

Funcionario
├── Id, NomeCompleto, Login, SenhaHash (BCrypt)
├── Cpf, Email, Telefone, DataNascimento
├── Status (bool — soft delete)
└── EnderecoId → Endereco

Produto
├── Id, Nome, Ean, Descricao
├── PrecoVenda, PrecoCusto
├── QuantidadeEstoque, QuantidadeMinimo
├── Status (bool), DataCadastro
├── FotoProduto (caminho relativo)
├── CategoriaId → Categoria
└── ItensPedido, Movimentacoes

Categoria
├── Id, Nome
└── Produtos (ICollection)

Pedido
├── Id, ClienteId → Cliente
├── ValorTotal, DataPedido (UTC)
├── Status (Pendente | Confirmado | Cancelado)
├── EnderecoEntrega* (flattened — 7 campos nullable)
└── Itens (ICollection)

ItemPedido
├── Id, PedidoId → Pedido, ProdutoId → Produto
├── Quantidade, PrecoUnitario
└── Subtotal (calculado — não persiste)

Movimentacao
├── Id, ProdutoId → Produto
├── Quantidade, Direcao (Entrada | Saida)
├── Motivo, Data (UTC)
└── ValorImpactoEstoque (calculado — não persiste)
```

---

## 🔐 Autenticação

### Cliente — Web

- Login via `POST /api/auth/login-cliente` com BCrypt
- Credenciais armazenadas em **sessão server-side** (`HttpContext.Session`)
- `AuthFilter` protege rotas que exigem autenticação
- Logout limpa a sessão e redireciona para Home

### Funcionário — Desktop

- Login via `POST /api/funcionario/login` com BCrypt
- Status `false` bloqueia o acesso
- Credenciais mantidas na memória da aplicação desktop

---

## 🏗️ Funcionalidades Implementadas

### Web

- [x] Login e cadastro com validação frontend + server-side
- [x] Sessão server-side segura (sem localStorage para auth)
- [x] Catálogo com filtro por categoria, busca textual e ordenação
- [x] Carrinho com persistência no localStorage
- [x] Checkout com endereço preenchido via ViaCEP
- [x] Registro de pedido com baixa automática de estoque
- [x] Minhas Compras com filtro por status
- [x] Perfil com atualização de dados, endereço e senha
- [x] Animações GSAP + Lenis em todas as páginas

### Desktop (Guna UI2)

- [x] Login com validação BCrypt
- [x] Dashboard com indicadores de vendas e estoque
- [x] CRUD completo de produtos
- [x] CRUD completo de funcionários
- [x] Gestão e visualização de clientes
- [x] Acompanhamento e atualização de pedidos
- [x] Registro e histórico de movimentações de estoque

### Arquitetura e Infraestrutura

- [x] Clean Architecture com 4 camadas bem definidas
- [x] Repository Pattern + Unit of Work
- [x] BCrypt para todas as senhas
- [x] Soft delete via campo `Status` booleano
- [x] Migrations EF Core com data seeding automático
- [x] Seed de 8 categorias + admin no primeiro startup
- [x] CORS configurado (restringir em produção)

---

## 🛒 Carrinho

O carrinho é mantido no **localStorage** do navegador:

- Ícone na nav visível apenas para usuários logados
- Persiste entre páginas e recarregamentos da sessão
- Limpo automaticamente ao fazer logout
- No checkout, os itens são enviados via `POST /api/pedido`, que valida estoque e registra a baixa automaticamente

---

## 🗺️ Roadmap

- [ ] Deploy em nuvem (Azure / Railway)

---

## 👨‍💻 Autor

Desenvolvido por **TecnoMancy**

---

*Projeto acadêmico fictício — todos os dados são simulados.*
