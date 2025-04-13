using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext dbContext;
        private Hashtable Repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            this.dbContext = dbContext;
            Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        => await dbContext.SaveChangesAsync();


        public ValueTask DisposeAsync()
        => dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;  // Get the name of the entity type (Product)
            if (!Repositories.ContainsKey(type)) // Check if the repository already exists in the hash table
            {
                var Repository = new GenericRepository<TEntity>(dbContext);
                Repositories.Add(type, Repository); // If not, create a new repository and add it to the hash table
            }
            return Repositories[type] as IGenericRepository<TEntity>; // Return the repository from the hash table
        }
    }
}
