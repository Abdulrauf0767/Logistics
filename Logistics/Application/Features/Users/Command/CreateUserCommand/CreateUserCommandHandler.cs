using MediatR;
using Logistics.Application.Features.Users.Command.CreateUserCommand;
using Logistics.Domain.Interfaces.Users;
using Logistics.Domain.Interfaces.UnitOfWorkInterface;
using Logistics.Domain.Interfaces.Roles.RoleInterface;
using Logistics.Domain.Entities.UserEntity;
namespace Logistics.Application.Features.Users.Command.CreateUserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IRoleRepository _roleRepository;
        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWorkRepository unitOfWorkRepository,IRoleRepository roleRepository) { 
            _userRepository = userRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _roleRepository = roleRepository;
        }
        public async Task<int> Handle (CreateUserCommand request , CancellationToken cancellationToken)
        {
            var roleExists = await _roleRepository.GetByIdAsync(request.RoleId);
            if (roleExists == null)
            {
                throw new BadHttpRequestException("Invalid role or role not found.");
            }
            if (roleExists.Name.Equals("Super Admin" ,  StringComparison.OrdinalIgnoreCase))
            {
                bool isAdminUserAlreadyExists = await _userRepository.IsUserExistsByRoleId(request.RoleId);

                if (isAdminUserAlreadyExists)
                {
                    throw new BadHttpRequestException("System already has this role and only this role could assign only one person!");
                }
            }
            var phoneExists = await _userRepository.IsPhoneExists(request.PhoneNumber);
            if (phoneExists)
            {
                throw new BadHttpRequestException("This phone number already exists");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var newUser = new UserEntity(request.PhoneNumber,hashedPassword,request.RoleId);
                await _userRepository.CreateUserAsync(newUser);
                await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
                return newUser.Id;
            }
            catch
            {
                await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
