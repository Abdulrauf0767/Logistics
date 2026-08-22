using Logistics.Application.Services;
using Logistics.Domain.Authorization.Permissions;
using Logistics.Domain.Interfaces.PermissionsInterface;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Logistics.Infrastructure.Repositories.RoleRepository;
using Logistics.Infrastructure.Repositories.PermissionRepository;
using Microsoft.EntityFrameworkCore;
using Logistics.Domain.Interfaces.RolePermissionsInterface;
using Logistics.Infrastructure.Repositories.RolePermissionRepository;
using Logistics.Application.Services.RolePermissionService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddControllers();
// register interfaces
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<IPermissionRepository,PermissionRepository>();
builder.Services.AddScoped<IRolePermissionsRepository,RolePermissionRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// register services
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<RolePermissionService>();

var app = builder.Build();


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
