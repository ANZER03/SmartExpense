using AutoMapper;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategoryColor, opt => opt.MapFrom(src => src.Category.Color));
            
        CreateMap<CreateExpenseDto, Expense>();
    }
}
