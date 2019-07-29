using System.Collections.Generic;
using BookRental.DataAccess.Infrastructure;
using BookRental.DataAccess.Repositories;
using BookRental.DataModels;

namespace BookRental.Services
{
    public class MembershipService : IMembershipService
    {
        #region Variables
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Role> _roleRepository;
        private readonly IEntityBaseRepository<UserRole> _userRoleRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        public MembershipService(IEntityBaseRepository<User> userRepository, IEntityBaseRepository<Role> roleRepository,
        IEntityBaseRepository<UserRole> userRoleRepository, IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        public User CreateUser(string username, string email, string password, int[] roles)
        {
            throw new System.NotImplementedException();
        }

        public User GetUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public List<Role> GetUserRoles(string username)
        {
            throw new System.NotImplementedException();
        }

        public MembershipContext ValidateUser(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
