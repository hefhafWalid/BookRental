using AutoMapper;
using BookRental.DataModels;
using BookRental.Web.Models;
using System.Linq;

namespace BookRental.Web.Infrastructure.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        public DomainToViewModelMappingProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(vm => vm.Genre, map => map.MapFrom(m => m.Genre.Name))
                .ForMember(vm => vm.GenreId, map => map.MapFrom(m => m.Genre.ID))
                .ForMember(vm => vm.IsAvailable, map => map.MapFrom(m => m.Stocks.Any(s => s.IsAvailable)))
                .ForMember(vm => vm.NumberOfStocks, map => map.MapFrom(m => m.Stocks.Count))
                .ForMember(vm => vm.Image, map => map.MapFrom(m => string.IsNullOrEmpty(m.Image) == true ? "unknown.jpg" : m.Image));

            CreateMap<Genre, GenreViewModel>()
                .ForMember(vm => vm.NumberOfBooks, map => map.MapFrom(g => g.Books.Count()));

            CreateMap<Customer, CustomerViewModel>();

            CreateMap<Stock, StockViewModel>();

            CreateMap<Rental, RentalViewModel>();
        }
    }
}