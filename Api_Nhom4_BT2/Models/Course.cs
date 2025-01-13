using System.ComponentModel.DataAnnotations;

namespace Api_Nhom4_BT2.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
    }
}
