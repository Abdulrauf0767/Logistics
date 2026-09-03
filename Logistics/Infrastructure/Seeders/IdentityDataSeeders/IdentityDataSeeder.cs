using Logistics.Domain.Entities.Auth.RolesClaim;
using Logistics.Domain.Entities.Auth.RolesEntity;
using Logistics.Domain.Entities.Auth.UsersEntity;
using Logistics.Domain.Entities.Auth.UsersRole;
using Logistics.Domain.Entities.PermissionsEntities;
using Logistics.Domain.Interfaces.Auth.RoleInterface;
using Logistics.Domain.Interfaces.Auth.UserInterface;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;

namespace Logistics.Infrastructure.Seeders.IdentityDataSeeders
{
    public class IdentityDataSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly ILogger<IdentityDataSeeder> _logger;
        public IdentityDataSeeder (IUserRepository userRepository, IRoleRepository roleRepository , IUnitOfWorkRepository unitOfWorkRepository,ILogger<IdentityDataSeeder> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _logger = logger;
        }
        public async Task SeedDataAsync()
        {
           await _unitOfWorkRepository.BeginTransactionAsync();

            try
            {
                string defaultRole = "Super Admin";
                var role = await _roleRepository.ExistsRoleByName(defaultRole);
                if (!role)
                {
                    var newRole = new Role
                    {
                        Name = defaultRole,
                        NormalizedName = defaultRole.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        IsActive = true
                    };
                    await _roleRepository.CreateRoleAsync(newRole);
                    await _unitOfWorkRepository.SaveChangesAsync();
                    var permissions = Permission.GetAllPermissions();
                    foreach (var permission in permissions) {
                        var roleClaim = new RoleClaim
                        {
                            RoleId = newRole.Id,
                            ClaimType = "Permission",
                            ClaimValue = permission
                        };
                        await _roleRepository.AddPermissionToRoleAsync(roleClaim);
                    }
                }
                string adminPhone = "+921111111111";
                string Password = "123456";
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                var existUser = await _userRepository.ExistUserByPhone(adminPhone);
                if (!existUser)
                {
                    var newAdmin = new User
                    {
                        PhoneNumber = adminPhone,
                        PasswordHash = hashPassword,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    };
                    await _userRepository.CreateUserAsync(newAdmin);
                    await _unitOfWorkRepository.SaveChangesAsync();
                    var RoleLinked = new UserRole
                    {
                        UserId = newAdmin.Id,
                        RoleId = 1
                    };
                    await _userRepository.AssignRoleToUserAsync(RoleLinked);
                    await _unitOfWorkRepository.CommitTransactionAsync();
                }
            }
            catch (Exception ex) { 
                await _unitOfWorkRepository.RollbackTransactionAsync();
                _logger.LogError(ex,"error to insert seeding data."); 
            }
        }
    }
}
