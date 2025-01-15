using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Nhom4_BT2.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Student>> GetAll()
        {
            var listStudent = await _context.Student.ToListAsync();
            return listStudent;
        }
        public async Task<Student> AddStudent(StudentRequest studentRequest)
        {
            var student = new Student
            {
                LastName = studentRequest.LastName,
                FirstMidName = studentRequest.FirstMidName,
                EnrollmentDate = studentRequest.EnrollmentDate
            };

            await _context.Student.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public ApiResponse<Student> UpdateStudent(int id, StudentRequest updateStudentRequest)
        {
            var existingStudent = _context.Student.FirstOrDefault(student => student.ID == id);
            if (existingStudent == null)
            {
                return ApiResponse<Student>.fail("Student not found");
            }

            existingStudent.LastName = updateStudentRequest.LastName;
            existingStudent.FirstMidName = updateStudentRequest.FirstMidName;
            existingStudent.EnrollmentDate = updateStudentRequest.EnrollmentDate;

            _context.SaveChanges();

            return ApiResponse<Student>.success(existingStudent)
                .WithDescription("Student updated successfully");
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
