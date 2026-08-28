namespace Logistics.Domain.Entities.UserEntity
{
    public class UserEntity
    {
        public int Id { get; private set; }
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public int RoleId { get; private set; }
        public bool IsActive { get; private set; }
        // Navigation property
        public RoleEntity Role { get; private set; } = null!;
        private UserEntity() { }
        public UserEntity (string phoneNumber, string password, int roleId)
        {
            PhoneNumber = phoneNumber;
            Password = password;
            RoleId = roleId;
            IsActive = true;
        }
    }
}
