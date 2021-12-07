using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sofisoft.Abstractions;

namespace Sofisoft.MongoDb
{
    [ExcludeFromCodeCoverage]
    public class SofisoftMongoDbContext : ISofisoftDbContext<IClientSessionHandle>
    {
        #region Variables

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private IClientSessionHandle? _currentSession;

        #endregion

        #region Constructors

        public SofisoftMongoDbContext(IOptionsMonitor<SofisoftMongoDbOptions> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _client = new MongoClient(options.CurrentValue.ConnectionString);
            _database = _client.GetDatabase(options.CurrentValue.Database);
        }

        #endregion

        #region Implements

        public bool HasActiveTransaction => _currentSession != null && _currentSession.IsInTransaction;

        public async Task<IClientSessionHandle?> BeginTransactionAsync()
        {
            if (_currentSession is not null)
            {
                return null;
            }

            _currentSession = await _client.StartSessionAsync();
            _currentSession.StartTransaction();

            return _currentSession;
        }

        public async Task CommitTransactionAsync(IClientSessionHandle? transaction)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != _currentSession)
            {
                throw new InvalidOperationException($"Session {transaction.ServerSession.Id} is not current");
            }

            await transaction.CommitTransactionAsync();
        }

        public IClientSessionHandle? GetCurrentTransaction() => _currentSession;

        public TDatabase GetDatabase<TDatabase>()
        {
            return (TDatabase) _database;
        }

        #endregion
    }
}