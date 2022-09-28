using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Data.Repository.ImplRepo.UniversityRepo
{
    public class UniversityRepo : Repository<University>, IUniversityRepo
    {
        private IUserRepo _userRepo;

        public UniversityRepo(UniCECContext context, IUserRepo userRepo) : base(context)
        {
            _userRepo = userRepo;
        }

        //Check-University-Email
        public async Task<bool> CheckEmailUniversity(string email)
        {
            University university = await context.Universities.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return (university != null) ? true : false;
        }

        public async Task UpdateStatusByCityId(int cityId, bool status)
        {
            List<University> universities = await (from u in context.Universities
                                                   where u.CityId.Equals(cityId)
                                                   select u).ToListAsync();

            if (universities.Count > 0)
            {
                foreach (var university in universities)
                {
                    university.Status = status;
                    UserStatus userStatus = UserStatus.Active;
                    if (status == false) userStatus = UserStatus.InActive;
                    await _userRepo.UpdateStatusByUniversityId(university.Id, userStatus);
                }
                await Update();
            }
        }

        public async Task DeleteUniversity(int UniversityId)
        {
            var query = from u in context.Universities
                        where u.Id == UniversityId
                        select u;
            University uni = await query.FirstOrDefaultAsync();
            context.Universities.Remove(uni);
            await Update();
        }

        public async Task<ViewUniversity> GetById(int id)
        {
            return await (from u in context.Universities
                          join c in context.Cities on u.CityId equals c.Id
                          where u.Id.Equals(id)
                          select new ViewUniversity()
                          {
                              Id = u.Id,
                              CityId = u.CityId,
                              UniCode = u.UniCode,
                              Name = u.Name,
                              Description = u.Description,
                              Phone = u.Phone,
                              Email = u.Email,
                              Founding = u.Founding,
                              Opening = u.Openning,
                              Closing = u.Closing,
                              Status = u.Status,
                              CityName = c.Name,
                              ImgURL = u.ImageUrl,
                          }).FirstOrDefaultAsync();
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
                viewUniversity.Opening = u.Openning;
                viewUniversity.Closing = u.Closing;
                viewUniversity.Status = u.Status;
                viewUniversity.CityName = u.City.Name;
                viewUniversity.ImgURL = u.ImageUrl;
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

            var query = from University u in context.Universities
                        select u;

            //Status
            if (request.Status.HasValue) query = query.Where(u => u.Status == request.Status.Value);

            //City Id
            if (request.CityId.HasValue) query = query.Where(u => u.CityId == request.CityId.Value);

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(u => u.Name.Contains(request.Name));

            //Paging
            count = query.Count();

            List<University> listUni = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

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
                viewUniversity.Opening = u.Openning;
                viewUniversity.Closing = u.Closing;
                viewUniversity.Status = u.Status;
                viewUniversity.CityName = u.City.Name;
                viewUniversity.ImgURL = u.ImageUrl;
                viewUniversities.Add(viewUniversity);
            });
            result = new PagingResult<ViewUniversity>(viewUniversities, count, request.CurrentPage, request.PageSize);
            return result;
        }

        public async Task<List<ViewUniversity>> GetUniversities()
        {

            var query = from University u in context.Universities

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
                viewUniversity.Opening = u.Openning;
                viewUniversity.Closing = u.Closing;
                viewUniversity.Status = u.Status;
                viewUniversity.CityName = u.City.Name;
                viewUniversity.ImgURL = u.ImageUrl;
            //
            viewUniversities.Add(viewUniversity);
            });

            return viewUniversities;
        }

        public async Task<int> CheckDuplicatedUniversity(string name, int cityId, string uniCode)
        {
            return await (from u in context.Universities
                          where (u.Name.ToLower().Equals(name.ToLower()) && u.CityId.Equals(cityId))
                                    || u.UniCode.ToLower().Equals(uniCode.ToLower())
                          select u.Id).FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetListIdsUniByCity(int cityId)
        {
            return await (from u in context.Universities
                          where u.CityId.Equals(cityId)
                          select u.Id).ToListAsync();
        }

        public async Task<bool> CheckExistedUniversity(int universityId)
        {
            return await context.Universities.FirstOrDefaultAsync(uni => uni.Id.Equals(universityId) && uni.Status.Equals(true)) != null;
        }
    }
}
