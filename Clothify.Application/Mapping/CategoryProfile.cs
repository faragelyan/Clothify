using AutoMapper;
using Clothify.Application.DTOs.Category;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
        }
    }
}
