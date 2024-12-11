using Livraria.Domain.Entities;
using Livraria.Infrastructure.Repositories;

namespace Livraria.Application.Handlers;

public class ConsultarLivrosQueryHandler
{
    private readonly LivroLeituraRepository _leituraRepository;

    public ConsultarLivrosQueryHandler(LivroLeituraRepository leituraRepository)
    {
        _leituraRepository = leituraRepository;
    }

    public async Task<IEnumerable<Livro>> HandleAsync()
    {
        return await _leituraRepository.GetAllAsync();
    }

    public async Task<Livro?> HandleByIdAsync(Guid id)
    {
        return await _leituraRepository.GetByIdAsync(id);
    }
}