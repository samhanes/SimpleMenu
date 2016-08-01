namespace SimpleMenu.AggregateSource
{
    public interface IEventBus
    {
        void Publish(object ev);
    }
}