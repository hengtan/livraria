using Livraria.Application.Handlers;
using Livraria.Domain.Entities;
using Livraria.Infrastructure.Repositories;

namespace Livraria.Application.Queries;

public class LivroQueryHandler : ILivroQueryHandler
{
    private readonly LivroLeituraRepository _leituraRepository;

    public LivroQueryHandler(LivroLeituraRepository leituraRepository)
    {
        _leituraRepository = leituraRepository;
    }

    public async Task<IEnumerable<Livro>> GetAllLivrosAsync()
    {
        return await _leituraRepository.GetAllAsync();
    }

    public async Task<Livro?> GetLivroByIdAsync(Guid id)
    {
        return await _leituraRepository.GetByIdAsync(id);
    }
}