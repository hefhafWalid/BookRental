﻿using AutoMapper;
using BookRental.DataAccess.Infrastructure;
using BookRental.DataAccess.Repositories;
using BookRental.DataModels;
using BookRental.Web.Infrastructure.Core;
using BookRental.Web.Infrastructure.Extensions;
using BookRental.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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


        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var book = _booksRepository.GetSingle(id);

                BookViewModel bookVM = Mapper.Map<Book, BookViewModel>(book);

                response = request.CreateResponse<BookViewModel>(HttpStatusCode.OK, bookVM);

                return response;
            });
        }


        [AllowAnonymous]
        [Route("{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Book> books = null;
                int totalBooks = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    books = _booksRepository.GetAll()
                        .OrderBy(m => m.ID)
                        .Where(m => m.Title.ToLower()
                        .Contains(filter.ToLower().Trim()))
                        .ToList();
                }
                else
                {
                    books = _booksRepository.GetAll().ToList();
                }

                totalBooks = books.Count();
                books = books.Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                IEnumerable<BookViewModel> booksVM = Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books);

                PaginationSet<BookViewModel> pagedSet = new PaginationSet<BookViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalBooks,
                    TotalPages = (int)Math.Ceiling((decimal)totalBooks / currentPageSize),
                    Items = booksVM
                };

                response = request.CreateResponse<PaginationSet<BookViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }


        //[MimeMultipart]
        //[Route("images/upload")]
        //[HttpPost]
        //public HttpResponseMessage Post(HttpRequestMessage request, int bookId)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        var bookOld = _booksRepository.GetSingle(bookId);
        //        if (bookOld == null)
        //            response = Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
        //        else
        //        {
        //            var uploadPath = HttpContext.Current.Server.MapPath("~/Content/images/books");

        //            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

        //            // Read the MIME multipart asynchronously 
        //            Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

        //            string _localFileName = multipartFormDataStreamProvider
        //                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

        //            // Create response
        //            FileUploadResult fileUploadResult = new FileUploadResult
        //            {
        //                LocalFilePath = _localFileName,

        //                FileName = Path.GetFileName(_localFileName),

        //                FileLength = new FileInfo(_localFileName).Length
        //            };

        //            // update database
        //            bookOld.Image = fileUploadResult.FileName;
        //            _booksRepository.Edit(bookOld);
        //            _unitOfWork.Commit();

        //            response = request.CreateResponse(HttpStatusCode.OK, fileUploadResult);
        //        }

        //        return response;
        //    });
        //}

        public async Task Post()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

            //access form data
            NameValueCollection formData = provider.FormData;

            //access files
            IList<HttpContent> files = provider.Files;

            //Example: reading a file's stream like below
            HttpContent file1 = files[0];
            Stream file1Stream = await file1.ReadAsStreamAsync();
        }






        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, BookViewModel book)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    Book newBook = new Book();
                    newBook.UpdateBook(book);

                    for (int i = 0; i < book.NumberOfStocks; i++)
                    {
                        Stock stock = new Stock()
                        {
                            IsAvailable = true,
                            Book = newBook,
                            UniqueKey = Guid.NewGuid()
                        };
                        newBook.Stocks.Add(stock);
                    }

                    _booksRepository.Add(newBook);

                    _unitOfWork.Commit();

                    // Update view model
                    book = Mapper.Map<Book, BookViewModel>(newBook);
                    response = request.CreateResponse<BookViewModel>(HttpStatusCode.Created, book);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, BookViewModel book)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var bookDb = _booksRepository.GetSingle(book.ID);
                    if (bookDb == null)
                        response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid book.");
                    else
                    {
                        bookDb.UpdateBook(book);
                        book.Image = bookDb.Image;
                        _booksRepository.Edit(bookDb);

                        _unitOfWork.Commit();
                        response = request.CreateResponse<BookViewModel>(HttpStatusCode.OK, book);
                    }
                }

                return response;
            });
        }
    }
}