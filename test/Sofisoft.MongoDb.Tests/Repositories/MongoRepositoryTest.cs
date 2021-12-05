using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Sofisoft.MongoDb.Attributes;
using Sofisoft.MongoDb.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;
using static Sofisoft.MongoDb.Tests.MongoRepositoryTest;

namespace Sofisoft.MongoDb.Tests
{
    public class MongoRepositoryTest
    {
        private readonly Mock<ISofisoftMongoDbContext> _contextMock;

        public MongoRepositoryTest()
        {
            _contextMock = new Mock<ISofisoftMongoDbContext>();
        }

        [Fact]
        public void Constructor_throws_an_exception_for_null_context()
        {
            // Arrange
            var context = (SofisoftMongoDbContext) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new MongoRepository<Document>(context));

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
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.AggregateAsync(It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task CountAsync_return_count(bool hasActiveTransaction, bool withFilterExpresion)
        {
            // Arrange
            var expectedResult = 2L;
            var cancellationToken = new CancellationTokenSource().Token;
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            Expression<Func<FakeDocument, bool>> filterExpression = p => p.Id == "1";
            var session = new Mock<IClientSessionHandle>().Object;
            
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            if (hasActiveTransaction)
            {
                if (withFilterExpresion)
                {
                    collectionMock.Setup(x => x.CountDocumentsAsync(session, It.IsAny<ExpressionFilterDefinition<FakeDocument>>(), It.IsAny<CountOptions>(), cancellationToken))
                        .ReturnsAsync(expectedResult);
                }
                else
                {
                    collectionMock.Setup(x => x.CountDocumentsAsync(session, FilterDefinition<FakeDocument>.Empty, It.IsAny<CountOptions>(), cancellationToken))
                        .ReturnsAsync(expectedResult);
                }
            }
            else
            {
                if (withFilterExpresion)
                {
                    collectionMock.Setup(x => x.CountDocumentsAsync(It.IsAny<ExpressionFilterDefinition<FakeDocument>>(), It.IsAny<CountOptions>(), cancellationToken))
                        .ReturnsAsync(expectedResult);
                }
                else
                {
                    collectionMock.Setup(x => x.CountDocumentsAsync(FilterDefinition<FakeDocument>.Empty, It.IsAny<CountOptions>(), cancellationToken))
                        .ReturnsAsync(expectedResult);
                }
            }

            // Act
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.CountAsync(withFilterExpresion ? filterExpression : It.IsAny<Expression<Func<FakeDocument, bool>>>(), cancellationToken);

            // Assert
            Assert.Equal(expectedResult, result);
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
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);
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
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task FilterByAsync_return_list(bool hasActiveTransaction, bool withFilterExpresion)
        {
            // Arrange
            var expectedResult = new List<FakeDocument>() { new FakeDocument() };
            var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
            var cursorMock = new Mock<IAsyncCursor<FakeDocument>>();
            var session = new Mock<IClientSessionHandle>().Object;
            Expression<Func<FakeDocument, bool>> filterExpression = p => p.Id == "1";
            
            cursorMock.SetupSequence(x => x.MoveNext(default))
                .Returns(true)
                .Returns(false);
            cursorMock.SetupGet(x => x.Current).Returns(expectedResult);
            _contextMock.Setup(x => x.HasActiveTransaction)
                .Returns(hasActiveTransaction);
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            if (hasActiveTransaction)
            {
                if (withFilterExpresion)
                {
                    collectionMock.Setup(x => x.FindSync(session,
                        It.IsAny<ExpressionFilterDefinition<FakeDocument>>(),
                        It.IsAny<FindOptions<FakeDocument>>(),
                        default))
                        .Returns(cursorMock.Object);
                }
                else
                {
                    collectionMock.Setup(x => x.FindSync(session,
                        FilterDefinition<FakeDocument>.Empty,
                        It.IsAny<FindOptions<FakeDocument>>(),
                        default))
                        .Returns(cursorMock.Object);
                }
            }
            else
            {
                if (withFilterExpresion)
                {
                    collectionMock.Setup(x => x.FindSync(It.IsAny<ExpressionFilterDefinition<FakeDocument>>(),
                        It.IsAny<FindOptions<FakeDocument>>(),
                        default))
                        .Returns(cursorMock.Object);
                }
                else
                {
                    collectionMock.Setup(x => x.FindSync(FilterDefinition<FakeDocument>.Empty,
                        It.IsAny<FindOptions<FakeDocument>>(),
                        default))
                        .Returns(cursorMock.Object);
                }
            }

            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);

            // Act
            var result = await repository.FilterByAsync(
                withFilterExpresion ? filterExpression : It.IsAny<Expression<Func<FakeDocument, bool>>>(),
                default);

            // Assert
            Assert.Equal(expectedResult, result);
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
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);

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
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
                .Returns(collectionMock.Object);

            // Act
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);

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
            _contextMock.Setup(x => x.GetCurrentSession())
                .Returns(session);
            _contextMock.Setup(x => x.Database.GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
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
            var repository = new MongoRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.UpdateOneAsync(new FakeDocument(), It.IsAny<CancellationToken>());

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

    public interface IFakeMongoCollection : IMongoCollection<FakeDocument>
    {
        IFindFluent<FakeDocument, FakeDocument> Find(FilterDefinition<FakeDocument> filter, FindOptions options);
    
        IFindFluent<FakeDocument, FakeDocument> Project(ProjectionDefinition<FakeDocument, FakeResult> projection);
    }
}