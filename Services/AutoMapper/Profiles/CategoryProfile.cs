﻿using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Services.AutoMapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>().ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<CategoryUpdateDto, Category>().ForMember(dest => dest.ModifiedDate,
                opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Category, CategoryUpdateDto>();
        }
    }
}
