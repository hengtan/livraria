using Livraria.Application.Commands;
using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Shared;

namespace Livraria.Application.Handlers;

public class CriarLivroHandler(IRepository<Livro> repository, IMessageBus messageBus)
{
    public async Task HandleAsync(CreateLivroCommand command)
    {
        // Criar entidade Livro
        var livro = new Livro(command.Titulo, command.Autor, command.Preco, command.DataPublicacao);

        // Gravar no banco SQL
        await repository.AddAsync(livro);

        // Publicar evento no Kafka
        var evento = new LivroCriadoEvent()
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Autor = livro.Autor,
            Preco = livro.Preco,
            DataPublicacao = livro.DataPublicacao
        };

        await messageBus.PublishAsync("livros-criado", evento);
    }
}

// using Livraria.Domain.Entities;
// using Livraria.Domain.Event;
// using Livraria.Shared;
//
// namespace Livraria.Application.Handlers;
//
// public class CriarLivroHandler(IRepository<Livro> repository, IMessageBus messageBus)
// {
//     public async Task HandleAsync(Livro livro)
//     {
//         // Salvar no banco SQL
//         Console.WriteLine($"[Handler] Salvando livro no banco SQL: {livro.Titulo}");
//         livro.Id = Guid.NewGuid();
//         livro.Timestamp = DateTime.UtcNow.AddHours(-3);
//
//         await repository.AddAsync(livro);
//
//         // Criar o evento
//         var evento = new LivroCriadoEvent
//         {
//             Id = livro.Id,
//             Titulo = livro.Titulo,
//             Autor = livro.Autor,
//             Preco = livro.Preco,
//             DataPublicacao = livro.DataPublicacao
//         };
//
//         // Publicar o evento no Message Bus
//         Console.WriteLine($"[Handler] Publicando evento no message bus: {evento.Titulo}");
//         await messageBus.PublishAsync("livros", evento);
//     }
// }