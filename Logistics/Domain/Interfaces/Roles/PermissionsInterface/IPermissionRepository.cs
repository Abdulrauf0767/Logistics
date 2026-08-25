using Logistics.Domain.Entities.PermissionEntities;

namespace Logistics.Domain.Interfaces.Roles.PermissionsInterface
{
    public interface IPermissionRepository
    {
        // create permissions
        Task AddPermissionsAsync (PermissionEntity permission);
        // to validate all ids in one query 
        Task<int> GetCountByIdAsync(List<int> ids);
        // check existing names
        Task<List<string>> GetExistingNamesAsync();
        // save changes
        Task SaveChangesAsync();
    }
}
