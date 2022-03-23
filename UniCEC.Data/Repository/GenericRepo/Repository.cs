using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Have to add context in startup.cs
        protected readonly UniCECContext context;
        private DbSet<T> _entities;

        public Repository(UniCECContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> Get(string id)
        {
            // finding record by id
            T entity = await _entities.FindAsync(id);
            return entity == null ?
                throw new NullReferenceException("Not found the given identity") : entity;

            // finish this process then service will return result to view
        }

        public Task<PagingResult<T>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
