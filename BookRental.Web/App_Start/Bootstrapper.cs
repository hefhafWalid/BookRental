using BookRental.Web.Infrastructure.Mappings;
using System.Web.Http;

namespace BookRental.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            // Configure Autofac
            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
            //Configure AutoMapper
            AutoMapperConfiguration.ConfigureServices();
        }
    }
}