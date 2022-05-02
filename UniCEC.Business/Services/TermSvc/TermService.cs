using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.TermSvc
{
    public class TermService : ITermService
    {
        private ITermRepo _termRepo;
        public TermService(ITermRepo termRepo)
        {
            _termRepo = termRepo;
        }

        private ViewTerm TransformViewTerm(Term term)
        {
            return new ViewTerm()
            {
                Id = term.Id,
                Name = term.Name,
                CreateTime = term.CreateTime,
                EndTime = term.EndTime
            };
        }

        public async Task<PagingResult<ViewTerm>> GetByClub(int clubId, PagingRequest request)
        {
            PagingResult<Term> terms = await _termRepo.GetByClub(clubId, request);
            if (terms == null) throw new NullReferenceException("Not found any term");

            List<ViewTerm> items = new List<ViewTerm>();
            terms.Items.ForEach(element =>
            {
                ViewTerm term = TransformViewTerm(element);
                items.Add(term);
            });

            return new PagingResult<ViewTerm>(items, terms.TotalCount, request.CurrentPage, request.PageSize);
        }

        public async Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request)
        {
            PagingResult<Term> terms = await _termRepo.GetByConditions(clubId, request);
            if (terms == null) throw new NullReferenceException("Not found any term");

            List<ViewTerm> items = new List<ViewTerm>();
            terms.Items.ForEach(element =>
            {
                ViewTerm term = TransformViewTerm(element);
                items.Add(term);
            });

            return new PagingResult<ViewTerm>(items, terms.TotalCount, request.CurrentPage, request.PageSize);
        }

        public async Task<ViewTerm> GetById(int clubId, int id)
        {
            Term term = await _termRepo.GetById(clubId, id);
            if (term == null) throw new NullReferenceException("Not found this term");
            return TransformViewTerm(term);
        }

        public Task<ViewTerm> Insert(TermInsertModel term)
        {
            throw new NotImplementedException();
        }

        public Task Update(ViewTerm term)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
