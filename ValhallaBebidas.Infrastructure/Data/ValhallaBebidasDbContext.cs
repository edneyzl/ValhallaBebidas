using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Infrastructure.Data;

public class ValhallaBebidasDbContext : DbContext, IUnitOfWork
{
    public ValhallaBebidasDbContext(DbContextOptions<ValhallaBebidasDbContext> options) : base(options)
    {
    }

    // ════════════════════════════════════════════════════════
    // DbSets — tabelas do banco
    // ════════════════════════════════════════════════════════
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Movimentacao> Movimentacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ════════════════════════════════════════════════════════
        // ENDERECO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Numero).IsRequired();
            entity.Property(e => e.Complemento).HasMaxLength(150);
            entity.Property(e => e.Cep).IsRequired().HasMaxLength(9);
            entity.Property(e => e.Bairro).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(2);
            entity.HasIndex(e => e.Cep);
        });

        // ════════════════════════════════════════════════════════
        // CATEGORIA
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(40);
            entity.HasIndex(c => c.Nome).IsUnique();
        });

        // ════════════════════════════════════════════════════════
        // CLIENTE
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.NomeCliente).IsRequired().HasMaxLength(200);
            entity.Property(c => c.DataNascimento).HasColumnType("datetime");
            entity.Property(c => c.Documento).IsRequired().HasMaxLength(20);
            entity.Property(c => c.Telefone).HasMaxLength(20);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
            entity.Property(c => c.SenhaHash).IsRequired().HasMaxLength(500);
            entity.Property(c => c.Status).IsRequired();
            /* EnderecoId nullable — cliente pode cadastrar sem endereço */
            entity.Property(c => c.EnderecoId).IsRequired(false);
            entity.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey(c => c.EnderecoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(c => c.Documento).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();
        });

        // ════════════════════════════════════════════════════════
        // FUNCIONARIO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.NomeCompleto).IsRequired().HasMaxLength(200);
            entity.Property(f => f.DataNascimento).HasColumnType("datetime");
            entity.Property(f => f.Cpf).IsRequired().HasMaxLength(11);
            entity.Property(f => f.Telefone).HasMaxLength(20);
            entity.Property(f => f.Email).IsRequired().HasMaxLength(150);
            entity.Property(f => f.Login).IsRequired().HasMaxLength(80);
            entity.Property(f => f.SenhaHash).IsRequired().HasMaxLength(500);
            entity.Property(f => f.Status).IsRequired();

            /* EnderecoId obrigatório — funcionário deve ter endereço */
            entity.Property(f => f.EnderecoId).IsRequired();

            entity.HasOne(f => f.Endereco)
                .WithMany()
                .HasForeignKey(f => f.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(f => f.Cpf).IsUnique();
            entity.HasIndex(f => f.Email).IsUnique();
            entity.HasIndex(f => f.Login).IsUnique();
        });

        // ════════════════════════════════════════════════════════
        // PRODUTO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Ean).IsRequired().HasMaxLength(13);
            entity.Property(p => p.Descricao).HasMaxLength(500);
            entity.Property(p => p.PrecoVenda).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.PrecoCusto).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.QuantidadeEstoque).IsRequired();
            entity.Property(p => p.QuantidadeMinimo).IsRequired();
            entity.Property(p => p.DataCadastro).IsRequired().HasColumnType("datetime2");
            entity.Property(p => p.Status).IsRequired();
            entity.Property(p => p.FotoProduto).HasMaxLength(300);

            /* CategoriaId nullable — produto pode existir sem categoria */
            entity.Property(p => p.CategoriaId).IsRequired(false);

            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.ItensPedido)
                .WithOne(i => i.Produto)
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.Ean).IsUnique();
            entity.HasIndex(p => p.Nome);
            entity.HasIndex(p => p.CategoriaId);
        });

        // ════════════════════════════════════════════════════════
        // PEDIDO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.ClienteId).IsRequired();
            entity.Property(p => p.DataPedido).IsRequired().HasColumnType("datetime2");
            entity.Property(p => p.ValorTotal).IsRequired().HasColumnType("decimal(18,2)");

            /* Status salvo como string para legibilidade no banco */
            entity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            /* Endereço de entrega (flattened value object) */
            entity.Property(p => p.EnderecoEntregaLogradouro).HasMaxLength(200);
            entity.Property(p => p.EnderecoEntregaNumero).HasMaxLength(10);
            entity.Property(p => p.EnderecoEntregaComplemento).HasMaxLength(150);
            entity.Property(p => p.EnderecoEntregaBairro).HasMaxLength(120);
            entity.Property(p => p.EnderecoEntregaCidade).HasMaxLength(120);
            entity.Property(p => p.EnderecoEntregaEstado).HasMaxLength(2);
            entity.Property(p => p.EnderecoEntregaCep).HasMaxLength(9);

            entity.HasIndex(p => p.ClienteId);
            entity.HasIndex(p => p.DataPedido);
            entity.HasIndex(p => new { p.ClienteId, p.DataPedido });
        });

        // ════════════════════════════════════════════════════════
        // ITEM PEDIDO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.PedidoId).IsRequired();
            entity.Property(i => i.ProdutoId).IsRequired();
            entity.Property(i => i.Quantidade).IsRequired();

            entity.Ignore(i => i.Subtotal);

            entity.HasIndex(i => i.PedidoId);
            entity.HasIndex(i => i.ProdutoId);

            entity.HasIndex(i => new { i.PedidoId, i.ProdutoId }).IsUnique();

            // 🔗 Relacionamentos
            entity.HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId);

            entity.HasOne(i => i.Produto)
                .WithMany(p => p.ItensPedido) // 🔥FIO ESSENCIAL
                .HasForeignKey(i => i.ProdutoId);
        });

        // ════════════════════════════════════════════════════════
        // MOVIMENTACAO
        // ════════════════════════════════════════════════════════
        modelBuilder.Entity<Movimentacao>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.ProdutoId).IsRequired();
            entity.Property(m => m.Quantidade).IsRequired();
            entity.Property(m => m.Motivo).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Data).IsRequired().HasColumnType("datetime2");

            /* Direcao salvo como string para legibilidade no banco */
            entity.Property(m => m.Direcao)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10);

            /* ValorImpactoEstoque é propriedade calculada — não persiste no banco */
            entity.Ignore(m => m.ValorImpactoEstoque);

            entity.HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(m => m.ProdutoId);
            entity.HasIndex(m => m.Data);
        });
    }
}