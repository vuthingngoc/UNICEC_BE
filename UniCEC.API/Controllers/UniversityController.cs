using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UniversityController : ControllerBase
    {
        // GET: api/<UniversityController>

        //tạo service
        private IUniversityService _universityService;

        //constructor để DI Service vào
        public UniversityController(IUniversityService universityService)
        {
            this._universityService = universityService;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Get 1 university by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            try {
                
                ViewUniversity result = await _universityService.GetUniversityById(id);
                if (result == null)
                {
                    return NotFound();
                }
                else 
                {
                    //
                    return Ok(result);
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // POST api/<UniversityController>
        [HttpPost]
        public async Task<IActionResult> InsertUniversity([FromBody] UniversityInsertModel model)
        {
            //gọi service
            ViewUniversity result = await _universityService.Insert(model);


            return Ok();

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
