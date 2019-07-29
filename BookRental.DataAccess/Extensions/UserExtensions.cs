using BookRental.DataAccess.Repositories;
using BookRental.DataModels;
using System.Linq;

namespace BookRental.DataAccess.Extensions
{
    public static class UserExtensions
    {
        public static User GetSingleByUsername(this IEntityBaseRepository<User> userRepository, string username)
        {
            return userRepository.GetAll().FirstOrDefault(x => x.Username == username);
        }
    }
}
