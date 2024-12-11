using Livraria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Infrastructure.Data;

public class LivrariaDbContext : DbContext
{
    public LivrariaDbContext(DbContextOptions<LivrariaDbContext> options) : base(options)
    {
    }

    public DbSet<Livro> Livros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração da entidade Livro
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(l => l.Id); // Chave primária

            entity.Property(l => l.Titulo)
                .HasMaxLength(200) // Limite de caracteres para Titulo
                .IsRequired(); // Campo obrigatório

            entity.Property(l => l.Autor)
                .HasMaxLength(100) // Limite de caracteres para Autor
                .IsRequired(); // Campo obrigatório

            entity.Property(l => l.Preco)
                .HasColumnType("decimal(18,2)") // Configuração de precisão decimal
                .IsRequired(); // Campo obrigatório

            entity.Property(l => l.DataPublicacao)
                .IsRequired(); // Campo obrigatório

            entity.Property(l => l.Timestamp)
                .HasColumnType("datetime2") // Tipo datetime para maior precisão
                .IsRequired(); // Campo obrigatório
        });
    }
}