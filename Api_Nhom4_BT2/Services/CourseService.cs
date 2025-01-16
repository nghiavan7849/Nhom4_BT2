using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.dto;
using Api_Nhom4_BT2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Nhom4_BT2.Services
{
    public class CourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            var listCourse = await _context.Course.ToListAsync();

            return listCourse;
        }

        public async Task<ApiResponse<Course>> AddCourse(CourseRequest courseRequest)
        {
            if (string.IsNullOrEmpty(courseRequest.Title))
            {
                return ApiResponse<Course>.fail("The Title field is required.");
            }
            if (!courseRequest.Credits.HasValue)
            {
                return ApiResponse<Course>.fail("The Credits field is required.");
            } else if (courseRequest.Credits < 0)
            {
                return ApiResponse<Course>.fail("The Credit field must be greater than 0.");
            }

            Course course = new Course();
            course.Title = courseRequest.Title;
            if(courseRequest.Credits.HasValue)
                course.Credits = courseRequest.Credits.Value;

            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();


            return ApiResponse<Course>.success(course);
        }

        public ApiResponse<Course> UpdateCourse(int id, CourseRequest updateCourse)
        {
            var existingCourse = _context.Course.FirstOrDefault(course => course.CourseID == id);
            if (existingCourse == null) 
            {
                return ApiResponse<Course>.fail("Course not found");
            }
            if (updateCourse.Credits < 0)
            {
                return ApiResponse<Course>.fail("The Credit field must be greater than 0.");
            }
            existingCourse.Title = updateCourse.Title;
            if (updateCourse.Credits.HasValue)
                existingCourse.Credits = updateCourse.Credits.Value;

            _context.SaveChanges();

            return ApiResponse<Course>.success(existingCourse)
                .WithDescription("Course updated successfully");
        }

        public ApiResponse<string> DeleteCourse(int id)
        {
            var isReferenced = _context.Enrollment.Any(enrollment => enrollment.CourseID == id);
        
            if (isReferenced)
            {
               return ApiResponse<string>.fail("Course cannot be deleted because it is referenced in Enrollment.");
            }

            var existingCourse = _context.Course.FirstOrDefault(course => course.CourseID == id);
            if (existingCourse == null)
            {
                return ApiResponse<string>.fail("Course not found");
            }

            _context.Course.Remove(existingCourse);
            _context.SaveChanges();

            return ApiResponse<string>.success("Course deleted successfully.");
        }
    }
}
