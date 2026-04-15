# 🍺 Valhalla Bebidas

> Distribuidora premium de bebidas. Plataforma B2B para parceiros comerciais realizarem pedidos de produtos das maiores marcas do mercado.

---

## 📋 Sobre o Projeto

A **Valhalla Bebidas** é uma aplicação completa para distribuidoras de bebidas, permitindo que parceiros comerciais acessem o catálogo exclusivo, adicionem produtos ao carrinho e realizem pedidos online com preços especiais.

O projeto é **full-stack**, dividido em camadas:

- **Frontend Web** — ASP.NET Core MVC com Razor Views + JavaScript
- **Backend API** — .NET 10 REST API com Clean Architecture
- **Desktop Interno** — Windows Forms com Guna UI2 (gerenciamento administrativo)
- **Banco de Dados** — SQL Server via Entity Framework Core

---

## 🚀 Tecnologias

### Frontend Web (ASP.NET Core MVC)
| Tecnologia | Uso |
|---|---|
| ASP.NET Core MVC | Framework web |
| Razor Views | Templates server-side |
| JavaScript (ES6+) | Interações e requisições à API |
| [GSAP 3.12](https://gsap.com/) | Animações de scroll e entrada |
| [ScrollTrigger](https://gsap.com/docs/v3/Plugins/ScrollTrigger/) | Trigger de animações |
| [Lenis](https://github.com/darkroomengineering/lenis) | Scroll suave |
| [Sora](https://fonts.google.com/specimen/Sora) | Tipografia |

### Desktop Interno (Windows Forms)
| Tecnologia | Uso |
|---|---|
| Windows Forms (.NET) | Framework desktop |
| [Guna UI2](https://gunaui.com/) | Componentes visuais modernos |
| HttpClient | Comunicação com a API REST |

#### Telas implementadas
| Arquivo | Descrição |
|---|---|
| `FrmLogin` | Autenticação do funcionário |
| `frmPrincipal` | Shell principal com navegação |
| `frmCadastroFuncionario` | Cadastro de funcionários |
| `ucDashboard` | Painel com indicadores e gráficos |
| `ucClientes` | Listagem e gestão de clientes |
| `ucFuncionarios` | Listagem e gestão de funcionários |
| `ucProdutos` | Listagem e gestão de produtos |
| `ucPedidos` | Acompanhamento de pedidos |
| `ucMovimentacoes` | Histórico de movimentações de estoque |
| `ucNovoCliente` | Formulário de novo cliente |
| `ucNovoFuncionario` | Formulário de novo funcionário |
| `ucNovoPedido` | Formulário de novo pedido |
| `ucNovoProduto` | Formulário de novo produto |
| `ucNovaMovimentacao` | Formulário de nova movimentação |

### Backend (API — Clean Architecture)
| Tecnologia | Uso |
|---|---|
| .NET 10 | API REST |
| Entity Framework Core 10 | ORM + migrations |
| SQL Server | Banco de dados |
| BCrypt.Net | Hash de senhas |
| Swagger / OpenAPI | Documentação da API |

### Arquitetura
API (Controllers)
↓
Application (Services + DTOs)
↓
Domain (Entities + Enums + Interfaces)
↑
Infrastructure (DbContext + Repositories)
Desktop (Windows Forms + Guna UI2)
→ HttpClient → API REST

---

## 🗂️ Estrutura do Projeto
ValhallaBebidas/
├── ValhallaBebidas.API/               # API REST
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.Development.json
│
├── ValhallaBebidas.Application/       # Camada de aplicação
│   ├── DTOs/
│   └── Services/
│
├── ValhallaBebidas.Domain/            # Entidades e contratos
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
│
├── ValhallaBebidas.Infrastructure/    # Persistência
│   ├── Data/                          # DbContext + Seeder
│   ├── Repositories/
│   └── Migrations/
│
├── ValhallaBebidas.Web/               # Frontend MVC
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/                       # CSS, JS, imagens
│   ├── Filters/
│   └── Models/
│
└── ValhallaBebidas.UI/                # Desktop interno (WinForms + Guna)
├── DTO/                           # DTOs locais do desktop
├── Services/                      # HttpClient services
├── Resources/                     # Imagens e assets
├── FrmLogin.cs                    # Login do funcionário
├── frmPrincipal.cs                # Shell principal
├── frmCadastroFuncionario.cs      # Cadastro de funcionário
├── ucDashboard.cs                 # Painel administrativo
├── ucClientes.cs                  # Gestão de clientes
├── ucFuncionarios.cs              # Gestão de funcionários
├── ucProdutos.cs                  # Gestão de produtos
├── ucPedidos.cs                   # Gestão de pedidos
├── ucMovimentacoes.cs             # Movimentações de estoque
├── ucNovoCliente.cs
├── ucNovoFuncionario.cs
├── ucNovoPedido.cs
├── ucNovoProduto.cs
└── ucNovaMovimentacao.cs

---

## 📄 Páginas Web

### Landing Page (pública)
| Seção | Descrição |
|---|---|
| **Nav** | Fixo, estados visitante e logado |
| **Hero** | Título principal + CTA |
| **Brands** | Marquee com marcas parceiras |
| **Stats** | Indicadores da empresa |
| **About** | Sobre + cards de benefícios |
| **Footer** | Links e redes sociais |

### Login / Cadastro
| Página | Descrição |
|---|---|
| **Login** | Validação via API, sessão server-side |
| **Cadastro** | Formulário completo com endereço via ViaCEP |

### Autenticado
| Página | Descrição |
|---|---|
| **Catálogo** | Produtos com filtro por categoria + busca + ordenação |
| **Produto** | Info completa, estoque, adicionar ao carrinho |
| **Carrinho** | Sidebar com itens, quantidades e total |
| **Checkout** | Endereço de entrega + método de pagamento |
| **Confirmação** | Pedido confirmado com resumo |
| **Minhas Compras** | Histórico com filtro por status |
| **Perfil** | Dados pessoais, endereço e senha |

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
| `--color-border-btn` | `#404040` | Bordas e secundários |

### Tipografia
- **Web:** Sora (Google Fonts)
- **Desktop:** Guna UI2 default + customizações

---

## 🔄 Fluxo da Aplicação

### Web (cliente)
Landing Page (pública)
↓
Login / Cadastro  →  POST /api/auth/login-cliente  →  Session
↓
Catálogo  →  GET /api/produto
↓
Carrinho (localStorage)
↓
Checkout  →  POST /api/pedido  →  Salva pedido + baixa estoque
↓
Confirmação

### Desktop (funcionário)
FrmLogin  →  POST /api/funcionario/login
↓
frmPrincipal (shell com sidebar)
↓
ucDashboard  →  GET /api/dashboard
ucProdutos   →  GET/POST/PUT/DELETE /api/produto
ucPedidos    →  GET/PUT /api/pedido
ucClientes   →  GET /api/cliente
ucMovimentacoes  →  GET/POST /api/movimentacao

---

## 📦 Como Rodar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/) ou LocalDB
- Visual Studio 2022+ (para o Windows Forms)

### 1. Clone o repositório
```bash
git clone https://github.com/RodrigolsBento/ValhallaBebidas.git
cd ValhallaBebidas
```

### 2. Configure a conexão
`ValhallaBebidas.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "ValhallaBebidasConnection": "Server=(localdb)\\mssqllocaldb;Database=ValhallaBebidasDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
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
# API (http://localhost:5101)
dotnet run --project ValhallaBebidas.API

# Web MVC
dotnet run --project ValhallaBebidas.Web
```

Para o **Desktop**: abra `ValhallaBebidas.slnx` no Visual Studio e execute `ValhallaBebidas.UI`.

### 5. Credenciais padrão (admin)
Login: admin
Senha: adminValhalla

### 6. Swagger
`http://localhost:5101/` com a API rodando.

---

## 🗄️ Modelagem do Banco
Cliente ──────────────── Pedido ──── ItemPedido ──── Produto
└── Endereco               └── (flattened EnderecoEntrega)    └── Categoria
└── Movimentacao
Funcionario
└── Endereco

---

## 🔐 Autenticação

### Cliente (Web)
- Login via `POST /api/auth/login-cliente` com BCrypt
- Sessão server-side (`HttpContext.Session`)
- `AuthFilter` protege rotas autenticadas

### Funcionário (Desktop)
- Login via `POST /api/funcionario/login` com BCrypt
- Status `false` bloqueia acesso

---

## 🏗️ Funcionalidades

### Web
- [x] Login e cadastro com validação
- [x] Catálogo com filtro, busca e ordenação
- [x] Carrinho com persistência local
- [x] Checkout com endereço e pagamento
- [x] Minhas compras com filtro por status
- [x] Perfil com dados, endereço e senha

### Desktop (Guna UI2)
- [x] Login do funcionário
- [x] Dashboard com indicadores
- [x] CRUD de produtos
- [x] CRUD de funcionários
- [x] Gestão de clientes
- [x] Acompanhamento de pedidos
- [x] Movimentações de estoque

### Arquitetura
- [x] Clean Architecture (4 camadas)
- [x] Repository Pattern + Unit of Work
- [x] BCrypt para senhas
- [x] Soft delete por Status booleano
- [x] Migrations + Data seeding

---

## 👨‍💻 Autor

Desenvolvido por **TecnoMancy**

---

*Projeto acadêmico fictício.*
