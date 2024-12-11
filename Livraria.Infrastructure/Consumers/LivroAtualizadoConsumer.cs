using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Infrastructure.Repositories;
using Livraria.Shared;
using MongoDB.Driver;

namespace Livraria.Infrastructure.Consumers;

public class LivroAtualizadoConsumer(LivroLeituraRepository leituraRepository) : IConsumer<LivroAtualizadoEvent>
{
    public async Task ConsumeAsync(LivroAtualizadoEvent message)
    {
        try
        {
            var livro = new Livro(message.Titulo, message.Autor, message.Preco, message.DataPublicacao)
            {
                Id = message.Id // Garante que o ID seja consistente
            };

            await leituraRepository.UpdateAsync(livro);
            Console.WriteLine($"[KafkaConsumer] Livro salvo com sucesso no MongoDB: {livro.Titulo}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KafkaConsumer] Erro ao salvar no MongoDB: {ex.Message}");
        }
    }
}