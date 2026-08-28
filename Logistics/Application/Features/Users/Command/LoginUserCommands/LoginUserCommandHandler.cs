using Logistics.Domain.Interfaces.JwtProvider;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Interfaces.Roles.RolePermissionInterface;
using Logistics.Domain.Interfaces.Users;
using MediatR;

namespace Logistics.Application.Features.Users.Command.LoginUserCommands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IJwtProvider jwtProvider,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByPhoneAsync(request.PhoneNumber);
            if (user == null || !user.IsActive)
                throw new BadHttpRequestException("Invalid phone number or password.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
                throw new BadHttpRequestException("Invalid phone number or password.");

            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            if (role == null || !role.IsActive)
                throw new BadHttpRequestException("Role assigned to this user is invalid or inactive.");

            var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(user.RoleId);
            var mappingPermissions = rolePermissions
                .Select(rp => new MappingPermissions
                {
                    PermissionId = rp.PermissionId,
                    PermissionName = rp.Permission.Name
                })
                .ToList();

            var token = _jwtProvider.GenerateToken(user.Id, role.Name, mappingPermissions);
            var expireMinutes = int.Parse(_configuration["JwtSettings:ExpireInMinutes"]!);

            return new LoginUserResponse(token, DateTime.UtcNow.AddMinutes(expireMinutes));
        }
    }
}
