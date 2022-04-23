using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<T> Get(int id)
        {          
            return await _entities.FindAsync(id);
        }

        public async Task<PagingResult<T>> GetAllPaging(PagingRequest request)
        {
            List<T> items = await _entities.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            return (items.Count > 0) ? new PagingResult<T>(items, _entities.Count(), request.CurrentPage, request.PageSize) : null;
        }

        public async Task<int> Insert(T entity)
        {           
            await _entities.AddAsync(entity);
            await Update();
            return (int)entity.GetType().GetProperty("Id").GetValue(entity);            
        }

        public async Task Update()
        {
            await context.SaveChangesAsync();
        }
    }
}
