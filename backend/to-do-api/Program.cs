using Microsoft.EntityFrameworkCore;
using to_do_api.IRepository;
using to_do_api.Mappers;
using to_do_api.Models;
using to_do_api.Repository;

var builder = WebApplication.CreateBuilder(args);

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// Add AutoMapper 
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure SQL Server (Ensure connection string is correct in appsettings)
builder.Services.AddDbContext<TodoAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<ITaskRepository, TaskRepository>(); 

// Register controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use cors
app.UseCors("AllowSpecificOrigin");

// Map controller routes
app.MapControllers();  // Ensure this is correctly placed

app.Run();
