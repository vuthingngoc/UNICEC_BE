using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Data.Repository.ImplRepo.UniversityRepo
{
    public class UniversityRepo : Repository<University>, IUniversityRepo
    {
        public UniversityRepo(UniCECContext context) : base(context)
        {

        }
        //Check-University-Email
        public async Task<bool> CheckEmailUniversity(string email)
        {
            University university = await context.Universities.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return (university != null) ? true : false;
        }

        //Get-List-Universities-By-Email
        public async Task<List<ViewUniversity>> GetListUniversityByEmail(string email)
        {
            //Constain Email
            var query = from University u in context.Universities
                        where u.Email.Contains(email)
                        select u;

            List<University> universities = await query.ToListAsync();
            List<ViewUniversity> viewUniversities = new List<ViewUniversity>();
            //return view
            universities.ForEach(u =>
            {
                ViewUniversity viewUniversity = new ViewUniversity();
                viewUniversity.Id = u.Id;
                viewUniversity.CityId = u.CityId;
                viewUniversity.UniCode = u.UniCode;
                viewUniversity.Name = u.Name;
                viewUniversity.Description = u.Description;
                viewUniversity.Phone = u.Phone;
                viewUniversity.Email = u.Email;
                viewUniversity.Founding = u.Founding;
                viewUniversity.Openning = u.Openning;
                viewUniversity.Closing = u.Closing;
                viewUniversity.Status = u.Status;
                //
                viewUniversities.Add(viewUniversity);
            });

            return viewUniversities;

        }

        public async Task<string> GetNameUniversityById(int id)
        {
            var university = await context.Universities.FindAsync(id);
            return (university != null) ? university.Name : null;
        }


        //Get-Universities-By-Conditions
        public async Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request)
        {
            List<ViewUniversity> viewUniversities = new List<ViewUniversity>();
            PagingResult<ViewUniversity> result = null;
            int count = 0;
            //Check Conditions
            //--------Status
            if (request.Status.HasValue)
            {
                //Constains Name
                var queryStatus = from University u in context.Universities
                               where u.Status == request.Status && u.Name.Contains(request.Name)
                               select u;

                //City Id
                if (request.CityId.HasValue)
                {
                    queryStatus = from University u in queryStatus
                                  where u.CityId == request.CityId
                                  select u;
                }
                //Paging
                count = queryStatus.Count();
                List<University> listUni = await queryStatus.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                
                //return view
                listUni.ForEach(u =>
                {
                    ViewUniversity viewUniversity = new ViewUniversity();
                    viewUniversity.Id = u.Id;
                    viewUniversity.CityId = u.CityId;
                    viewUniversity.UniCode = u.UniCode;
                    viewUniversity.Name = u.Name;
                    viewUniversity.Description = u.Description;
                    viewUniversity.Phone = u.Phone;
                    viewUniversity.Email = u.Email;
                    viewUniversity.Founding = u.Founding;
                    viewUniversity.Openning = u.Openning;
                    viewUniversity.Closing = u.Closing;
                    viewUniversity.Status = u.Status;
                    //
                    viewUniversities.Add(viewUniversity);
                });
                result = new PagingResult<ViewUniversity>(viewUniversities, count, request.CurrentPage, request.PageSize);
                return result;

            }
            //NoStatus
            else {

                var queryAll = from University u in context.Universities
                               select u;
                              

                //Constains Name
                if (request.Name != null)
                {
                     queryAll = from University u in context.Universities
                                   where u.Name.Contains(request.Name)
                                   select u;
                }

                //City Id
                if (request.CityId.HasValue)
                {
                    queryAll = from University u in queryAll
                                  where u.CityId == request.CityId
                                  select u;
                }

                //Paging
                count = queryAll.Count();
                List<University> listUni = await queryAll.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                //return view
                listUni.ForEach(u =>
                {
                    ViewUniversity viewUniversity = new ViewUniversity();
                    viewUniversity.Id = u.Id;
                    viewUniversity.CityId = u.CityId;
                    viewUniversity.UniCode = u.UniCode;
                    viewUniversity.Name = u.Name;
                    viewUniversity.Description = u.Description;
                    viewUniversity.Phone = u.Phone;
                    viewUniversity.Email = u.Email;
                    viewUniversity.Founding = u.Founding;
                    viewUniversity.Openning = u.Openning;
                    viewUniversity.Closing = u.Closing;
                    viewUniversity.Status = u.Status;
                    //
                    viewUniversities.Add(viewUniversity);
                });
                result = new PagingResult<ViewUniversity>(viewUniversities, count, request.CurrentPage, request.PageSize);
                return result;
            }

        }
    }
}
