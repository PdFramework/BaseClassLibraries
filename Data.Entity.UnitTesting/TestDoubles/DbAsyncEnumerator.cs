namespace PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity.UnitTesting.TestDoubles
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;

    public class DbAsyncEnumerator<TDto> : IDbAsyncEnumerator<TDto>
    {
        private readonly IEnumerator<TDto> _inner;

        public DbAsyncEnumerator(IEnumerator<TDto> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public TDto Current => _inner.Current;

        object IDbAsyncEnumerator.Current => Current;
    }
}
