using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleMenu.AggregateSource
{
    public class EventRecorder : IEnumerable<object>
    {
        private readonly List<object> _recorded;

        public EventRecorder()
        {
            _recorded = new List<object>();
        }

        public void Record(object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            _recorded.Add(@event);
        }
        
        public void Reset()
        {
            _recorded.Clear();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _recorded.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}