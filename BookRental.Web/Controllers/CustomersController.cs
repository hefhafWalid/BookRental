using BookRental.DataAccess.Infrastructure;
using BookRental.DataAccess.Repositories;
using BookRental.DataModels;
using BookRental.Web.Infrastructure.Core;
using System.Web.Mvc;

namespace BookRental.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Customer> _customersRepository;

        public CustomersController(IEntityBaseRepository<Customer> customersRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
        }
    }
}