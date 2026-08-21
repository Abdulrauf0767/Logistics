using Logistics.Application.Services;
using Logistics.Domain.Authorization.Permissions;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Infrastructure.Persistance.ApplicationDbContext;
using Logistics.Infrastructure.Repositories.RoleRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddControllers();
// register interfaces
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// register services
builder.Services.AddScoped<RoleService>();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await PermissionSeeder.SeedAsync(dbContext);
}

app.Run();
