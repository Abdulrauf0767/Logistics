namespace Logistics.Domain.Entities.RoleEntities
{
    public class RoleEntity
    {
        public int Id { get; private set; }
        public string RoleName { get; private set; } = null!;
        public string RoleDescription { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private RoleEntity() { }
        public RoleEntity(string roleName,string description)
        {
            RoleName = roleName;
            RoleDescription = description;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
