using System;

namespace BookRental.DataAccess.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BookRentalContext Init();
    }
}