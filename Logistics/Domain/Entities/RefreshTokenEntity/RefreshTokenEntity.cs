

using Logistics.Domain.Entities.UserEntities;

namespace Logistics.Domain.Entities.RefreshTokenEntity
{
    public class RefreshTokenEntity
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string TokenHash { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string IpAddress { get; private set; } = string.Empty;
        // Navigation Property
        public UserEntity User { get; private set; } = null!;
        private RefreshTokenEntity() { }
        public RefreshTokenEntity (int userid,string tokenHash,DateTime expiresAt,string ipAddress)
        {
            UserId = userid;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            RevokedAt = null;
            IpAddress = ipAddress;
        }

    }
}
