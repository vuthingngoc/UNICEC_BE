
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.Models.DB;

namespace UNICS.Data.Respository.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // khỏi tạo biến để kết nối CSDL
        // phải thêm vào service ở starup.cs
        protected readonly UNICSContext context;
        //
        private DbSet<T> enitities;


        public Repository(UNICSContext context)
        {
            this.context = context;
            enitities = context.Set<T>();
        }

        public T Get(string id)
        {
            try
            {
                // tìm record theo khóa chính 
                T entity = enitities.Find(id);
                return entity == null ? throw new NullReferenceException("Not found the given identity") : entity;
            }
            catch (SqlException) {
                throw;
            }
            //xử lý xong lên service trả ra view
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }


        //Insert
        public bool Insert(T entity)
        {
            throw new NotImplementedException();
        }


        //Update Or Delete chung 1 hàm
        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
