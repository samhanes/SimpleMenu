namespace SimpleMenu.Domain.Products.Events
{
    public class ProductCategoryChanged : ProductEvent
    {
        public ProductCategoryChanged(ProductId id, string newCategory)
        {
            Id = id;
            NewCategory = newCategory;
        }

        public ProductId Id { get; }
        public string NewCategory { get; }
    }
}