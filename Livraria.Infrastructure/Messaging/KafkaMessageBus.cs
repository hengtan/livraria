using System.Text.Json;
using Confluent.Kafka;
using Livraria.Shared;

namespace Livraria.Infrastructure.Messaging;

public class KafkaMessageBus : IMessageBus
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IProducer<Null, string> _producer;

    public KafkaMessageBus(string bootstrapServers)
    {
        // Configuração do produtor
        var producerConfig = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

        // Configuração do consumidor
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "livraria-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    public async Task PublishAsync<T>(string topic, T message)
    {
        var messageValue = JsonSerializer.Serialize(message);

        Console.WriteLine($"[KafkaMessageBus] Publicando mensagem no tópico '{topic}': {messageValue}");
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = messageValue });
    }

    public async Task SubscribeAsync<T>(string topic, Func<T, Task> handler)
    {
        // Cria um consumidor para escutar mensagens
        var consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();
        consumer.Subscribe(topic);

        Console.WriteLine($"[KafkaMessageBus] Inscrito no tópico '{topic}'");

        // Loop para consumir mensagens
        await Task.Run(() =>
        {
            while (true)
                try
                {
                    var result = consumer.Consume();
                    Console.WriteLine(
                        $"[KafkaMessageBus] Mensagem recebida no tópico '{topic}': {result.Message.Value}");

                    var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                    if (message != null) handler(message).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[KafkaMessageBus] Erro ao processar mensagem: {ex.Message}");
                }
        });
    }
}