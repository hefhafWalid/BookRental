using BookRental.Web.Models;
using FluentValidation;

namespace BookRental.Web.Infrastructure.Validators
{
    public class BookViewModelValidator : AbstractValidator<BookViewModel>
    {
        public BookViewModelValidator()
        {
            RuleFor(book => book.GenreId).GreaterThan(0)
                .WithMessage("Select a Genre");

            RuleFor(book => book.Director).NotEmpty().Length(1, 100)
                .WithMessage("Select a Director");

            RuleFor(book => book.Writer).NotEmpty().Length(1, 50)
                .WithMessage("Select a writer");

            RuleFor(book => book.Producer).NotEmpty().Length(1, 50)
                .WithMessage("Select a producer");

            RuleFor(book => book.Description).NotEmpty()
                .WithMessage("Select a description");

            RuleFor(book => book.Rating).InclusiveBetween((byte)0, (byte)5)
                .WithMessage("Rating must be less than or equal to 5");

            RuleFor(book => book.TrailerURI).NotEmpty().Must(ValidTrailerURI)
                .WithMessage("Only Youtube Trailers are supported");
        }

        private bool ValidTrailerURI(string trailerURI)
        {
            return (!string.IsNullOrEmpty(trailerURI) && trailerURI.ToLower().StartsWith("https://www.youtube.com/watch?"));
        }
    }
}