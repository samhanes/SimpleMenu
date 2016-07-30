namespace SimpleMenu.Domain
{
    public class ProductNameChanged
    {
        public ProductNameChanged(ProductId id, string newName)
        {
            Id = id;
            NewName = newName;
        }

        public ProductId Id { get; }
        public string NewName { get; }
    }
}