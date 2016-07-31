using System;

namespace SimpleMenu.AggregateSource
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(AggregateRootEntityId identifier, Type type)
            : base($"Aggregate of type '{type.Name}' with id '{identifier}' not found.")
        {
        }
    }
}