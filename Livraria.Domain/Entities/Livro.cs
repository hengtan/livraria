namespace Livraria.Domain.Entities;

public class Livro
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataPublicacao { get; set; }
    public DateTime Timestamp { get; set; } // Adicionado para compatibilidade
    
    // Construtor padrão exigido pelo Dapper
    public Livro()
    {
    }
    
    public Livro(string titulo, string autor, decimal preco, DateTime dataPublicacao)
    {
        Id = Guid.NewGuid(); // Gera um novo ID único
        Titulo = titulo;
        Autor = autor;
        Preco = preco > 0 ? preco : throw new ArgumentException("O preço deve ser maior que zero.");
        DataPublicacao = dataPublicacao;
        Timestamp = DateTime.UtcNow.AddHours(-3);
    }

    // Método para atualizar o título
    public void AlterarTitulo(string novoTitulo)
    {
        if (string.IsNullOrWhiteSpace(novoTitulo))
            throw new ArgumentException("O título não pode ser vazio.");
        Titulo = novoTitulo;
    }

    // Método para atualizar o preço
    public void AtualizarPreco(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");
        Preco = novoPreco;
    }
}