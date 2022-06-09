﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/v1/cities")]
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
        //[AllowAnonymous]
        //[HttpGet("update-image")]
        //[SwaggerOperation(Summary = "update image")]
        //public async Task<IActionResult> CreateImage([FromQuery] string filename)
        //{
        //    try
        //    {
        //        return Ok(await _cityService.GetUrlFromFilenameAsync(filename));
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
        [SwaggerOperation(Summary = "Search cities by name - Authenticated user")]
        public async Task<IActionResult> SearchCitiesByName([FromQuery] string name, [FromQuery] PagingRequest request)
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
        [SwaggerOperation(Summary = "Get city by id - Authenticated user")]
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
        [SwaggerOperation(Summary = "Insert city - System Admin")]
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
        [SwaggerOperation(Summary = "Update city - System Admin")]
        public async Task<IActionResult> UpdateCity([FromBody] CityUpdateModel city)
        {
            try
            {
                bool check = await _cityService.Update(city);
                return (check == true) ? Ok() : Ok(new object());
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

        [Authorize(Roles = "System Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete city - System Admin")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                await _cityService.Delete(id);
                return Ok();
            }
            catch (NullReferenceException ex)
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
