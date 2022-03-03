using System;
using System.Collections.Generic;

namespace UNICS.Data.Repository.GenericRepo
{
    public interface IRepository<T> where T : class
    {
        // Get All
        IEnumerable<T> GetAll();
        // Get By Id
        T Get(String id);
        // Insert
        bool Insert(T entity);
        // Update / Delete = Enum status equal 0
        bool Update(T entity);

        //
    }
}
