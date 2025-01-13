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

        public async Task<Course> AddCourse(Course course)
        {
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();

            return course;
        }
    }
}
