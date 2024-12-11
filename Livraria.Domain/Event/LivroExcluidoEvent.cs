namespace Livraria.Domain.Event;

public class LivroExcluidoEvent
{
    public Guid Id { get; set; } // Identificador único do livro que foi excluído
}