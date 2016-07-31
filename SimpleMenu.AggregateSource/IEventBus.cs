using System.Collections.Generic;

namespace SimpleMenu.AggregateSource
{
    public interface IEventBus
    {
        void Publish(IEnumerable<object> events);
    }
}