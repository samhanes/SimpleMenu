namespace SimpleMenu.AggregateSource.Persistence
{
    public interface IRepository
    {
        TAggregateRoot Get<TAggregateRoot>(AggregateRootEntityId identifier) where TAggregateRoot : AggregateRootEntity;
        TAggregateRoot Find<TAggregateRoot>(AggregateRootEntityId identifier) where TAggregateRoot : AggregateRootEntity;
        void Save(AggregateRootEntity root);
    }
}