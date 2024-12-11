namespace Livraria.Shared;

public interface IMessageBus
{
    // Publica uma mensagem em um tópico
    Task PublishAsync<T>(string topic, T message);

    // Inscreve-se em um tópico e define um handler para processar mensagens recebidas
    Task SubscribeAsync<T>(string topic, Func<T, Task> handler);
}