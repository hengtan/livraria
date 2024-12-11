using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Infrastructure.Repositories;
using Livraria.Shared;

namespace Livraria.Infrastructure.Consumers;

public class LivroCriadoConsumer : IConsumer<LivroCriadoEvent>
{
    private readonly LivroLeituraRepository _leituraRepository;

    public LivroCriadoConsumer(LivroLeituraRepository leituraRepository)
    {
        _leituraRepository = leituraRepository;
    }

    public async Task ConsumeAsync(LivroCriadoEvent message)
    {
        var livro = new Livro(message.Titulo, message.Autor, message.Preco, message.DataPublicacao);
        await _leituraRepository.AddAsync(livro);
    }
}


// using System.Collections.Concurrent;
// using Livraria.Domain.Event;
// using Livraria.Shared;
// using MongoDB.Driver;
//
// namespace Livraria.Infrastructure.Consumers;
//
// public class LivroCriadoConsumer : IConsumer<LivroCriadoEvent>
// {
//     private readonly int _batchSize = 10; // Tamanho do lote para inserção
//     private readonly CancellationTokenSource _cancellationTokenSource;
//     private readonly IMongoCollection<LivroCriadoEvent> _collection;
//     private readonly BlockingCollection<LivroCriadoEvent> _eventQueue;
//
//     public LivroCriadoConsumer(IMongoDatabase database)
//     {
//         _collection = database.GetCollection<LivroCriadoEvent>("Livros");
//         _eventQueue = new BlockingCollection<LivroCriadoEvent>();
//         _cancellationTokenSource = new CancellationTokenSource();
//
//         // Inicia o processador em lote
//         Task.Run(() => ProcessBatchAsync(_cancellationTokenSource.Token));
//     }
//
//     public async Task ConsumeAsync(LivroCriadoEvent evento)
//     {
//         // Adiciona o evento na fila para processamento em lote
//         if (evento != null)
//         {
//             _eventQueue.Add(evento);
//             Console.WriteLine($"[Consumer] Evento enfileirado: {evento.Titulo}");
//         }
//     }
//
//     private async Task ProcessBatchAsync(CancellationToken cancellationToken)
//     {
//         var batch = new List<LivroCriadoEvent>();
//
//         while (!cancellationToken.IsCancellationRequested)
//             try
//             {
//                 // Tenta obter eventos da fila
//                 LivroCriadoEvent evento;
//                 while (_eventQueue.TryTake(out evento, TimeSpan.FromSeconds(1)) && batch.Count < _batchSize)
//                     batch.Add(evento);
//
//                 if (batch.Count > 0)
//                 {
//                     Console.WriteLine($"[BatchProcessor] Inserindo lote de {batch.Count} eventos no MongoDB...");
//
//                     // Insere o lote no MongoDB
//                     await _collection.InsertManyAsync(batch, cancellationToken: cancellationToken);
//
//                     Console.WriteLine($"[BatchProcessor] Lote de {batch.Count} eventos inserido com sucesso.");
//                     batch.Clear();
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"[BatchProcessor] Erro ao processar lote: {ex.Message}");
//             }
//     }
//
//     public void StopProcessing()
//     {
//         // Encerra o processamento em lote
//         _cancellationTokenSource.Cancel();
//         _eventQueue.CompleteAdding();
//     }
// }