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
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            return Created("", ApiResponse<Course>.success(await _courseService.AddCourse(course))); 
        }

    }
}
