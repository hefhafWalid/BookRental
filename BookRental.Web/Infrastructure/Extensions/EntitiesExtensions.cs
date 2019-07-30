using BookRental.DataModels;
using BookRental.Web.Models;
using System;

namespace BookRental.Web.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {
        public static void UpdateCustomer(this Customer customer, CustomerViewModel customerVm)
        {
            customer.FirstName = customerVm.FirstName;
            customer.LastName = customerVm.LastName;
            customer.IdentityCard = customerVm.IdentityCard;
            customer.Mobile = customerVm.Mobile;
            customer.DateOfBirth = customerVm.DateOfBirth;
            customer.Email = customerVm.Email;
            customer.UniqueKey = (customerVm.UniqueKey == null || customerVm.UniqueKey == Guid.Empty)
                ? Guid.NewGuid() : customerVm.UniqueKey;
            customer.RegistrationDate = (customer.RegistrationDate == DateTime.MinValue ? DateTime.Now : customerVm.RegistrationDate);
        }

        public static void UpdateBook(this Book book, BookViewModel bookVm)
        {
            book.Title = bookVm.Title;
            book.Description = bookVm.Description;
            book.GenreId = bookVm.GenreId;
            book.Director = bookVm.Director;
            book.Writer = bookVm.Writer;
            book.Producer = bookVm.Producer;
            book.Rating = bookVm.Rating;
            book.TrailerURI = bookVm.TrailerURI;
            book.ReleaseDate = bookVm.ReleaseDate;
        }
    }
}