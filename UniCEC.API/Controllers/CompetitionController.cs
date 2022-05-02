using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionController : ControllerBase
    {
        
       
        // GET api/<CompetitionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CompetitionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CompetitionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompetitionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
