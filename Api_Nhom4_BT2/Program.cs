using Microsoft.EntityFrameworkCore;
using Api_Nhom4_BT2.Models;
using Api_Nhom4_BT2.DBContext;
using Api_Nhom4_BT2.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Tải biến môi trường từ file .env
Env.Load();

// Lấy chuỗi kết nối từ biến môi trường
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Nếu chuỗi kết nối không được thiết lập, ném ra ngoại lệ
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Chuỗi kết nối không được thiết lập.");
}
// Cấu hình DbContext với chuỗi kết nối
builder.Services.AddDbContext<ApplicationDbContext>(option => 
    option.UseNpgsql(connectionString));

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<EnrollmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
