using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public class QueryRepositoryTest
    {
        private readonly Mock<ISofisoftDbContext<IClientSessionHandle>> _contextMock;

        public QueryRepositoryTest()
        {
            _contextMock = new Mock<ISofisoftDbContext<IClientSessionHandle>>();
        }

        [Fact]
        public void Constructor_throws_an_exception_for_null_context()
        {
            // Arrange
            var context = (SofisoftMongoDbContext) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new QueryRepository<BaseEntity>(context));

            // Assert
            Assert.Equal("context", exception.ParamName);
        }

        // [Theory]
        // [InlineData(true)]
        // [InlineData(false)]
        // public async Task AggregateAsync_return_list(bool hasActiveTransaction)
        // {
        //     // Arrange
        //     var expectedResult = new List<FakeResult> { new FakeResult { Id = "my-id" } };
        //     var clientSessionMock = new Mock<IClientSessionHandle>();
        //     var collectionMock = new Mock<IMongoCollection<FakeDocument>>();
        //     var asyncCursor = new Mock<IAsyncCursor<FakeResult>>();

        //     collectionMock.Setup(x => x.Aggregate(It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
        //         .Returns(asyncCursor.Object);
        //     collectionMock.Setup(x => x.Aggregate(It.IsAny<IClientSessionHandle>(), It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
        //         .Returns(asyncCursor.Object);
        //     _contextMock.Setup(x => x.HasActiveTransaction)
        //         .Returns(hasActiveTransaction);
        //     _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
        //         .Returns(collectionMock.Object);
            
        //     asyncCursor.SetupSequence(x => x.MoveNext(default)).Returns(true)
        //         .Returns(false);
        //     asyncCursor.SetupGet(x => x.Current)
        //         .Returns(expectedResult);

        //     // Act
        //     var repository = new MongoRepository<FakeDocument>(_contextMock.Object);
        //     var result = await repository.AggregateAsync(It.IsAny<PipelineDefinition<FakeDocument, FakeResult>>(), It.IsAny<CancellationToken>());

        //     // Assert
        //     Assert.Equal(expectedResult, result);
        // }

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
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
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
            var repository = new QueryRepository<FakeDocument>(_contextMock.Object);
            var result = await repository.CountAsync(withFilterExpresion ? filterExpression : It.IsAny<Expression<Func<FakeDocument, bool>>>(), cancellationToken);

            // Assert
            Assert.Equal(expectedResult, result);
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
            _contextMock.Setup(x => x.GetCurrentTransaction())
                .Returns(session);
            _contextMock.Setup(x => x.GetDatabase<IMongoDatabase>().GetCollection<FakeDocument>("fakeDocument", It.IsAny<MongoCollectionSettings>()))
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

            var repository = new QueryRepository<FakeDocument>(_contextMock.Object);

            // Act
            var result = await repository.FilterByAsync(
                withFilterExpresion ? filterExpression : It.IsAny<Expression<Func<FakeDocument, bool>>>(),
                default);

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