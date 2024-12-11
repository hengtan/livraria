using Livraria.Domain.Entities;
using Livraria.Domain.Events;
using Livraria.Shared;

namespace Livraria.Application.UseCases;

public class CriarLivroHandler
{
    private readonly IRepository<Livro> _repository;
    private readonly IMessageBus _messageBus;

    public CriarLivroHandler(IRepository<Livro> repository, IMessageBus messageBus)
    {
        _repository = repository;
        _messageBus = messageBus;
    }

    public async Task HandleAsync(Livro livro)
    {
        await _repository.AddAsync(livro);

        var evento = new LivroCriadoEvent
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Autor = livro.Autor
        };

        await _messageBus.PublishAsync("livros", evento);
    }
}