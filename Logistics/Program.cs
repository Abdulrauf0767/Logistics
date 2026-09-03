using Logistics.Application.Features;
using Logistics.Domain.Interfaces.Auth.RoleClaimInterface;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using Logistics.Domain.Interfaces.Auth.UserInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Logistics.Infrastructure.Repositories;
using Logistics.Infrastructure.Repositories.Auth.RoleClaimsRepository;
using Logistics.Infrastructure.Repositories.Auth.RolesRepository;
using Logistics.Infrastructure.Repositories.Auth.UsersRepository;
using Logistics.Infrastructure.Seeders.IdentityDataSeeders;
using Logistics.Middlewares.ApiKeyMiddlware;
using Logistics.Middlewares.ExceptionHandlingMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// register interfaces
builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
builder.Services.AddScoped<IdentityDataSeeder>();
builder.Services.AddApplicationServices();
builder.Services.AddAuthentication();
var app = builder.Build();
// register middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
// register scope for seeding data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seeder = services.GetRequiredService<IdentityDataSeeder>();
        await seeder.SeedDataAsync();
    }catch(Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "seeding error");
    }
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
