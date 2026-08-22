namespace Logistics.Domain.Entities.RoleEntities
{
    public class RoleEntity
    {
        public int Id { get; private set; }
        public string RoleName { get; private set; } = null!;
        public string RoleDescription { get; private set; } = string.Empty;
        private RoleEntity() { }
        public RoleEntity(string roleName,string description)
        {
            RoleName = roleName;
            RoleDescription = description;
        }
    }
}
