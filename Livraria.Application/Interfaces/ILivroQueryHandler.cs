using Livraria.Domain.Entities;

namespace Livraria.Application.Handlers;

public interface ILivroQueryHandler
{
    Task<IEnumerable<Livro>> GetAllLivrosAsync();
    Task<Livro?> GetLivroByIdAsync(Guid id);
}