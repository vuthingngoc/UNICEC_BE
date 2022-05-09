using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public class MemberService : IMemberService
    {
        private IMemberRepo _memberRepo;

        public MemberService(IMemberRepo memberRepo)
        {
            _memberRepo = memberRepo;
        }

        //Transfer
        private ViewMember TransferViewModel(Member member)
        {
            return new ViewMember()
            {
                Id = member.Id,                
                //Status = member.Status,
            };

        }

        public Task<PagingResult<ViewMember>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewMember> GetByMemberId(int id)
        {
            ViewMember member = await _memberRepo.GetById(id);
            if (member == null) throw new NullReferenceException("Not found this member");
            return member;
        }

        public async Task<List<ViewMember>> GetLeadersByClub(int clubId)
        {
            List<ViewMember> members = await _memberRepo.GetLeadersByClub(clubId);
            if (members == null) throw new NullReferenceException("Not found any Leaders");
            return members;
        }

        public async Task<int> GetQuantityNewMembersByClub(int clubId)
        {
            int quantity = await _memberRepo.GetQuantityNewMembersByClub(clubId);
            if (quantity < 0) throw new NullReferenceException("Not found this club");
            return quantity;
        }

        //Insert-Member
        public async Task<ViewMember> Insert(MemberInsertModel model)
        {
            bool isMember = await _memberRepo.CheckExistedMemberInClub(model.StudentId, model.ClubId);
            if (isMember) throw new ArgumentException("The user has already in this club");
            try
            {
                Member member = new Member()
                {
                    StudentId = model.StudentId,
                    //Status = ,
                };
                //
                int result = await _memberRepo.Insert(member);
                if (result > 0)
                {
                    //
                    Member m = await _memberRepo.Get(result);
                    ViewMember viewMember = TransferViewModel(m);
                    return viewMember;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Update-Member
        public async Task<bool> Update(MemberUpdateModel model)
        {
            try
            {
                //get that member
                Member member = await _memberRepo.Get(model.Id);

                if (member != null)
                {
                    //member.Status = (model.Status != null) ? model.Status : member.Status;
                    await _memberRepo.Update();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Member member = await _memberRepo.Get(id);
                if (member != null)
                {
                    //
                    //member.Status = false;
                    await _memberRepo.Update();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
