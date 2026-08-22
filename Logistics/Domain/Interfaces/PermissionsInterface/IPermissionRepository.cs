using Logistics.Domain.Entities.PermissionEntities;

namespace Logistics.Domain.Interfaces.PermissionsInterface
{
    public interface IPermissionRepository
    {
        // create permissions
        Task AddPermissionsAsync (PermissionEntity permission);
        // get all permissions 
        Task<IEnumerable<PermissionEntity>> GetPermissionsAsync();
        // get by id 
        Task <PermissionEntity?> GetPermissionByIdAsync (int id);
        // to validate all ids in one query 
        Task<int> GetCountByIdAsync(List<int> ids);
        // check existing names
        Task<List<string>> GetExistingNamesAsync();
        // save changes
        Task SaveChangesAsync();
    }
}
