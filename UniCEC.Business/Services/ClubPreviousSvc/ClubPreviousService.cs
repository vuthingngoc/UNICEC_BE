using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.Business.Services.ClubPreviousSvc
{
    public class ClubPreviousService : IClubPreviousService
    {
        private IClubPreviousRepo _clubPreviousRepo;

        public ClubPreviousService(IClubPreviousRepo clubPreviousRepo)
        {
            _clubPreviousRepo = clubPreviousRepo;
        }

        private ViewClubPrevious TransformViewClubPrevious(ClubPreviou clubPrevious)
        {
            return new ViewClubPrevious()
            {
                Id = clubPrevious.Id,
                ClubId = clubPrevious.ClubId,
                ClubRoleId = clubPrevious.ClubRoleId,
                MemberId = clubPrevious.MemberId,
                StartTime = clubPrevious.StartTime,
                EndTime = clubPrevious.EndTime
            };
        }

        public async Task<PagingResult<ViewClubPrevious>> GetAllPaging(PagingRequest request)
        {
            PagingResult<ClubPreviou> listClubPrevious = await _clubPreviousRepo.GetAllPaging(request);
            if (listClubPrevious == null) throw new NullReferenceException("Not found any previous clubs");
            
            List<ViewClubPrevious> previousClubs = new List<ViewClubPrevious>();
            listClubPrevious.Items.ForEach(element =>
            {
                ViewClubPrevious clubPrevious = TransformViewClubPrevious(element);
                previousClubs.Add(clubPrevious);
            });

            return new PagingResult<ViewClubPrevious>(previousClubs, listClubPrevious.TotalCount, listClubPrevious.CurrentPage, listClubPrevious.PageSize);
        }

        public async Task<ViewClubPrevious> GetByClubPrevious(int id)
        {
            ClubPreviou clubPrevious = await _clubPreviousRepo.Get(id);
            if (clubPrevious == null) throw new NullReferenceException("Not found this club previous");
            return TransformViewClubPrevious(clubPrevious);

        }

        public async Task<PagingResult<ViewClubPrevious>> GetByContitions(ClubPreviousRequestModel request)
        {
            PagingResult<ClubPreviou> listClubPrevious = await _clubPreviousRepo.GetByConditions(request);
            if (listClubPrevious == null) throw new NullReferenceException("Not found any previous clubs");

            List<ViewClubPrevious> previousClubs = new List<ViewClubPrevious>();
            listClubPrevious.Items.ForEach(element =>
            {
                ViewClubPrevious clubPrevious = TransformViewClubPrevious(element);
                previousClubs.Add(clubPrevious);
            });

            return new PagingResult<ViewClubPrevious>(previousClubs, listClubPrevious.TotalCount, listClubPrevious.CurrentPage, listClubPrevious.PageSize);            
        }

        public async Task<ViewClubPrevious> Insert(ClubPreviousInsertModel clubPrevious)
        {
            if (clubPrevious == null) throw new ArgumentNullException("Null argument");
            string year = DateTime.Now.Year.ToString();
            int checkId = await _clubPreviousRepo.CheckDuplicated(clubPrevious.ClubId, clubPrevious.ClubRoleId, clubPrevious.MemberId, year);
            if (checkId > 0) throw new ArgumentException("Duplicated record");

            ClubPreviou clubPreviousObject = new ClubPreviou()
            {
                ClubId = clubPrevious.ClubId,
                ClubRoleId = clubPrevious.ClubRoleId,
                MemberId = clubPrevious.MemberId,
                StartTime = clubPrevious.StartTime,
                EndTime = clubPrevious.EndTime,
                Year = year,
                Status = ClubPreviousStatus.Active
            };
            int id = await _clubPreviousRepo.Insert(clubPreviousObject);
            clubPreviousObject.Id = id;
            return TransformViewClubPrevious(clubPreviousObject);
        }

        public async Task<bool> Update(ClubPreviousUpdateModel clubPrevious)
        {
            if (clubPrevious == null) throw new ArgumentNullException("Null argument");            

            ClubPreviou clubPreviousObject = await _clubPreviousRepo.Get(clubPrevious.Id);
            if (clubPreviousObject == null) throw new NullReferenceException("Not found this club previous");
            // check duplicated record when change role member
            if(clubPreviousObject.ClubRoleId != clubPrevious.ClubRoleId)
            {
                int checkId = await _clubPreviousRepo.CheckDuplicated(clubPreviousObject.ClubId, clubPrevious.ClubRoleId, clubPreviousObject.MemberId, clubPrevious.Year);
                if (checkId > 0) throw new ArgumentException("Duplicated record");
            }

            clubPreviousObject.ClubRoleId = clubPrevious.ClubRoleId;
            clubPreviousObject.StartTime = clubPrevious.StartTime;
            clubPreviousObject.EndTime = clubPrevious.EndTime;
            clubPreviousObject.Status = clubPrevious.Status;
            clubPreviousObject.Year = clubPrevious.Year;            

            return await _clubPreviousRepo.Update();
        }

        public async Task<bool> Delete(int id)
        {
            ClubPreviou clubPreviousObject = await _clubPreviousRepo.Get(id);
            if (clubPreviousObject == null) throw new NullReferenceException("Not found this club previous");
            if (clubPreviousObject.Status == 0) return true;
            clubPreviousObject.Status = 0;
            return await _clubPreviousRepo.Update();
        }
    }
}
