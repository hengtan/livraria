using System.Data;
using Dapper;
using Livraria.Domain.Entities;
using Livraria.Shared;

namespace Livraria.Infrastructure.Repositories;

public class LivroRepository : IRepository<Livro>
{
    private readonly IDbConnection _dbConnection;

    public LivroRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Livro?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM Livros WHERE Id = @Id";
        var test = await _dbConnection.QueryFirstOrDefaultAsync<Livro>(sql, new { Id = id });
        return await _dbConnection.QueryFirstOrDefaultAsync<Livro>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Livro>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Livros";
        return await _dbConnection.QueryAsync<Livro>(sql);
    }

    public async Task AddAsync(Livro entity)
    {
        const string sql = @"
            INSERT INTO Livros (Id, Titulo, Autor, Preco, DataPublicacao, Timestamp)
            VALUES (@Id, @Titulo, @Autor, @Preco, @DataPublicacao, @Timestamp)";
        await _dbConnection.ExecuteAsync(sql, new
        {
            entity.Id,
            entity.Titulo,
            entity.Autor,
            entity.Preco,
            entity.DataPublicacao,
            entity.Timestamp
        });
    }

    public async Task UpdateAsync(Livro entity)
    {
        const string sql = @"
            UPDATE Livros
            SET Titulo = @Titulo, Autor = @Autor, Preco = @Preco, DataPublicacao = @DataPublicacao
            WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new
        {
            entity.Id,
            entity.Titulo,
            entity.Autor,
            entity.Preco,
            entity.DataPublicacao
        });
    }

    public async Task DeleteAsync(Livro entity)
    {
        const string sql = "DELETE FROM Livros WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { entity.Id });
    }
}