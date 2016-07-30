using System;
using System.Net.Http.Headers;
using System.Transactions;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using SimpleMenu.AggregateSource;
using SimpleMenu.Domain;
using SimpleMenu.Domain.Products;

namespace SimpleMenu.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var scope = new TransactionScope())
            using (var store = WireupEventStore())
            {
                var repository = new Repository(store);
                var productId = new ProductId(Guid.NewGuid());
                var product = new Product(productId, "Pizza", "Foods");
                repository.Save(product);

                product = repository.Get<Product>(productId);
                product.UpdateName("Pizznizza");
                repository.Save(product);

                scope.Complete();
            }
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
