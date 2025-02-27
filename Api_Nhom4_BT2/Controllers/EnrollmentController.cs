﻿using Api_Nhom4_BT2.dto;
using Api_Nhom4_BT2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprache;

namespace Api_Nhom4_BT2.Controllers
{
    [ApiController]
    [Route("api/enrollment")]
    public class EnrollmentController : Controller
    {
        private readonly EnrollmentService enrollmentService;

        public EnrollmentController(EnrollmentService service)
        {
            enrollmentService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listEnrollment = await enrollmentService.GetAllEnrollment();

            return Ok(listEnrollment);

        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment([FromBody] EnrollmentRequest enrollment)
        {
            var result = await enrollmentService.AddEnrollment(enrollment);
            if (result.code == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] EnrollmentRequest enrollmentRespone)
        {
            var result = await enrollmentService.UpdateEnrollment(id, enrollmentRespone);

            if (result.code == 1)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // New Delete Method kien
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var result = await enrollmentService.DeleteEnrollment(id);
            if (result.code == 1)
            {
                return NotFound(result);
            }
            // return Ok();
            return Ok(result);
        }
    }
}
