using System;
using System.Collections.Generic;

namespace SimpleMenu.AggregateSource
{
    internal class EventRouter : IEventRouter
    {
        private readonly Dictionary<Type, Action<object>> _handlers;

        public EventRouter()
        {
            _handlers = new Dictionary<Type, Action<object>>();
        }

        public void ConfigureRoute(Type @event, Action<object> handler)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handlers.Add(@event, handler);
        }

        public void ConfigureRoute<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public void Route(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            Action<object> handler;
            if (_handlers.TryGetValue(@event.GetType(), out handler))
            {
                handler(@event);
            }
        }
    }
}