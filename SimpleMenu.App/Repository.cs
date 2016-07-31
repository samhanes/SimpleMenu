using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NEventStore;
using SimpleMenu.AggregateSource;

namespace SimpleMenu.App
{
    public class SimpleEventBus : IEventBus
    {
        public void Publish(IEnumerable<object> events)
        {
            throw new NotImplementedException();
        }
    }

    public class Repository : IRepository
    {
        private readonly IStoreEvents _eventStore;
        private readonly IEventBus _bus;

        public Repository(IStoreEvents eventStore, IEventBus bus)
        {
            if (eventStore == null) throw new ArgumentNullException(nameof(eventStore));
            _eventStore = eventStore;
            _bus = bus;
        }
        
        public TAggregateRoot Get<TAggregateRoot>(AggregateRootEntityId identifier)
            where TAggregateRoot : AggregateRootEntity
        {
            var result = Find<TAggregateRoot>(identifier);
            if (result == null)
                throw new AggregateNotFoundException(identifier, typeof(TAggregateRoot));
            return result;
        }
        
        public TAggregateRoot Find<TAggregateRoot>(AggregateRootEntityId identifier)
            where TAggregateRoot : AggregateRootEntity
        {
            using (var stream = _eventStore.OpenStream(identifier.ToGuid()))
            {
                if (stream.StreamRevision == 0)
                    return null;

                var events = stream.CommittedEvents.Select(evMsg => evMsg.Body);
                return (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), events);
            }
        }
        
        public void Save(AggregateRootEntity root)
        {
            if (!root.HasChanges())
                return;

            var eventsToSave = root.GetChanges().ToList();
            PersistEvents(root.AggregateRootId.ToGuid(), eventsToSave);
            
            _bus.Publish(eventsToSave);
            root.ClearChanges();
        }

        private void PersistEvents(Guid persistenceId, IEnumerable<object> eventsToSave)
        {
            using (var scope = new TransactionScope())
            using (var stream = _eventStore.OpenStream(persistenceId))
            {
                foreach (var ev in eventsToSave)
                {
                    stream.Add(new EventMessage { Body = ev });
                }
                stream.CommitChanges(Guid.NewGuid());
                scope.Complete();
            }
        }
    }
}
