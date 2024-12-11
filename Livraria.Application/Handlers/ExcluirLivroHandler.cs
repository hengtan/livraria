using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Shared;

namespace Livraria.Application.Handlers;

public class ExcluirLivroHandler(IRepository<Livro> repository, IMessageBus messageBus)
{
    public async Task HandleAsync(Guid id)
    {
        // Buscar entidade no SQL
        var livro = await repository.GetByIdAsync(id);
        if (livro == null)
        {
            throw new KeyNotFoundException($"Livro com ID {id} não encontrado.");
        }

        // Remover do banco SQL
        await repository.DeleteAsync(livro);

        // Publicar evento no Kafka
        var evento = new LivroExcluidoEvent()
        {
            Id = livro.Id
        };

        await messageBus.PublishAsync("livros-excluido", evento);
    }
}