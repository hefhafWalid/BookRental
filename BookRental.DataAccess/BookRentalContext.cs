using BookRental.DataAccess.Configurations;
using BookRental.DataAccess.Configurations.Migration;
using BookRental.DataModels;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BookRental.DataAccess
{
    public class BookRentalContext : DbContext
    {
        public BookRentalContext()
            : base("BookRental")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookRentalContext, BookRentalContextMigrationConfiguration>());
        }

        #region Entity Sets
        public IDbSet<User> UserSet { get; set; }
        public IDbSet<Role> RoleSet { get; set; }
        public IDbSet<UserRole> UserRoleSet { get; set; }
        public IDbSet<Customer> CustomerSet { get; set; }
        public IDbSet<Book> BookSet { get; set; }
        public IDbSet<Genre> GenreSet { get; set; }
        public IDbSet<Stock> StockSet { get; set; }
        public IDbSet<Rental> RentalSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("dbo");
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new BookConfiguration());
            modelBuilder.Configurations.Add(new GenreConfiguration());
            modelBuilder.Configurations.Add(new StockConfiguration());
            modelBuilder.Configurations.Add(new RentalConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
