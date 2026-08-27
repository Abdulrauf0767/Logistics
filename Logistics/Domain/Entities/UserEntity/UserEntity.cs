namespace Logistics.Domain.Entities.UserEntity
{
    public class UserEntity
    {
        public int Id { get; private set; }
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        private UserEntity() { }
        public UserEntity (string phoneNumber, string password)
        {
            PhoneNumber = phoneNumber;
            Password = password;
            IsActive = true;
        }
    }
}
