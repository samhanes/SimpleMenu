using System;

namespace SimpleMenu.AggregateSource
{
    public abstract class AggregateRootEntityId
    {
        public abstract Guid ToGuid();
    }
}