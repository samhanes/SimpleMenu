using System;

namespace SimpleMenu.Domain
{
    public class ProductId : GuidAggregateRootEntityId
    {
        public ProductId(Guid id) : base(id)
        {
        }
    }
}