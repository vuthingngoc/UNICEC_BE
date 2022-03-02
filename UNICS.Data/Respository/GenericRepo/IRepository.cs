using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNICS.Data.Respository.GenericRepo
{
    //tạo 1 lớp generic dùng để 
    public interface IRepository<T> where T : class
    {
        //Get All
        IEnumerable<T> GetAll();
        //Get By Id
        T Get(String id);
        //Insert
        bool Insert(T entity);
        //Update / Delete = Enum status bằng 0
        bool Update(T entity);

        //

        
    }
    
}
