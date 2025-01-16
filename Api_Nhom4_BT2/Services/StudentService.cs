using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.dto;
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
        public async Task<ApiResponse<Student>> AddStudent(StudentRequest studentRequest)
        {
            if (string.IsNullOrWhiteSpace(studentRequest.LastName) || studentRequest.LastName.Length > 100 ||
            string.IsNullOrWhiteSpace(studentRequest.FirstMidName) || studentRequest.FirstMidName.Length > 100)
            {
                return ApiResponse<Student>.fail("LastName and FirstMidName must not exceed 100 characters and cannot be empty or whitespace.");
            }

            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo hcmTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");

            // Chuyển đổi thời gian UTC sang múi giờ Asia/Ho_Chi_Minh
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, hcmTimeZone);

            // Đảm bảo giá trị lưu vào PostgreSQL luôn ở UTC
            DateTime dateTimeToSave = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            var student = new Student
            {
                LastName = studentRequest.LastName,
                FirstMidName = studentRequest.FirstMidName,
                EnrollmentDate = dateTimeToSave
            };

            await _context.Student.AddAsync(student);
            await _context.SaveChangesAsync();
            return ApiResponse<Student>.success(student);
        }

        public ApiResponse<Student> UpdateStudent(int id, StudentRequest updateStudentRequest)
        {
            if (string.IsNullOrWhiteSpace(updateStudentRequest.LastName) || updateStudentRequest.LastName.Length > 100 ||
           string.IsNullOrWhiteSpace(updateStudentRequest.FirstMidName) || updateStudentRequest.FirstMidName.Length > 100)
            {
                return ApiResponse<Student>.fail("LastName and FirstMidName must not exceed 100 characters and cannot be empty or whitespace.");
            }

            var existingStudent = _context.Student.FirstOrDefault(student => student.ID == id);
            if (existingStudent == null)
            {
                return ApiResponse<Student>.fail("Student not found");
            }

            existingStudent.LastName = updateStudentRequest.LastName;
            existingStudent.FirstMidName = updateStudentRequest.FirstMidName;

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
