using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToDoList;
using ToDoList.Services;
using ToDoList.ORM;

var builder = WebApplication.CreateBuilder(args);

// Add services
string connectionString = "Server=localhost\\SQLEXPRESS;Database=ToDoWork;Integrated Security=True;TrustServerCertificate=True";
builder.Services.AddSingleton<IOrm>(new EFOrm(connectionString));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<FriendRequestService>();
builder.Services.AddSingleton<WorkService>();
builder.Services.AddSingleton<FriendShipService>();

var jwtService = new JwtService();
builder.Services.AddSingleton(jwtService);

// Enable controllers
builder.Services.AddControllers();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = jwtService.GetValidationParameters();
    });

builder.Services.AddAuthorization();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkAssignment API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = new ApplicationContext(connectionString);
//dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
