using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.CitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;
using UniCEC.Data.ViewModels.Entities.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CityController : ControllerBase
    {


        //gọi service
        private ICityService _ICityService;

        public CityController(ICityService ICityService)
        {
            _ICityService = ICityService;
        }

        // Get-List-Cities
        [HttpGet("GetListCities")]
        public async Task<IActionResult> GetListCity([FromQuery] CityRequestModel request)
        {
            try {
                PagingResult<ViewCity> result = await _ICityService.GetListCities(request);
                if (result != null)
                {
                    
                    return Ok(result);
                }
                else
                {
                    return NotFound();
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

        //GET CITY BY ID
        [HttpGet("GetCityBy{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                ViewCity result = await _ICityService.GetByCityId(id);
                if (result != null)
                {
                  return Ok(result);
                }
                else
                {
                  return NotFound("Not found this City");
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //Post-Insert-City
        [HttpPost("InsertCity")]
        public async Task<IActionResult> InsertCity ([FromBody] CityInsertModel model)
        {
            try
            {
                ViewCity result = await _ICityService.Insert(model);                
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // PUT api/<CityController>/5
        [HttpPut("UpdateCityById")]
        public async Task<IActionResult> UpdateCity([FromBody] ViewCity city)
        {
            try
            {
                Boolean check = false;
                //
                check = await _ICityService.Update(city);
                if (check) {
                    return Ok();
                }
                else {
                    return NotFound("Not found this City");
                }
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // DELETE api/<CityController>/5
        [HttpDelete("NotYet{id}")]
        public void Delete(int id)
        {
        }
    }
}
