namespace Infrastructure.Interfaces
{
    public interface IPublisher
    {
        void Send<T>(T message);
    }
}