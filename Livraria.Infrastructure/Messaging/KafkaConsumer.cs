using System.Text.Json;
using Confluent.Kafka;

namespace Livraria.Infrastructure.Messaging;

public class KafkaConsumer<T>
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly Func<T, Task> _handler;
    private readonly string _topic;

    public KafkaConsumer(string topic, Func<T, Task> handler)
    {
        _topic = topic;
        _handler = handler;

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092", // Endereço do Kafka
            GroupId = "livraria-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    public void Start()
    {
        _consumer.Subscribe(_topic);

        Console.WriteLine($"[KafkaConsumer] Consumindo mensagens do tópico '{_topic}'");

        Task.Run(async () =>
        {
            while (true)
                try
                {
                    var result = _consumer.Consume();
                    var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                    Console.WriteLine($"[KafkaConsumer] Mensagem recebida: {result.Message.Value}");

                    await _handler(message ?? throw new InvalidOperationException("Mensagem inválida"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[KafkaConsumer] Erro ao processar mensagem: {ex.Message}");
                }
        });
    }
}