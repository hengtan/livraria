namespace Livraria.Shared;

public interface IConsumer<T>
{
    Task ConsumeAsync(T message);
}