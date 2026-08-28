using Logistics.Application.Features;
using Logistics.Domain.Authorization.Permissions;
using Logistics.Domain.Interfaces.JwtProvider;
using Logistics.Domain.Interfaces.Roles.PermissionsInterface;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Domain.Interfaces.Users;
using Logistics.Infrastructure.Authentication.JwtProviders;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Logistics.Infrastructure.Repositories;
using Logistics.Infrastructure.Repositories.Roles.PermissionRepository;
using Logistics.Infrastructure.Repositories.Roles.RolePermissionRepository;
using Logistics.Infrastructure.Repositories.Roles.RoleRepository;
using Logistics.Infrastructure.Repositories.Users;
using Logistics.Middlewares.ApiKeyMiddlware;
using Logistics.Middlewares.ExceptionHandlingMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero 
    };
});
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// register interfaces
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddApplicationServices();

var app = builder.Build();
// register middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var permissionRepo = services.GetRequiredService<IPermissionRepository>();
        await PermissionSeeder.SeedAsync(permissionRepo);
        await permissionRepo.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        throw new Exception("Permissions generation error",ex);  
    }
}

app.Run();
