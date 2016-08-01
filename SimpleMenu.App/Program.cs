using System;
using System.Linq;
using System.Transactions;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using SimpleMenu.Domain.Products;

namespace SimpleMenu.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SimpleEventBus.Register(new ProductInfoHandler());
            var productId = new ProductId(Guid.NewGuid());

            using (var scope = new TransactionScope())
            using (var store = WireupEventStore())
            {
                var bus = new SimpleEventBus();
                var repository = new Repository(store, bus);

                var product = new Product(productId, "Piza", "Foods");
                repository.Save(product);

                product = repository.Get<Product>(productId);
                product.UpdateName("Pizza");
                repository.Save(product);

                scope.Complete();
            }

            var model = ReadModelStore.ProductInfo.First(pi => pi.Id == productId);
        }

        private static IStoreEvents WireupEventStore()
        {
            return Wireup.Init()
                         .LogToOutputWindow()
                         .UsingInMemoryPersistence()
                         .UsingSqlPersistence("EventStore") // Connection string is in app.config
                         .WithDialect(new MsSqlDialect())
                         .EnlistInAmbientTransaction() // two-phase commit
                         .InitializeStorageEngine()
                         .TrackPerformanceInstance("example")
                         .UsingJsonSerialization()
                         .Compress()
                         //.EncryptWith(EncryptionKey)
                         //.HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
                         //.DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
                         .Build();
        }
    }
}
