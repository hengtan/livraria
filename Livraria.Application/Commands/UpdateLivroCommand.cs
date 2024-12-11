namespace Livraria.Application.Commands;

public class UpdateLivroCommand
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataPublicacao { get; set; }
}