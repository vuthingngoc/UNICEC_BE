using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CitySvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/city")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class CityController : ControllerBase
    {
        //gọi service
        private ICityService _cityService;

        public CityController(ICityService ICityService)
        {
            _cityService = ICityService;
        }

        // Test upload file

        //[HttpPost("update-image/{filename}")]
        //[SwaggerOperation(Summary = "update image")]
        //public async Task<IActionResult> CreateImage(string filename)
        //{
        //    try
        //    {

        //        string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
        //        IFormFile file = Request.Form.Files[0];

        //        await _ICityService.UpdateFile(filename, file, token);
        //        return Ok();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
        

        // Search cities
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search cities by name")]
        public async Task<IActionResult> SearchCitiesByName([FromQuery] string name, PagingRequest request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewCity> result = await _cityService.SearchCitiesByName(name, token, request);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
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
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewCity result = await _cityService.GetByCityId(id, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //Post-Insert-City
        [Authorize(Roles = "System Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert city - Admin")]
        public async Task<IActionResult> InsertCity([FromBody] CityInsertModel model)
        {
            try
            {
                ViewCity result = await _cityService.Insert(model);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
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
        [Authorize(Roles = "System Admin")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update city - Admin")]
        public async Task<IActionResult> UpdateCity([FromBody] ViewCity city)
        {
            try
            {
                Boolean check = false;
                //
                check = await _cityService.Update(city);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    //Not has data
                    return Ok(new object());
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
