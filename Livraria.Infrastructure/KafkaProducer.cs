// using Confluent.Kafka;
// using System.Text.Json;
// using Livraria.Domain.Event;
//
// namespace Livraria.Infrastructure.Messaging;
//
// public class KafkaProducer
// {
//     private readonly IProducer<Null, string> _producer;
//
//     public KafkaProducer(string broker)
//     {
//         var config = new ProducerConfig { BootstrapServers = broker };
//         _producer = new ProducerBuilder<Null, string>(config).Build();
//     }
//
//     public async Task PublicarAsync(LivroCriadoEvent evento, string topico)
//     {
//         var mensagem = JsonSerializer.Serialize(evento);
//         await _producer.ProduceAsync(topico, new Message<Null, string> { Value = mensagem });
//     }
// }

