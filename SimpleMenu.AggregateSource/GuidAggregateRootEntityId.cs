using System;

namespace SimpleMenu.AggregateSource
{
    public abstract class GuidAggregateRootEntityId : AggregateRootEntityId
    {
        protected GuidAggregateRootEntityId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public override Guid ToGuid()
        {
            return Id;
        }
    }
}