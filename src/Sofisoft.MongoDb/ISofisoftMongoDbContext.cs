using MongoDB.Driver;

namespace Sofisoft.MongoDb
{
    public interface ISofisoftMongoDbContext
    {
        IMongoDatabase Database { get; }
        bool HasActiveTransaction { get; }

        Task<IClientSessionHandle?> BeginSessionAsync();
        IClientSessionHandle? GetCurrentSession();
    }
}