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
