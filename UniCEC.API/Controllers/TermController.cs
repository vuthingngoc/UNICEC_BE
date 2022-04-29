using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.TermSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TermController : ControllerBase
    {
        private ITermService _itermService;
        public TermController(ITermService termService)
        {
            _itermService = termService;
        }

        [HttpGet]
        public Task<IActionResult> GetAllTerm([FromQuery] PagingRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetTermById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{search}")]
        public Task<IActionResult> GetTermByConditions(TermRequestModel request)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> InsertTerm(TermInsertModel term)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> UpdateTerm(ViewTerm term)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteTerm(int id)
        {
            throw new NotImplementedException();
        }
    }
}
