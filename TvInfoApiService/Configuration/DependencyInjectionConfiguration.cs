using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TvInfo.Persistence.Interfaces;
using TvInfo.Persistence.Models;
using TvInfo.Persistence.Models.DTO;
using TvInfo.Persistence.Repositories;

namespace TvInfoApiService.Configuration
{
    internal static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IShowRepository, ShowRepository>();
            services.AddTransient<IShowContext, ShowContext>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShowDto, Show>()
                    .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.Cast));
                cfg.CreateMap<Cast, PersonDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Person.Id))
                    .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Person.Birthdate))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Person.Name));
            }); 

            services.AddSingleton<IMapper, IMapper>(sp => mappingConfig.CreateMapper());
        }
    }
}