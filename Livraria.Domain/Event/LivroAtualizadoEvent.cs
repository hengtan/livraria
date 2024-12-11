namespace Livraria.Domain.Event;

public class LivroAtualizadoEvent
{
    public Guid Id { get; set; } // Identificador único do livro
    public string Titulo { get; set; } // Novo título do livro
    public string Autor { get; set; } // Autor
    public decimal Preco { get; set; } // Novo preço
    public DateTime DataPublicacao { get; set; } // Data de publicação
}