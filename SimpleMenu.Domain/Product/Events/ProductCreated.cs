using System;

namespace SimpleMenu.Domain
{
    public class ProductCreated
    {
        public ProductCreated(ProductId id, string name, string category, DateTime dateTimeCreated)
        {
            Id = id;
            Name = name;
            Category = category;
            DateTimeCreated = dateTimeCreated;
        }

        public ProductId Id { get; }
        public string Name { get; }
        public string Category { get; }
        public DateTime DateTimeCreated { get; }
    }
}