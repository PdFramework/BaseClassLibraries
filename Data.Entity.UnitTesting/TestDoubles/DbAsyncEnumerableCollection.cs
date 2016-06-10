namespace PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity.UnitTesting.TestDoubles
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class DbAsyncEnumerableCollection<TDto> : EnumerableQuery<TDto>, IDbAsyncEnumerable<TDto>, IQueryable<TDto>
    {
        public DbAsyncEnumerableCollection(IEnumerable<TDto> enumerable)
            : base(enumerable)
        { }

        public DbAsyncEnumerableCollection(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<TDto> GetAsyncEnumerator()
        {
            return new DbAsyncEnumerator<TDto>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider => new DbAsyncQueryProvider<TDto>(this);
    }
}
