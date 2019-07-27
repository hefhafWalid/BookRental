using BookRental.Entities;
using System.Data.Entity.ModelConfiguration;

namespace BookRental.DAL.Configurations
{
    public class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntityBase
    {
        public EntityBaseConfiguration()
        {
            HasKey(e => e.ID);
        }
    }
}
