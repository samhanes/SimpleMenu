namespace SimpleMenu.App
{
    public interface IEventHandler
    {
        void Handle(object ev);
    }

    public interface IEventHandler<in TEvent> : IEventHandler
    { 
    }
}