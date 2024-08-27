using Core.Test;
using Core.Test.Dummy.EF;

namespace Core.Infrastructure.EF.DbContext.Tests
{
    [TestFixture]
    public class BaseDbContextTests
    {
        private DbContextOptions<DummyDbContext> _options;
        private DummyDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DummyDbContext>()
                .UseInMemoryDatabase(databaseName: "BaseDbContextTest")
                .Options;

            _dbContext = new DummyDbContext(_options);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task SaveChangeAsync_ShouldSaveChangesToDbContext()
        {
            // Arrange
            var entity = new DummyAgreegateRoot(new DummyAggregateId(Guid.NewGuid()));
            _dbContext.DummyAgreegateRoots.Add(entity);

            // Act
            await _dbContext.SaveChangeAsync();

            // Assert
            Assert.That(await _dbContext.DummyAgreegateRoots.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task SaveChangeAsync_ShouldReturnNumberOfSavedEntities()
        {
            // Arrange
            var entity = new DummyAgreegateRoot(new DummyAggregateId(Guid.NewGuid()));
            var other = new DummyAgreegateRoot(new DummyAggregateId(Guid.NewGuid()));
            _dbContext.DummyAgreegateRoots.Add(entity);
            _dbContext.DummyAgreegateRoots.Add(other);

            // Act
            var result = await _dbContext.SaveChangeAsync();

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public Task SaveChangeAsync_ShouldThrowExceptionIfDbContextIsDisposed()
        {
            // Arrange
            _dbContext.Dispose();

            // Act & Assert
            Assert.ThrowsAsync<ObjectDisposedException>(async () => await _dbContext.SaveChangeAsync());
            return Task.CompletedTask;
        }
    }
}