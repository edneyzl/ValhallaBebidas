using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Infrastructure.Data;

public class ValhallaBebidasDbContext : DbContext
{

    public ValhallaBebidasDbContext(DbContextOptions<ValhallaBebidasDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações de mapeamento usando Fluent API para cada entidade
        base.OnModelCreating(modelBuilder);

        // ====================== CONFIGURAÇÕES DE FUNCIONARIO ======================
        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.NomeCompleto).IsRequired().HasMaxLength(200);
            entity.Property(f => f.DataNascimento).HasColumnType("datetime2");
            entity.Property(f => f.Cpf).IsRequired().HasMaxLength(11);
            entity.Property(f => f.Telefone).HasMaxLength(20);
            entity.Property(f => f.Email).IsRequired().HasMaxLength(150);
            entity.Property(f => f.Login).IsRequired().HasMaxLength(80);
            entity.Property(f => f.SenhaHash).IsRequired().HasMaxLength(500);
            entity.Property(f => f.Status).IsRequired();
            entity.Property(f => f.PesoCargo).IsRequired();
            entity.Property(f => f.EnderecoId).IsRequired();
            // Relacionamento
            entity.HasOne(f => f.Endereco).WithMany().HasForeignKey(f => f.EnderecoId).OnDelete(DeleteBehavior.Restrict);
            // Índices
            entity.HasIndex(f => f.Cpf).IsUnique();
            entity.HasIndex(f => f.Email).IsUnique();
            entity.HasIndex(f => f.Login).IsUnique();
        });

        // ====================== CONFIGURAÇÕES DE ENDEREÇO ======================
        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoLogradouro).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Numero).IsRequired();
            entity.Property(e => e.Complemento).HasMaxLength(150);
            entity.Property(e => e.Cep).IsRequired().HasMaxLength(8);
            entity.Property(e => e.Bairro).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(2);
            entity.HasIndex(e => e.Cep);
        });

        // ====================== CONFIGURAÇÕES DE CATEGORIA ======================
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(40);
            entity.HasIndex(c => c.Nome).IsUnique();
        });


        // ====================== CONFIGURAÇÕES DE ITEM PEDIDO ======================
        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.PedidoId).IsRequired();
            entity.Property(i => i.ProdutoId).IsRequired();
            entity.Property(i => i.Quantidade).IsRequired();
            // Ignorar Subtotal pois é propriedade calculada
            entity.Ignore(i => i.Subtotal);
            // Relacionamento com Pedido
            entity.HasOne(i => i.Pedido).WithMany(p => p.Itens).HasForeignKey(i => i.PedidoId).OnDelete(DeleteBehavior.Cascade);
            // Relacionamento com Produto
            entity.HasOne(i => i.Produto).WithMany().HasForeignKey(i => i.ProdutoId).OnDelete(DeleteBehavior.Restrict);
            // Índices para melhorar consultas
            entity.HasIndex(i => i.PedidoId);
            entity.HasIndex(i => i.ProdutoId);
            // Índice composto para evitar produto duplicado no mesmo pedido
            entity.HasIndex(i => new { i.PedidoId, i.ProdutoId }).IsUnique();
        });

        // ====================== CONFIGURAÇÕES DE PEDIDO ======================
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.DataPedido).IsRequired().HasColumnType("datetime2");
            entity.Property(p => p.ValorTotal).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.ClienteId).IsRequired();
            // Relacionamento com Cliente
            entity.HasOne(p => p.Cliente).WithMany().HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.Restrict);
            // Índices para melhorar performance
            entity.HasIndex(p => p.ClienteId);
            entity.HasIndex(p => p.DataPedido);
            // Índice composto útil para relatórios
            entity.HasIndex(p => new { p.ClienteId, p.DataPedido });
        });

        // ====================== CONFIGURAÇÕES DE PRODUTO ======================
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);

            entity.Property(p => p.EanCodBarras)
                .IsRequired()
                .HasMaxLength(13);

            entity.Property(p => p.Descricao)
                .HasMaxLength(500);

            entity.Property(p => p.PrecoVenda)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(p => p.PrecoCusto)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(p => p.QuantidadeEstoque)
                .IsRequired();

            entity.Property(p => p.QuantidadeMinimo)
                .IsRequired();

            entity.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.Property(p => p.Status)
                .IsRequired();

            entity.Property(p => p.FotoProduto)
                .HasMaxLength(300);

            entity.Property(p => p.CategoriaId)
                .IsRequired();

            // Relacionamento com Categoria
            entity.HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para performance
            entity.HasIndex(p => p.EanCodBarras)
                .IsUnique();

            entity.HasIndex(p => p.Nome);

            entity.HasIndex(p => p.CategoriaId);

            // Índice útil para controle de estoque
            entity.HasIndex(p => new { p.Status, p.QuantidadeEstoque });
        });

        // ====================== CONFIGURAÇÕES DE CLIENTE ======================
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.NomeCliente)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.DataNascimento)
                .HasColumnType("datetime2");

            entity.Property(c => c.Documento)
                .IsRequired()
                .HasMaxLength(14);

            entity.Property(c => c.Telefone)
                .HasMaxLength(20);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Login)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(c => c.SenhaHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(c => c.Status)
                .IsRequired();

            entity.Property(c => c.EnderecoId)
                .IsRequired();

            // Relacionamento com Endereco
            entity.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey(c => c.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para performance e integridade
            entity.HasIndex(c => c.Documento)
                .IsUnique();

            entity.HasIndex(c => c.Email)
                .IsUnique();

            entity.HasIndex(c => c.Login)
                .IsUnique();
        });




    }
}
