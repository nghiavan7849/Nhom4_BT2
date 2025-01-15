using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Nhom4_BT2.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        [ForeignKey("CourseID")]
        public int CourseID { get; set; }

        [ForeignKey("ID")]
        public int StudentID { get; set; }

        public string Grade { get; set; }

        public Course Course { get;}
        public Student Student { get;}
    }
}
