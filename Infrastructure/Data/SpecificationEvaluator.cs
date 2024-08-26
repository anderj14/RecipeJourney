using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            // Apply criteria (Where clause)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply paging if enabled
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Apply includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

    }
}