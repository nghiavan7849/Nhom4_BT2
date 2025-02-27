﻿using Api_Nhom4_BT2.dto;
using Api_Nhom4_BT2.Models;
using Api_Nhom4_BT2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api_Nhom4_BT2.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listStudent = await _studentService.GetAll();
            return Ok(ApiResponse<IEnumerable<Student>>.success(listStudent));
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequest studentRequest)
        {
            var response = await _studentService.AddStudent(studentRequest);
            if(response.code == 1)
            {
                return BadRequest(response);
            }
            return Created("", response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] StudentRequest updateStudentRequest)
        {
            var response = _studentService.UpdateStudent(id, updateStudentRequest);

            if (response.code == 0)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var response = _studentService.DeleteStudent(id);

            if (response.code == 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
