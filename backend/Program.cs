using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<TourPlannerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();




builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourLogsRepository, TourLogsRepository>();
builder.Services.AddScoped<ITourLogsService, TourLogsService>();
builder.Services.AddScoped<ITourCoordinateRepository, TourCoordinateRepository>();
builder.Services.AddScoped<ITourCoordinateService, TourCoordinateService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");
//app.UseAuthentication();
app.UseAuthorization();


// app.MapGet("/api/ping", () => Results.Json(new { message = "Hallo aus dem Backend!", timestamp = DateTime.UtcNow }));



app.MapControllers();

app.Run();

