using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.SeedsWalletSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/seeds-wallets")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class SeedsWalletController : ControllerBase
    {
        private ISeedsWalletService _seedsWalletService;

        public SeedsWalletController(ISeedsWalletService seedsWalletService)
        {
            _seedsWalletService = seedsWalletService;
        }

        // Search 
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search seeds wallet by conditions - Authenticated user")]
        public async Task<IActionResult> SearchCitiesByName([FromQuery] SeedsWalletRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewSeedsWallet> result = await _seedsWalletService.GetByConditions(token, request);
                return Ok(result);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get seeds wallet by id - Authenticated user")]
        public async Task<IActionResult> GetSeedsWalletById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewSeedsWallet result = await _seedsWalletService.GetById(token, id);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

        // maybe open in the future
        //[HttpPost]
        //[SwaggerOperation(Summary = "Insert seeds wallet - Authenticated user")]
        //public async Task<IActionResult> InsertSeedsWallet([FromQuery] int studentId)
        //{
        //    try
        //    {
        //        string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
        //        ViewSeedsWallet result = await _seedsWalletService.Insert(token, studentId);
        //        return Ok(result);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        [HttpPut]
        [SwaggerOperation(Summary = "Update seeds wallet - Authenticated user")]
        public async Task<IActionResult> UpdateSeedsWallet([FromBody] SeedsWalletUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _seedsWalletService.Update(token, model);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
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

        //[HttpDelete("{id}")]
        //[SwaggerOperation(Summary = "Delete seeds wallet - Authenticated user")]
        //public async Task<IActionResult> DeleteSeedsWallet(int id)
        //{
        //    try
        //    {
        //        string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
        //        await _seedsWalletService.Delete(token, id);
        //        return Ok();
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}
    }
}
