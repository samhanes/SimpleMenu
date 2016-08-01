using System.Linq;
using SimpleMenu.Domain.Products.Events;

namespace SimpleMenu.App
{
    public class ProductInfoHandler : IEventHandler<ProductEvent>
    {
        public void Handle(object ev)
        {
            if (ev is ProductEvent)
                Handle((dynamic) ev);
        }

        private static void Handle(ProductCreated ev)
        {
            var model = new ProductInfo(ev.Id, ev.Name, ev.Category);
            ReadModelStore.ProductInfo.Add(model);
        }
        
        private static void Handle(ProductNameChanged ev)
        {
            var old = ReadModelStore.ProductInfo.Single(prod => prod.Id == ev.Id);
            var newModel = new ProductInfo(ev.Id, ev.NewName, old.Category);
            
            UpdateProductInfo(old, newModel);
        }
        
        private static void Handle(ProductCategoryChanged ev)
        {
            var old = ReadModelStore.ProductInfo.Single(prod => prod.Id == ev.Id);
            var newModel = new ProductInfo(ev.Id, old.Name, ev.NewCategory);

            UpdateProductInfo(old, newModel);
        }

        private static void UpdateProductInfo(ProductInfo old, ProductInfo newModel)
        {
            var index = ReadModelStore.ProductInfo.FindIndex(pi => pi == old);
            ReadModelStore.ProductInfo[index] = newModel;
        }
    }
}