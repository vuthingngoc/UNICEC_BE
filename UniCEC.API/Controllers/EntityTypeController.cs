using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.EntityTypeSvc;
using UniCEC.Data.ViewModels.Entities.EntityType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/entity-types")]
    [ApiController]
    [ApiVersion("1.0")]
    //[Authorize(Roles = "System Admin")]
    public class EntityTypeController : ControllerBase
    {
        private IEntityTypeService _entityTypeService;
        public EntityTypeController(IEntityTypeService entityTypeService)
        {
            _entityTypeService = entityTypeService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get entity type by id - System Admin")]
        public async Task<IActionResult> GetEntityTypeById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewEntityType result = await _entityTypeService.GetEntityTypeById(id, token);
                return Ok(result);

            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all entity type - System Admin")]
        public async Task<IActionResult> GetAllEntityType()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewEntityType> result = await _entityTypeService.GetAllEntityType(token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insert entity type - System Admin")]
        public async Task<IActionResult> InsertNewEntityType([FromQuery] string name)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewEntityType entityType = await _entityTypeService.Insert(name, token);
                return Ok(entityType);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update entity type - System Admin")]
        public async Task<IActionResult> UpdateEntityType([FromBody, BindRequired] ViewEntityType model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _entityTypeService.Update(model, token);
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }
    }
}
