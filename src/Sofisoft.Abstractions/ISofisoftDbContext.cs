namespace Sofisoft.Abstractions
{
    public interface ISofisoftDbContext<TContextTransaction>
        where TContextTransaction : class
    {
        bool HasActiveTransaction { get; }
        Task<TContextTransaction?> BeginTransactionAsync();
        Task CommitTransactionAsync(TContextTransaction? transaction);
        TContextTransaction? GetCurrentTransaction();
        TDatabase GetDatabase<TDatabase>();
    }
}