using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.Models;
using Api_Nhom4_BT2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Nhom4_BT2.Controllers
{
    [ApiController]
    [Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService) {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {

            var listCourse = await _courseService.GetAll();

            return Ok(ApiResponse<IEnumerable<Course>>.success(listCourse));
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseRequest courseRequest)
        {
            var response = await _courseService.AddCourse(courseRequest);
            if(response.code == 1) 
                return BadRequest(response);
            
            return Created("", response); 
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] CourseRequest updateCourse)
        {
            var response = _courseService.UpdateCourse(id, updateCourse);

            if (response.code == 0)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var response = _courseService.DeleteCourse(id);

            if (response.code == 0)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
