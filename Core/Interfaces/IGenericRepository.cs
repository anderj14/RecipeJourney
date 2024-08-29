using System.Linq.Expressions;
using Core.Entities;
using Core.Specification;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListByConditionAsync(Expression<Func<T, bool>> predicate = null);

        Task<int> CountAsync(ISpecification<T> spec);

        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);

        Task SaveAsync();
    }
}