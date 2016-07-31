using System;

namespace SimpleMenu.AggregateSource
{
    /// <summary>
    /// Base class for aggregate entities that need some basic infrastructure for tracking state changes on their aggregate root entity.
    /// </summary>
    public abstract class Entity : IEventRouter
    {
        private readonly Action<object> _applier;
        private readonly EventRouter _router;

        /// <summary>Initializes a new instance of the <see cref="Entity"/> class.</summary>
        /// <param name="applier">The event's apply handler from the aggregate root.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="applier"/> is null.</exception>
        protected Entity(Action<object> applier)
        {
            if (applier == null) throw new ArgumentNullException(nameof(applier));
            _applier = applier;
            _router = new EventRouter();
        }

        /// <summary>Registers the state handler to be invoked when the specified event is applied.</summary>
        /// <typeparam name="TEvent">The type of the event to register the handler for.</typeparam>
        /// <param name="handler">The state handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="handler"/> is null.</exception>
        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _router.ConfigureRoute(handler);
        }

        /// <summary>Routes the specified <paramref name="event"/> to a configured state handler, if any.</summary>
        /// <param name="event">The event to route.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="event"/> is null.</exception>
        public void Route(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _router.Route(@event);
        }

        /// <summary>Invokes the associated state handler in the aggregate root entity.</summary>
        /// <param name="event">The event to apply.</param>
        protected void Apply(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _applier(@event);
        }
    }
}