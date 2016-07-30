namespace SimpleMenu.Domain
{
    public class ProductCategoryChanged
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