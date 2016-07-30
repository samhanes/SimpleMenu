using System;
using System.Linq;
using NEventStore;

namespace SimpleMenu.AggregateSource
{
    public class Repository : IRepository
    {
        private readonly IStoreEvents _eventStore;

        public Repository(IStoreEvents eventStore)
        {
            if (eventStore == null) throw new ArgumentNullException(nameof(eventStore));
            _eventStore = eventStore;
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
            using (var stream = _eventStore.OpenStream(root.Id.ToGuid()))
            {
                foreach (var ev in root.GetChanges())
                {
                    stream.Add(new EventMessage { Body = ev });
                }
                stream.CommitChanges(Guid.NewGuid());
                root.ClearChanges();
            }
        }
    }

    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(AggregateRootEntityId identifier, Type type)
            : base($"Aggregate of type '{type.Name}' with id '{identifier}' not found.")
        {
        }
    }
}
