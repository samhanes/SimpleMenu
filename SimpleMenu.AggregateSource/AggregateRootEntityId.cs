using System;

namespace SimpleMenu.AggregateSource
{
    public abstract class AggregateRootEntityId : EntityId
    {
        public abstract Guid ToGuid();
    }
}