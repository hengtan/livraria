using Livraria.Application.Commands;
using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Shared;

namespace Livraria.Application.Handlers;

public class AtualizarLivroHandler(IRepository<Livro> repository, IMessageBus messageBus)
{
    public async Task HandleAsync(UpdateLivroCommand command)
    {
        // Buscar entidade no SQL
        var livro = await repository.GetByIdAsync(command.Id);
        if (livro == null)
        {
            throw new KeyNotFoundException($"Livro com ID {command.Id} não encontrado.");
        }

        // Atualizar os dados
        livro.AlterarTitulo(command.Titulo);
        livro.AtualizarPreco(command.Preco);

        // Gravar no banco SQL
        await repository.UpdateAsync(livro);

        // Publicar evento no Kafka
        var evento = new LivroAtualizadoEvent()
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Autor = livro.Autor,
            Preco = livro.Preco,
            DataPublicacao = livro.DataPublicacao
        };

        await messageBus.PublishAsync("livros-atualizado", evento);
    }
}