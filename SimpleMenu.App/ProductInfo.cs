using SimpleMenu.Domain.Products;

namespace SimpleMenu.App
{
    public class ProductInfo
    {
        public ProductInfo(ProductId id, string name, string category)
        {
            Id = id;
            Name = name;
            Category = category;
        }

        public ProductId Id { get; }
        public string Name { get; }
        public string Category { get; }
        public string DisplayName => $"{Name} ({Category})";
    }
}