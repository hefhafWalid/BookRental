using AutoMapper;
using BookRental.DataAccess.Infrastructure;
using BookRental.DataAccess.Repositories;
using BookRental.DataModels;
using BookRental.Web.Infrastructure.Core;
using BookRental.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace BookRental.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/rentals")]
    public class RentalsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Rental> _rentalsRepository;
        private readonly IEntityBaseRepository<Customer> _customersRepository;
        private readonly IEntityBaseRepository<Stock> _stocksRepository;
        private readonly IEntityBaseRepository<Book> _booksRepository;

        public RentalsController(IEntityBaseRepository<Rental> rentalsRepository,
            IEntityBaseRepository<Customer> customersRepository, IEntityBaseRepository<Book> booksRepository,
            IEntityBaseRepository<Stock> stocksRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _rentalsRepository = rentalsRepository;
            _booksRepository = booksRepository;
            _customersRepository = customersRepository;
            _stocksRepository = stocksRepository;
        }

        private List<RentalHistoryViewModel> GetBookRentalHistory(int bookId)
        {
            List<RentalHistoryViewModel> _rentalHistory = new List<RentalHistoryViewModel>();
            List<Rental> rentals = new List<Rental>();

            var book = _booksRepository.GetSingle(bookId);

            foreach (var stock in book.Stocks)
            {
                rentals.AddRange(stock.Rentals);
            }

            foreach (var rental in rentals)
            {
                RentalHistoryViewModel _historyItem = new RentalHistoryViewModel()
                {
                    ID = rental.ID,
                    StockId = rental.StockId,
                    RentalDate = rental.RentalDate,
                    ReturnedDate = rental.ReturnedDate.HasValue ? rental.ReturnedDate : null,
                    Status = rental.Status
                    //Customer = _customersRepository.GetCustomerFullName(rental.CustomerId)
                };

                _rentalHistory.Add(_historyItem);
            }

            _rentalHistory.Sort((r1, r2) => r2.RentalDate.CompareTo(r1.RentalDate));

            return _rentalHistory;
        }

        //Request would be in the following form: api/rentals/4/rentalhistory
        [HttpGet]
        [Route("{id:int}/rentalhistory")]
        public HttpResponseMessage RentalHistory(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                List<RentalHistoryViewModel> _rentalHistory = GetBookRentalHistory(id);

                response = request.CreateResponse<List<RentalHistoryViewModel>>(HttpStatusCode.OK, _rentalHistory);

                return response;
            });
        }

        [HttpPost]
        [Route("return/{rentalId:int}")]
        public HttpResponseMessage Return(HttpRequestMessage request, int rentalId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var rental = _rentalsRepository.GetSingle(rentalId);

                if (rental == null)
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid rental");
                else
                {
                    rental.Status = "Returned";
                    rental.Stock.IsAvailable = true;
                    rental.ReturnedDate = DateTime.Now;

                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("rent/{customerId:int}/{stockId:int}")]
        public HttpResponseMessage Rent(HttpRequestMessage request, int customerId, int stockId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var customer = _customersRepository.GetSingle(customerId);
                var stock = _stocksRepository.GetSingle(stockId);

                if (customer == null || stock == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Customer or Stock");
                }
                else
                {
                    if (stock.IsAvailable)
                    {
                        Rental _rental = new Rental()
                        {
                            CustomerId = customerId,
                            StockId = stockId,
                            RentalDate = DateTime.Now,
                            Status = "Borrowed"
                        };

                        _rentalsRepository.Add(_rental);

                        stock.IsAvailable = false;

                        _unitOfWork.Commit();

                        RentalViewModel rentalVm = Mapper.Map<Rental, RentalViewModel>(_rental);

                        response = request.CreateResponse<RentalViewModel>(HttpStatusCode.Created, rentalVm);
                    }
                    else
                        response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Selected stock is not available anymore");
                }

                return response;
            });
        }


        private List<RentalHistoryPerDate> GetBookRentalHistoryPerDates(int bookId)
        {
            List<RentalHistoryPerDate> listHistory = new List<RentalHistoryPerDate>();
            List<RentalHistoryViewModel> _rentalHistory = GetBookRentalHistory(bookId);
            if (_rentalHistory.Count > 0)
            {
                List<DateTime> _distinctDates = new List<DateTime>();
                _distinctDates = _rentalHistory.Select(h => h.RentalDate.Date).Distinct().ToList();

                foreach (var distinctDate in _distinctDates)
                {
                    var totalDateRentals = _rentalHistory.Count(r => r.RentalDate.Date == distinctDate);
                    RentalHistoryPerDate _bookRentalHistoryPerDate = new RentalHistoryPerDate()
                    {
                        Date = distinctDate,
                        TotalRentals = totalDateRentals
                    };

                    listHistory.Add(_bookRentalHistoryPerDate);
                }

                listHistory.Sort((r1, r2) => r1.Date.CompareTo(r2.Date));
            }

            return listHistory;
        }


        [HttpGet]
        [Route("rentalhistory")]
        public HttpResponseMessage TotalRentalHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                List<TotalRentalHistoryViewModel> _totalMoviesRentalHistory = new List<TotalRentalHistoryViewModel>();

                var books = _booksRepository.GetAll();

                foreach (var book in books)
                {
                    TotalRentalHistoryViewModel _totalRentalHistory = new TotalRentalHistoryViewModel()
                    {
                        ID = book.ID,
                        Title = book.Title,
                        Image = book.Image,
                        Rentals = GetBookRentalHistoryPerDates(book.ID)
                    };

                    if (_totalRentalHistory.TotalRentals > 0)
                        _totalMoviesRentalHistory.Add(_totalRentalHistory);
                }

                response = request.CreateResponse<List<TotalRentalHistoryViewModel>>(HttpStatusCode.OK, _totalMoviesRentalHistory);

                return response;
            });
        }
    }
}