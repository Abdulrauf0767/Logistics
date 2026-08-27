namespace Logistics.Domain.Entities
{
    public class RoleEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public  bool IsActive { get; private set; }
        private RoleEntity() { }
        public RoleEntity(string name, string desciption)
        {
            Name = name;
            Description = desciption;
            IsActive = true;

        }
        public void Update(string name, string description,bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}