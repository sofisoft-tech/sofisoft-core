using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Attributes;
using Sofisoft.MongoDb.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;

namespace Sofisoft.MongoDb.Tests.Repositories
{
    public class CommandRepositoryTest
    {
        private readonly Mock<ISofisoftDbContext<IClientSessionHandle>> _contextMock;

        public CommandRepositoryTest()
        {
            _contextMock = new Mock<ISofisoftDbContext<IClientSessionHandle>>();
        }

        [Fact]
        public void Constructor_throws_an_exception_for_null_context()
        {
            // Arrange
            var context = (SofisoftMongoDbContext) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new CommandRepository<BaseEntity>(context));

            // Assert
            Assert.Equal("context", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteByIdAsync_verified_call_deleted(bool hasActiveTransaction)
        {
            // Arrange
            var list = new List<FakeDocument> { new FakeDocument() };
            var session = new Mock<IClientSessionHandle>().Object;
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var cancellationToken = new CancellationTokenSource().Token;

            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new CommandRepository<FakeDocument>(_contextMock.Object);
            await repository.DeleteByIdAsync("1", cancellationToken);

            // Assert
            if (hasActiveTransaction)
            {
                collectionMock.Verify(x => x.FindOneAndDeleteAsync(
                    session,
                    It.IsAny<FilterDefinition<FakeDocument>>(),
                    It.IsAny<FindOneAndDeleteOptions<FakeDocument>>(),
                    cancellationToken), Times.Once);
            }
            else
            {
                collectionMock.Verify(x => x.FindOneAndDeleteAsync(
                    It.IsAny<FilterDefinition<FakeDocument>>(),
                    It.IsAny<FindOneAndDeleteOptions<FakeDocument>>(),
                    cancellationToken), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InsertOneAsync_verified_call_inserted_one(bool hasActiveTransaction)
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var document = new FakeDocument();
            var session = new Mock<IClientSessionHandle>().Object;
            
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new CommandRepository<FakeDocument>(_contextMock.Object);

            await repository.InsertOneAsync(document, cancellationToken);

            // Assert
            if (hasActiveTransaction)
            {
                collectionMock.Verify(x => x.InsertOneAsync(
                    session,
                    document,
                    It.IsAny<InsertOneOptions>(),
                    cancellationToken
                ), Times.Once);
            }
            else
            {
                collectionMock.Verify(x => x.InsertOneAsync(
                    document,
                    It.IsAny<InsertOneOptions>(),
                    cancellationToken
                ), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InsertManyAsync_verified_call_inserted_many(bool hasActiveTransaction)
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var documents = new List<FakeDocument> { new FakeDocument() };
            var session = new Mock<IClientSessionHandle>().Object;
            
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new CommandRepository<FakeDocument>(_contextMock.Object);

            await repository.InsertManyAsync(documents, cancellationToken);

            // Assert
            if (hasActiveTransaction)
            {
                collectionMock.Verify(x => x.InsertManyAsync(
                    session,
                    documents,
                    It.IsAny<InsertManyOptions>(),
                    cancellationToken
                ), Times.Once);
            }
            else
            {
                collectionMock.Verify(x => x.InsertManyAsync(
                    documents,
                    It.IsAny<InsertManyOptions>(),
                    cancellationToken
                ), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateOneAsync_return_modified_count(bool hasActiveTransaction)
        {
            // Arrange
            var expectedResult = 1;
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var session = new Mock<IClientSessionHandle>().Object;
            var updateResultMock = new Mock<UpdateResult>();

            updateResultMock.Setup(x => x.ModifiedCount).Returns(expectedResult);

            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            if (hasActiveTransaction)
            {
                collectionMock.Setup(x => x.UpdateOne(
                    session,
                    It.IsAny<FilterDefinition<FakeDocument>>(),
                    It.IsAny<UpdateDefinition<FakeDocument>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                    .Returns(updateResultMock.Object);
            }
            else
            {
                collectionMock.Setup(x => x.UpdateOne(
                    It.IsAny<FilterDefinition<FakeDocument>>(),
                    It.IsAny<UpdateDefinition<FakeDocument>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                    .Returns(updateResultMock.Object);
            }

            // Act
            var repository = new CommandRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.UpdateOneAsync(new FakeDocument(), It.IsAny<CancellationToken>());

            // Assert
            Assert.Equal(expectedResult, result);
        }


        [BsonCollection("fakeDocument")]
        public class FakeDocument : BaseEntity { }

        public class FakeResult
        {
            public string? Id { get; set; }
        }

    }
}