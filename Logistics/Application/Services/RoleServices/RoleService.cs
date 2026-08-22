using Logistics.Application.DTOS.RoleDTO;
using Logistics.Domain.Entities.RoleEntities;
using Logistics.Domain.Interfaces.RoleInterfaces;
using Logistics.Domain.Interfaces.RolePermissionsInterface;
using Logistics.Domain.Entities.RolePermissionEntities;
using Logistics.Infrastructure.Persistance.ApplicationDbContext; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Logistics.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionsRepository _rolePermissionRepository;
        private readonly ApplicationDbContext _dbContext; 

        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissionsRepository rolePermissionRepository,
            ApplicationDbContext dbContext)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _dbContext = dbContext;
        }

        // CREATE ROLE WITH ATOMIC TRANSACTION & UNIQUE INDEX HANDLING
        public async Task CreateRolesAsync(CreateRoleRequest dto)
        {

            var exists = await _roleRepository.ExistsByName(dto.RoleName);
            if (exists)
            {
                throw new InvalidOperationException($"Role '{dto.RoleName}' already exists.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var newRole = new RoleEntity(dto.RoleName, dto.RoleDescription ?? "");
                await _roleRepository.AddRoleAsync(newRole);

                await _dbContext.SaveChangesAsync();

                var uniquePermissionIds = dto.PermissionIds.Distinct().ToList();

                foreach (var permissionId in uniquePermissionIds)
                {
                    var rolePermission = new RolePermissionEntity(newRole.Id, permissionId);
                    await _rolePermissionRepository.AddRolePermissionsAsync(rolePermission);
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                await transaction.RollbackAsync(); 
                throw new InvalidOperationException("Concurrency Alert: This Role Name or Permission mapping already exists in the system database.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }
    }
}
