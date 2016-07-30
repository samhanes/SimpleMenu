using System;
using System.Collections.Generic;
using SimpleMenu.AggregateSource;

namespace SimpleMenu.Domain
{
    public class Product : AggregateRootEntity
    {
        private Product()
        {
            Register<ProductCreated>(Apply);
            Register<ProductNameChanged>(Apply);
            Register<ProductCategoryChanged>(Apply);
        }

        public Product(IEnumerable<object> events) : this()
        {
            Initialize(events);
        }

        public override AggregateRootEntityId AggregateRootId => Id;
        public ProductId Id { get; private set; }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public DateTime CreatedDateTime { get; private set; }

        public static Product Create(ProductId id, string name, string category)
        {
            return new Product(new[] {new ProductCreated(id, name, category, DateTime.UtcNow)});
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Product name cannot be null or whitespace.");
            ApplyChange(new ProductNameChanged(Id, newName));
        }

        public void UpdateCategory(string newCategory)
        {
            if (string.IsNullOrWhiteSpace(newCategory))
                throw new ArgumentException("Product category cannot be null or whitespace.");
            ApplyChange(new ProductCategoryChanged(Id, newCategory));
        }

        private void Apply(ProductCreated ev)
        {
            Id = ev.Id;
            Name = ev.Name;
            Category = ev.Category;
            CreatedDateTime = ev.DateTimeCreated;
        }

        private void Apply(ProductNameChanged ev)
        {
            Name = ev.NewName;
        }

        private void Apply(ProductCategoryChanged ev)
        {
            Category = ev.NewCategory;
        }
    }
}
