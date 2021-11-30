using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Sofisoft.MongoDb.Attributes;
using Sofisoft.MongoDb.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;

namespace Sofisoft.MongoDb.Tests
{
    public class MongoDbRepositoryTest
    {
        private readonly Mock<ISofisoftMongoDbContext> _contextMock;

        public MongoDbRepositoryTest()
        {
            _contextMock = new Mock<ISofisoftMongoDbContext>();
        }

        [Fact]
        public void Constructor_throws_an_exception_for_null_context()
        {
            // Arrange
            var context = (SofisoftMongoDbContext) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new MongoDbRepository<Document>(context));

            // Assert
            Assert.Equal("context", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task AggregateAsync_return_list(bool hasActiveTransaction)
        {
            // Arrange
            var expectedResult = new List<FakeResult> { new FakeResult { Id = "my-id" } };
            var clientSessionMock = new Mock<IClientSessionHandle>();
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var asyncCursor = new Mock<IAsyncCursor<FakeResult>>();

            collectionMock.Setup(x => x.Aggregate(It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
                .Returns(asyncCursor.Object);
            collectionMock.Setup(x => x.Aggregate(It.IsAny<IClientSessionHandle>(), It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
                .Returns(asyncCursor.Object);
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);
            
            asyncCursor.SetupSequence(x => x.MoveNext(default)).Returns(true)
                .Returns(false);
            asyncCursor.SetupGet(x => x.Current)
                .Returns(expectedResult);

            // Act
            var repository = new MongoDbRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.AggregateAsync(It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CountAsync_return_count(bool hasActiveTransaction)
        {
            // Arrange
            var expectedResult = 1L;
            var clientSessionMock = new Mock<IClientSessionHandle>();
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();

            collectionMock.Setup(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<FakeDocument>>(), It.IsAny<CountOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);
            collectionMock.Setup(x => x.CountDocumentsAsync(It.IsAny<IClientSessionHandle>(), It.IsAny<FilterDefinition<FakeDocument>>(), It.IsAny<CountOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new MongoDbRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.CountAsync(p => true, It.IsAny<CancellationToken>());

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [BsonCollection("fakeDocument")]
        public class FakeDocument : Document { }

        public class FakeResult
        {
            public string? Id { get; set; }
        }

    }
}