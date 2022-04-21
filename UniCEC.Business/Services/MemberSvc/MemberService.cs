using System;
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
                StudentId = member.StudentId,
                Status = member.Status,
            };

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                bool check = false;
                //
                Member member = await _memberRepo.Get(id);
                if (member != null)
                {
                    //
                    member.Status = false;
                    check = await _memberRepo.Update();
                    if (check)
                    {
                        return check;
                    }
                }
                else
                {
                    return check;
                }
                return check;
            }
            catch (Exception) { throw; }
        }

        public Task<PagingResult<ViewMember>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewMember> GetByMemberId(int id)
        {
            //
            Member mem = await _memberRepo.Get(id);
            //
            if (mem != null)
            {
                return new ViewMember()
                {
                    Id = mem.Id,
                    StudentId = mem.StudentId,
                    Status = mem.Status
                };
            }
            else return null;
        }



        //Insert-Member
        public async Task<ViewMember> Insert(MemberInsertModel model)
        {
            try
            {
                Member member = new Member()
                {
                    StudentId = model.StudentId,
                    Status = model.Status,
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
            catch (Exception) { throw; }
        }

        //Update-Member
        public async Task<bool> Update(MemberUpdateModel model)
        {
            try
            {
                //get that member
                Member member = await _memberRepo.Get(model.Id);
                bool check = false;

                if (member != null)
                {
                    member.Status = (model.Status != null) ? model.Status : member.Status;
                    check = await _memberRepo.Update();
                    return check;
                }
                return check;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
