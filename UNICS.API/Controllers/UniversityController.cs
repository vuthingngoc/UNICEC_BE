﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UNICS.Business.Services.UniversitySvc;
using UNICS.Data.ViewModels.Entities.University;

namespace UNICS.API.Controllers
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
        public IActionResult GetUniversityById(String id)
        {

            //chưa check null
            ViewUniversity result = _universityService.GetUniversityById(id);
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