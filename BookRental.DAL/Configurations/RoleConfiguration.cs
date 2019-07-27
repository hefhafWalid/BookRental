using BookRental.Entities;

namespace BookRental.DAL.Configurations
{
    class RoleConfiguration : EntityBaseConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(ur => ur.Name).IsRequired().HasMaxLength(50);
        }
    }
}
