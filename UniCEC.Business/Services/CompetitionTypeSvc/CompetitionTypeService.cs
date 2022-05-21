using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionType;

namespace UniCEC.Business.Services.CompetitionTypeSvc
{
    public class CompetitionTypeService : ICompetitionTypeService
    {
        private ICompetitionTypeRepo _competitionTypeRepo;

        public CompetitionTypeService(ICompetitionTypeRepo competitionTypeRepo)
        {
            _competitionTypeRepo = competitionTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingResult<ViewCompetitionType>> GetAllPaging(PagingRequest request)
        {
            PagingResult<CompetitionType> result = await _competitionTypeRepo.GetAllPaging(request);
            if (result != null)
            {
                List<ViewCompetitionType> viewCompetitionTypes = new List<ViewCompetitionType>();
                result.Items.ForEach(item =>
                {
                    ViewCompetitionType viewCompetitionType = TransformView(item);
                    viewCompetitionTypes.Add(viewCompetitionType);
                });
                return new PagingResult<ViewCompetitionType>(viewCompetitionTypes, result.TotalCount, request.CurrentPage, request.PageSize);
            }
            throw new NullReferenceException();
        }


        public async Task<ViewCompetitionType> GetByCompetitionTypeId(int id)
        {
            //
            CompetitionType comt = await _competitionTypeRepo.Get(id);
            if (comt != null)
            {
                return TransformView(comt);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public async Task<ViewCompetitionType> Insert(CompetitionTypeInsertModel competitionType)
        {
            try
            {
                CompetitionType comt = new CompetitionType();
                comt.TypeName = competitionType.TypeName;
                int result = await _competitionTypeRepo.Insert(comt);
                if (result > 0)
                {
                    return TransformView(comt);
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

        public async Task<bool> Update(ViewCompetitionType competitionType)
        {
            try
            {
                CompetitionType comp = await _competitionTypeRepo.Get(competitionType.Id);
                if (comp != null)
                {
                    comp.TypeName = (!competitionType.TypeName.Equals(""))? competitionType.TypeName : comp.TypeName ;
                    await _competitionTypeRepo.Update();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        private ViewCompetitionType TransformView(CompetitionType comt)
        {
            return new ViewCompetitionType()
            {
                Id = comt.Id,
                TypeName = comt.TypeName
            };
        }
    }
}
