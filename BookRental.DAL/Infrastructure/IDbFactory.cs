using System;

namespace BookRental.DAL.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BookRentalContext Init();
    }
}