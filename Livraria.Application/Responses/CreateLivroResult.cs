namespace Livraria.Application.Responses;

public class CreateLivroResult
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
}