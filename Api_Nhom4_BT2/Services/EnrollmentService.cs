using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Nhom4_BT2.Services
{
    public class EnrollmentService
    {
        private readonly ApplicationDbContext dbContext;

        public EnrollmentService(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<ApiResponse<IEnumerable<Enrollment>>> GetAllEnrollment()
        {
            try
            {
                var listEnrollment = await dbContext.Enrollment.Include(e => e.Course).Include(e => e.Student).ToListAsync();
                return ApiResponse<IEnumerable<Enrollment>>.success(listEnrollment);
            }
            catch (Exception e)
            {
                return ApiResponse<IEnumerable<Enrollment>>.fail(null)
                    .WithDescription("Enrollment query error: " + e.Message);
            }
        }

        public async Task<ApiResponse<Enrollment>> AddEnrollment(EnrollmentRequest enrollmentRespone)
        {
            try
            {
                if (enrollmentRespone.Grade == null ||
                    string.IsNullOrWhiteSpace(enrollmentRespone.Grade))
                {
                    return ApiResponse<Enrollment>.fail(null)
                       .WithDescription("Please fill in the information completely");
                }

                var course = await dbContext.Course.FindAsync(enrollmentRespone.CourseID);
                var student = await dbContext.Student.FindAsync(enrollmentRespone.StudentID);
                var enrollment = new Enrollment();

                if (course == null || student == null)
                {
                    return ApiResponse<Enrollment>.fail(null)
                        .WithDescription("Invalid CourseID or StudentID.");
                }


                enrollment.CourseID = enrollmentRespone.CourseID;
                enrollment.StudentID = enrollmentRespone.StudentID;
                enrollment.Grade = enrollmentRespone.Grade;

                await dbContext.Enrollment.AddAsync(enrollment);
                await dbContext.SaveChangesAsync();

                return ApiResponse<Enrollment>.success(enrollment).WithDescription("Enrollment added successfully.");
            }
            catch (Exception e)
            {
                return ApiResponse<Enrollment>.fail(null)
                    .WithDescription("Error adding Enrollment: " + e.Message);
            }
        }


        public async Task<ApiResponse<Enrollment>> UpdateEnrollment(int id, EnrollmentRequest enrollmentRespone)
        {
            try
            {
                if (enrollmentRespone.Grade == null ||
                    string.IsNullOrWhiteSpace(enrollmentRespone.Grade)
                    )
                {
                    return ApiResponse<Enrollment>.fail(null)
                       .WithDescription("Please fill in the information completely");
                }

                var existingEnrollment = await dbContext.Enrollment.FirstOrDefaultAsync(enrollment => enrollment.EnrollmentID == id);
                if (existingEnrollment == null)
                {
                    return ApiResponse<Enrollment>.fail(null).WithDescription("Enrollment not found");
                }

                var courseExists = await dbContext.Course.AnyAsync(course => course.CourseID == enrollmentRespone.CourseID);
                var studentExists = await dbContext.Student.AnyAsync(student => student.ID == enrollmentRespone.StudentID);

                if (!courseExists)
                {
                    return ApiResponse<Enrollment>.fail(null).WithDescription($"CourseID {enrollmentRespone.CourseID} does not exist.");
                }

                if (!studentExists)
                {
                    return ApiResponse<Enrollment>.fail(null).WithDescription($"StudentID {enrollmentRespone.StudentID} does not exist.");
                }



                existingEnrollment.CourseID = enrollmentRespone.CourseID;
                existingEnrollment.StudentID = enrollmentRespone.StudentID;
                existingEnrollment.Grade = enrollmentRespone.Grade;

                await dbContext.SaveChangesAsync();

                var resultEnrollment = await dbContext.Enrollment.Include(e => e.Course).Include(e => e.Student).FirstOrDefaultAsync(enrollment => enrollment.EnrollmentID == id);

                return ApiResponse<Enrollment>.success(resultEnrollment)
                    .WithDescription("Enrollment updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Enrollment>.fail(null)
                    .WithDescription($"Error updating Enrollment: {ex.Message}");
            }
        }


        public async Task<ApiResponse<Enrollment>> DeleteEnrollment(int id)
        {
            try
            {
                // var existingEnrollment = await dbContext.Enrollment.Remove(enrollment => enrollment.EnrollmentID == id);
                var existingEnrollment = await dbContext.Enrollment.FirstOrDefaultAsync(enrollment => enrollment.EnrollmentID == id);
                if (existingEnrollment == null)
                {
                    return ApiResponse<Enrollment>.fail(null).WithDescription("Enrollment not found");
                }

                dbContext.Enrollment.Remove(existingEnrollment); // Remove the enrollment record
                await dbContext.SaveChangesAsync(); // Sa

                return ApiResponse<Enrollment>.success(null)
                    .WithDescription("Enrollment deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Enrollment>.fail(null).WithDescription($"Error deleting Enrollment: {ex.Message}");
            }
        }


    }
}
