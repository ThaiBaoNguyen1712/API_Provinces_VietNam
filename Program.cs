using API_Province_VietNam.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình DbContext với thời gian chờ là 300 giây (5 phút)
builder.Services.AddDbContext<ProvincesVietNamContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Chuỗi kết nối trong appsettings.json
        sqlServerOptions =>
        {
            sqlServerOptions.CommandTimeout(600); // Thời gian chờ là 600 giây
        }
    )
);
// Cấu hình CORS để cho phép tất cả các nguồn gốc
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Sử dụng CORS
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
