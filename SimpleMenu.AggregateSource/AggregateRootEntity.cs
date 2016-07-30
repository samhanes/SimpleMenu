using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMenu.AggregateSource
{
    public abstract class AggregateRootEntity
    {
        private readonly EventRecorder _recorder;
        private readonly EventRouter _router;

        public abstract AggregateRootEntityId AggregateRootId { get; }

        protected AggregateRootEntity()
        {
            _router = new EventRouter();
            _recorder = new EventRecorder();
        }

        protected void Initialize(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            if (HasChanges())
                throw new InvalidOperationException("Initialize cannot be called on an instance with changes.");

            foreach (var @event in events)
            {
                Play(@event);
            }
        }

        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _router.ConfigureRoute(handler);
        }
        
        protected void ApplyChange(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            BeforeApplyChange(@event);
            Play(@event);
            Record(@event);
            AfterApplyChange(@event);
        }

        protected virtual void BeforeApplyChange(object @event) { }
        protected virtual void AfterApplyChange(object @event) { }

        private void Play(object @event)
        {
            _router.Route(@event);
        }

        private void Record(object @event)
        {
            _recorder.Record(@event);
        }

        public bool HasChanges()
        {
            return _recorder.Any();
        }

        public IEnumerable<object> GetChanges()
        {
            return _recorder.ToArray();
        }

        public void ClearChanges()
        {
            _recorder.Reset();
        }
    }
}
