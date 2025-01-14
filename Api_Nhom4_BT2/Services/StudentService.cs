using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.Models;

namespace Api_Nhom4_BT2.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApiResponse<string> DeleteStudent(int id)
        {
            var isReferenced = _context.Enrollment.Any(enrollment => enrollment.StudentID == id);

            if (isReferenced)
            {
                return ApiResponse<string>.fail("Student cannot be deleted because it is referenced in Enrollment.");
            }

            var existingStudent = _context.Student.FirstOrDefault(student => student.ID == id);
            if (existingStudent == null)
            {
                return ApiResponse<string>.fail("Student not found");
            }

            _context.Student.Remove(existingStudent);
            _context.SaveChanges();
            return ApiResponse<string>.success("Student deleted successfully");
        }
    }
}
