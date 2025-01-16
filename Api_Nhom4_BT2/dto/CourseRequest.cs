using System.ComponentModel.DataAnnotations;

namespace Api_Nhom4_BT2.dto
{
    public class CourseRequest
    {
        public string Title { get; set; }
        public int? Credits { get; set; }

    }
}
