using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using UNICS.Data.Models.DB;

namespace UNICS.Data.Repository.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Have to add context in startup.cs
        protected readonly UNICSContext context;
        private DbSet<T> _entities;

        public Repository(UNICSContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public T Get(string id)
        {
            try
            {
                // finding record by id
                T entity = _entities.Find(id);
                return entity == null ?
                    throw new NullReferenceException("Not found the given identity") : entity;

            }catch(SqlException ex)
            {
                throw;
            }
            // finish this process then service will return result to view
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
