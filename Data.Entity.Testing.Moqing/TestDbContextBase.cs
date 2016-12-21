namespace PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity.Testing.Moqing
{
    using UnitTesting.TestDoubles;
    using DataAccess.Contracts;

    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Moq;

    public class TestDbContextBase<TDto, TId> : IDisposable
                                   where TDto : IdDtoBase<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public Mock<DbSet<TDto>> MockDbSet { get; }
        public Mock<DbContext> MockDbContext { get; }
        public DalBase<TDto, TId> DbContextBase { get; }
        private DbAsyncEnumerator<TDto> DbAsyncEnumerator { get; }

        public TestDbContextBase(IQueryable<TDto> testDtoObjects)
        {
            if (testDtoObjects == null) throw new ArgumentNullException(nameof(testDtoObjects));

            DbAsyncEnumerator = new DbAsyncEnumerator<TDto>(testDtoObjects.GetEnumerator());
            MockDbSet = new Mock<DbSet<TDto>>();
            MockDbSet.As<IDbAsyncEnumerable<TDto>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(DbAsyncEnumerator);

            MockDbSet.As<IQueryable<TDto>>()
                .Setup(m => m.Provider)
                .Returns(new DbAsyncQueryProvider<TDto>(testDtoObjects.Provider));

            MockDbSet.As<IQueryable<TDto>>().Setup(m => m.Expression).Returns(testDtoObjects.Expression);
            MockDbSet.As<IQueryable<TDto>>().Setup(m => m.ElementType).Returns(testDtoObjects.ElementType);
            MockDbSet.As<IQueryable<TDto>>().Setup(m => m.GetEnumerator()).Returns(testDtoObjects.GetEnumerator());

            MockDbContext = new Mock<DbContext>();
            MockDbContext.Setup(m => m.Set<TDto>()).Returns(MockDbSet.Object);
            DbContextBase = new DalBase<TDto, TId>(MockDbContext.Object);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    MockDbContext.Object.Dispose();
                    DbAsyncEnumerator.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
