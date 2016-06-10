namespace PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess
{
    using Contracts;

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    public class DalBase<TDto, TId> : IDalBase<TDto, TId> where TDto : IdDtoBase<TId>
    {
        private DbContext DbContext { get; }

        public DalBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TDto> Create(TDto dto)
        {
            var createdDto = DbContext.Set<TDto>().Add(dto);
            await DbContext.SaveChangesAsync();
            return createdDto;
        }

        public async Task<TDto> Read(TId id)
        {
            return await SafeRead(id, false);
        }

        public async Task<TDto> Update(TDto dto)
        {
            var storedDto = await SafeRead(dto.Id);

            var hasUpdatedValue = false;
            var unupdateablePropertyNames = new[] { "Id", "CreatedByUserId", "CreatedOn", "LastUpdatedOn" };
            var props = typeof(TDto).GetProperties();
            foreach (var propertyInfo in props)
            {
                var isVirtual = propertyInfo.GetAccessors()[0].IsVirtual;
                if (!isVirtual && unupdateablePropertyNames.All(unupdateablePropertyName => unupdateablePropertyName != propertyInfo.Name))
                {
                    var storedValue = propertyInfo.GetValue(storedDto);
                    var passedValue = propertyInfo.GetValue(dto);

                    if (storedValue == null || passedValue == null)
                    {
                        if ((storedValue == null && passedValue != null) || storedValue != null)
                        {
                            hasUpdatedValue = true;
                            propertyInfo.SetValue(storedDto, passedValue);
                        }
                    }
                    else if (!storedValue.Equals(passedValue))
                    {
                        hasUpdatedValue = true;
                        propertyInfo.SetValue(storedDto, passedValue);
                    }
                }
            }

            if (hasUpdatedValue)
            {
                storedDto.LastUpdatedOn = DateTimeOffset.UtcNow;
                await DbContext.SaveChangesAsync();
            }

            return storedDto;
        }

        public async Task Delete(TId id)
        {
            var dto = await SafeRead(id);
            DbContext.Set<TDto>().Remove(dto);
            await DbContext.SaveChangesAsync();
        }

        private async Task<TDto> SafeRead(TId id)
        {
            return await SafeRead(id, true);
        }

        private async Task<TDto> SafeRead(TId id, bool shouldTrack)
        {
            var dto = await GetEntity<TDto, TId>(e => e.Id, id, shouldTrack);
            if (dto == null) throw new KeyNotFoundException();

            return dto;
        }

        // http://stackoverflow.com/questions/10402029/ef-object-comparison-with-generic-types#answer-10423451
        private async Task<TEntity> GetEntity<TEntity, TKey>(Expression<Func<TEntity, TKey>> property, TKey key, bool shouldTrack = true) where TEntity : IdDtoBase<TKey>
        {
            var query = Filter(DbContext.Set<TEntity>(), property, key);
            return shouldTrack ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }

        private static IQueryable<TEntity> Filter<TEntity, TKey>(IQueryable<TEntity> dbSet, Expression<Func<TEntity, TKey>> property, TKey key)
        {
            var memberExpression = property.Body as MemberExpression;
            if (!(memberExpression?.Member is PropertyInfo))
            {
                throw new ArgumentException("Property expected", nameof(property));
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property.Body, Expression.Constant(key, typeof(TKey))), property.Parameters.Single());
            return dbSet.Where(lambda);
        }
    }
}
