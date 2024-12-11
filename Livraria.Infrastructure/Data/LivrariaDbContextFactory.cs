using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Livraria.Infrastructure.Data;

public class LivrariaDbContextFactory : IDesignTimeDbContextFactory<LivrariaDbContext>
{
    public LivrariaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LivrariaDbContext>();

        // Configure a string de conexão (ajuste conforme necessário)
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=Livraria;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;");

        return new LivrariaDbContext(optionsBuilder.Options);
    }
}