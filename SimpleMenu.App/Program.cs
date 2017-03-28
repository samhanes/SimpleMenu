using System;
using System.Linq;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using SimpleMenu.Domain.Products;

namespace SimpleMenu.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bus = new SimpleEventBus();
            bus.Register(new ProductInfoHandler());
            var productId = new ProductId(Guid.NewGuid());

            using (var store = WireupEventStore())
            {
                var repository = new Repository(store, bus);
                var product = new Product(productId, "Piza", "Foods");
                repository.Save(product);

                Console.WriteLine("Product saved. (1)");
                Console.ReadKey();

                product = repository.Get<Product>(productId);
                product.UpdateName("Pizza");
                repository.Save(product);

                Console.WriteLine("Product saved. (2)");
                Console.ReadKey();
            }

            var model = ReadModelStore.ProductInfo.First(pi => pi.Id == productId);
            Console.WriteLine($"Read model: Id = {model.Id.Id}, DisplayName = {model.DisplayName}");
            Console.ReadKey();
        }

        private static IStoreEvents WireupEventStore()
        {
            return Wireup.Init()
                         .LogToOutputWindow()
                         //.UsingInMemoryPersistence()
                         .UsingSqlPersistence("EventStore") // Connection string is in app.config
                         .WithDialect(new MsSqlDialect())
                         .InitializeStorageEngine()
                         .TrackPerformanceInstance("example")
                         .UsingJsonSerialization()
                         .Compress()
                         .Build();
        }
    }
}
