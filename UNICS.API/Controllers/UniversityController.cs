using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UNICS.Bussiness.Services.UniversitySvc;
using UNICS.Bussiness.ViewModels.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UNICS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UniversityController : ControllerBase
    {
        // GET: api/<UniversityController>

        //tạo service
        IUniversityService universityService;

        //constructor để DI Service vào
        public UniversityController(IUniversityService universityService )
        {
            this.universityService = universityService;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Get 1 university by ID
        [HttpGet("{id}")]
        public IActionResult GetUniversityById(String id)
        {

            //chưa check null
            ViewUniversity result = universityService.GetUniversityById(id);
            //
            return Ok(result);
        }

        // POST api/<UniversityController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UniversityController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UniversityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
