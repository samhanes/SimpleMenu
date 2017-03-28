using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleMenu.AggregateSource;

namespace SimpleMenu.App
{
    public class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<IEventHandler>> _handlers = new Dictionary<Type, List<IEventHandler>>();

        public void Register<TEvent>(IEventHandler<TEvent> handler)
        {
            var eventTypes = Assembly.GetAssembly(typeof(TEvent)).GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof (TEvent).IsAssignableFrom(t));

            foreach (var eventType in eventTypes)
                RegisterHandler(eventType, handler);
        }

        private void RegisterHandler(Type eventType, IEventHandler handler)
        {
            if (!_handlers.ContainsKey(eventType))
                _handlers.Add(eventType, new List<IEventHandler>());

            _handlers[eventType].Add(handler);
        }

        public void Publish(object ev)
        {
            var eventType = ev.GetType();
            if (!_handlers.ContainsKey(eventType))
                return;

            foreach (var handler in _handlers[eventType])
            {
                handler.Handle(ev);
            }
        }
    }
}