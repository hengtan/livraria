using Livraria.Infrastructure.Repositories;

namespace Livraria.Application.Queries;

public class ObterLivrosQueryHandler(LivroLeituraRepository repository)
{
    private readonly LivroLeituraRepository _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<IEnumerable<object>> HandleAsync()
    {
        var livros = await _repository.GetAllAsync();
        return livros.Select(livro => new
        {
            livro.Id,
            livro.Titulo,
            livro.Autor,
            livro.Preco,
            livro.DataPublicacao,
            livro.Timestamp
        });
    }
}