using AutoMapper;

namespace BookRental.Web.Infrastructure.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void ConfigureServices()
        {

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToViewModelMappingProfile());
            });

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);
        }
    }
}