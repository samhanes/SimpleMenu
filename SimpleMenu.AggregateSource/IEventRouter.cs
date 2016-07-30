namespace SimpleMenu.AggregateSource
{
    public interface IEventRouter
    {
        void Route(object @event);
    }
}