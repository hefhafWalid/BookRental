
namespace BookRental.DataAccess.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        BookRentalContext dbContext;

        public BookRentalContext Init()
        {
            return dbContext ?? (dbContext = new BookRentalContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }

}
