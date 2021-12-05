using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Sofisoft.MongoDb
{
    [ExcludeFromCodeCoverage]
    public class SofisoftMongoDbContext : ISofisoftMongoDbContext
    {
        #region Variables

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private IClientSessionHandle? _currentSession;

        #endregion

        #region Constructors

        public SofisoftMongoDbContext(IOptionsMonitor<SofisoftMongoDbOptions> options)
        {
            _client = new MongoClient(options.CurrentValue.ConnectionString);
            _database = _client.GetDatabase(options.CurrentValue.Database);
        }

        #endregion

        #region Methods

        public async Task<IClientSessionHandle?> BeginSessionAsync()
        {
            if (_currentSession is not null)
            {
                return null;
            }

            _currentSession = await _client.StartSessionAsync();

            return _currentSession;
        }

        public IClientSessionHandle? GetCurrentSession() => _currentSession;

        #endregion

        #region Properties

        public IMongoDatabase Database => _database;
        public bool HasActiveTransaction => _currentSession != null && _currentSession.IsInTransaction;

        #endregion
    }
}