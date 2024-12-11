using Livraria.Domain.Entities;
using Livraria.Shared;
using MongoDB.Driver;

namespace Livraria.Infrastructure.Repositories;

public class LivroLeituraRepository(IMongoDatabase database) : IRepository<Livro>
{
    private readonly IMongoCollection<Livro> _collection = database.GetCollection<Livro>("Livros");

    public async Task AddAsync(Livro livro)
    {
        try
        {
            // Atualiza o documento se o ID já existir ou o insere caso contrário
            await _collection.ReplaceOneAsync(
                x => x.Id == livro.Id, // Filtro pelo ID
                livro, // Documento a ser salvo
                new ReplaceOptions { IsUpsert = true } // Insere ou atualiza
            );
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            Console.WriteLine($"[MongoDB] Erro de chave duplicada: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MongoDB] Erro inesperado: {ex.Message}");
            throw; // Repropaga a exceção para depuração
        }
    }

    public async Task<Livro?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Livro>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(Livro livro)
    {
        // Gera a definição de atualização com base no objeto completo, excluindo o ID
        var updateDefinition = Builders<Livro>.Update
            .Set(x => x.Titulo, livro.Titulo)
            .Set(x => x.Autor, livro.Autor)
            .Set(x => x.Preco, livro.Preco)
            .Set(x => x.DataPublicacao, livro.DataPublicacao)
            .Set(x => x.Timestamp, livro.Timestamp);

        // Executa a atualização no MongoDB
        await _collection.UpdateOneAsync(
            filter: Builders<Livro>.Filter.Eq(x => x.Id, livro.Id), // Filtra pelo ID
            update: updateDefinition // Atualiza os campos especificados
        );
    }

    public async Task DeleteAsync(Livro livro)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == livro.Id);

        if (result.DeletedCount == 0)
        {
            throw new InvalidOperationException($"Livro com ID {livro.Id} não encontrado para exclusão.");
        }
    }
}