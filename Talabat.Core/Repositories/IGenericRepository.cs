using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<int> CountAsync(ISpecifications<T> countSpec);
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> spec);
        Task<T> GetByIdAsyncWithSpec(ISpecifications<T> spec);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
