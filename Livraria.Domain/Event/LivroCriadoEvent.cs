namespace Livraria.Domain.Event;

public class LivroCriadoEvent
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataPublicacao { get; set; }

    // Marca temporal do evento
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}