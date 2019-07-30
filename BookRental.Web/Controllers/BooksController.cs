using AutoMapper;
using BookRental.DataAccess.Infrastructure;
using BookRental.DataAccess.Repositories;
using BookRental.DataModels;
using BookRental.Web.Infrastructure.Core;
using BookRental.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace BookRental.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/books")]
    public class BooksController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Book> _booksRepository;

        public BooksController(IEntityBaseRepository<Book> BooksRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _booksRepository = BooksRepository;
        }

        [AllowAnonymous]
        [Route("latest")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var Books = _booksRepository.GetAll().OrderByDescending(m => m.ReleaseDate).Take(6).ToList();

                IEnumerable<BookViewModel> BooksVM = Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(Books);

                response = request.CreateResponse<IEnumerable<BookViewModel>>(HttpStatusCode.OK, BooksVM);

                return response;
            });
        }
    }
}