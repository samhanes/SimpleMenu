using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleMenu.AggregateSource;

namespace SimpleMenu.App
{
    public class SimpleEventBus : IEventBus
    {
        private static readonly Dictionary<Type, List<IEventHandler>> Handlers = new Dictionary<Type, List<IEventHandler>>();

        public static void Register<TEvent>(IEventHandler<TEvent> handler)
        {
            var eventTypes = Assembly.GetAssembly(typeof(TEvent)).GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof (TEvent).IsAssignableFrom(t));

            foreach (var eventType in eventTypes)
                RegisterHandler(eventType, handler);
        }

        private static void RegisterHandler(Type eventType, IEventHandler handler)
        {
            if (!Handlers.ContainsKey(eventType))
                Handlers.Add(eventType, new List<IEventHandler>());

            Handlers[eventType].Add(handler);
        }

        public void Publish(object ev)
        {
            var eventType = ev.GetType();
            if (!Handlers.ContainsKey(eventType))
                return;

            foreach (var handler in Handlers[eventType])
            {
                handler.Handle(ev);
            }
        }
    }
}