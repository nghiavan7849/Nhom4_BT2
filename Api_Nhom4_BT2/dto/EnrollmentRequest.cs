using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_Nhom4_BT2.dto
{
    public class EnrollmentRequest
    {
        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public string Grade { get; set; }
    }
}
