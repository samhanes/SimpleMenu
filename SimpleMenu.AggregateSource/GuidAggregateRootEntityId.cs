using System;

namespace SimpleMenu.AggregateSource
{
    public abstract class GuidAggregateRootEntityId : AggregateRootEntityId, IEquatable<GuidAggregateRootEntityId>
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

        public bool Equals(GuidAggregateRootEntityId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GuidAggregateRootEntityId) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(GuidAggregateRootEntityId left, GuidAggregateRootEntityId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GuidAggregateRootEntityId left, GuidAggregateRootEntityId right)
        {
            return !Equals(left, right);
        }
    }
}