using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/city")]
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
        [HttpGet("cities")]
        [SwaggerOperation(Summary = "Get cities")]
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
                    //Not has data
                    return Ok(new List<object>());
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
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get city by id")]
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
                  return Ok(new object());
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //Post-Insert-City
        [HttpPost]
        [SwaggerOperation(Summary = "Insert city")]
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
        [HttpPut]
        [SwaggerOperation(Summary = "Update city")]
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
                    //Not has data
                    return Ok(new object());
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
  
    }
}
