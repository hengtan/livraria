using Livraria.Application.Commands;
using Livraria.Application.Handlers;
using Livraria.Application.Responses;

namespace Livraria.Application.Interfaces;

public interface ILivroCommandHandler
{
    Task<AtualizarLivroHandler> HandleCreateAsync(AtualizarLivroHandler command);
    Task<bool> HandleUpdateAsync(AtualizarLivroHandler command);
    Task<bool> HandleDeleteAsync(Guid id);
}