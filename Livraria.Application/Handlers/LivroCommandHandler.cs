// namespace Livraria.Application.Handlers;
//
// public class LivroCommandHandler : ILivroCommandHandler
// {
//     private readonly IRepository<Livro> _repository;
//
//     public LivroCommandHandler(IRepository<Livro> repository)
//     {
//         _repository = repository;
//     }
//
//     public async Task<CreateLivroResult> HandleCreateAsync(CreateLivroCommand command)
//     {
//         var livro = new Livro(command.Titulo, command.Autor, command.Preco, command.DataPublicacao);
//         await _repository.AddAsync(livro);
//
//         return new CreateLivroResult
//         {
//             Id = livro.Id,
//             Titulo = livro.Titulo,
//             Autor = livro.Autor
//         };
//     }
//
//     public async Task<bool> HandleUpdateAsync(UpdateLivroCommand command)
//     {
//         var livro = await _repository.GetByIdAsync(command.Id);
//         if (livro == null) return false;
//
//         livro.AlterarTitulo(command.Titulo);
//         livro.AtualizarPreco(command.Preco);
//         await _repository.UpdateAsync(livro);
//
//         return true;
//     }
//
//     public async Task<bool> HandleDeleteAsync(Guid id)
//     {
//         var livro = await _repository.GetByIdAsync(id);
//         if (livro == null) return false;
//
//         await _repository.DeleteAsync(livro);
//         return true;
//     }
// }