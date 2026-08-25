using Logistics.Application.Features;
using Logistics.Domain.Authorization.Permissions;
using Logistics.Domain.Interfaces.Roles.PermissionsInterface;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Logistics.Infrastructure.Repositories;
using Logistics.Infrastructure.Repositories.Roles.PermissionRepository;
using Logistics.Infrastructure.Repositories.Roles.RolePermissionRepository;
using Logistics.Infrastructure.Repositories.Roles.RoleRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler();
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
