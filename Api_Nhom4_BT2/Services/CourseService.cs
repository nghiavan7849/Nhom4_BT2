using Api_Nhom4_BT2.DBContext;
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

        public async Task<Course> AddCourse(CourseRequest courseRequest)
        {
            Course course = new Course();
            course.Title = courseRequest.Title;
            course.Credits = courseRequest.Credits;

            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public ApiResponse<Course> UpdateCourse(int id, Course updateCourse)
        {
            var existingCourse = _context.Course.FirstOrDefault(course => course.CourseID == id);
            if (existingCourse == null) 
            {
                return ApiResponse<Course>.fail("Course not found");
            }

            existingCourse.Title = updateCourse.Title;
            existingCourse.Credits = updateCourse.Credits;

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
