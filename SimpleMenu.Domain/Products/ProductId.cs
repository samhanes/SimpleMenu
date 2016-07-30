using System;
using SimpleMenu.AggregateSource;

namespace SimpleMenu.Domain.Products
{
    public class ProductId : GuidAggregateRootEntityId
    {
        public ProductId(Guid id) : base(id)
        {
        }
    }
}