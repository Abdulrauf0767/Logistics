using System.ComponentModel.DataAnnotations;

namespace Logistics.Domain.Entities.PermissionEntities
{
    public class PermissionEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; internal set; }

        private PermissionEntity() { }

        public PermissionEntity(string name, string description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }

}
