using BookRental.DataAccess.Infrastructure;
using System.Data.Entity.Migrations;

namespace BookRental.DataAccess.Configurations.Migration
{ 
    public class BookRentalContextMigrationConfiguration : DbMigrationsConfiguration<BookRentalContext>
    {
        public BookRentalContextMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;

        }

#if DEBUG
        protected override void Seed(BookRentalContext context)
        {
            new BookRentalDataSeeder(context).Seed();
        }
#endif

    }
}