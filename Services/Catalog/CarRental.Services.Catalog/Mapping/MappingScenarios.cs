using AutoMapper;
using CarRental.Services.Catalog.Dtos;
using CarRental.Services.Catalog.Models;

namespace CarRental.Services.Catalog.Mapping
{
    public class MappingScenarios:Profile
    {
        public MappingScenarios()
        {
            CreateMap<CarRental.Services.Catalog.Models.Car, CarDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Feature   ,FeatureDto>().ReverseMap();
            CreateMap<CarRental.Services.Catalog.Models.Car, CarDto>().ReverseMap();
            CreateMap<CarRental.Services.Catalog.Models.Car, CarUpdatedDto>().ReverseMap();
            CreateMap<Category, CreatedCategoryDto>().ReverseMap();
        }
    }
}
