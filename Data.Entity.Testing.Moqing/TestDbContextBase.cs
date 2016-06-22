namespace PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity.Testing.Moqing
{
    using UnitTesting.TestDoubles;
    using DataAccess;
    using DataAccess.Contracts;

    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Moq;

    public class TestDbContextBase<TDto, TId> where TDto : IdDtoBase<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is utilizing the recommended approach based on the Entity Framework documentation: https://msdn.microsoft.com/en-us/data/dn314429")]
        public Mock<DbSet<TDto>> MockDbSet { get; }
        public Mock<DbContext> MockDbContext { get; }
        public DalBase<TDto, TId> DbContextBase { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is utilizing the recommended approach based on the Entity Framework documentation: https://msdn.microsoft.com/en-us/data/dn314429")]
        public TestDbContextBase(IQueryable<TDto> testDtoObjects)
        {
            if (testDtoObjects == null) throw new ArgumentNullException(nameof(testDtoObjects));

            MockDbSet = new Mock<DbSet<TDto>>();
            MockDbSet.As<IDbAsyncEnumerable<TDto>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new DbAsyncEnumerator<TDto>(testDtoObjects.GetEnumerator()));

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
    }
}
