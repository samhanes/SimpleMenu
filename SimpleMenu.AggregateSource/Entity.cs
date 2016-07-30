using System;

namespace SimpleMenu.AggregateSource
{
    public abstract class Entity : IEventRouter
    {
        private readonly Action<object> _applier;
        private readonly EventRouter _router;

        protected Entity(Action<object> applier)
        {
            if (applier == null) throw new ArgumentNullException(nameof(applier));
            _applier = applier;
            _router = new EventRouter();
        }

        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _router.ConfigureRoute(handler);
        }
        
        public void Route(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _router.Route(@event);
        }
        
        protected void Apply(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _applier(@event);
        }
    }
}