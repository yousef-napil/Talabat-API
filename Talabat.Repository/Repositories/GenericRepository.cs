using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Entities.ProductModule;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepository(StoreContext dbContext ) 
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CountAsync(ISpecifications<T> countSpec)
        {
            return await ApplySpecification(countSpec).CountAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await dbContext.Set<T>().ToListAsync();

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), spec).ToListAsync();
        //}

        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsyncWithSpec(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}

