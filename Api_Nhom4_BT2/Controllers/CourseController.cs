using Microsoft.AspNetCore.Mvc;

namespace Api_Nhom4_BT2.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
