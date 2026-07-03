using AutoMapper;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            // Entity to Response
            CreateMap<Category, CategoryResponse>();

            // Request to Entity
            CreateMap<CategoryCreateRequest, Category>();
        }
    }
}
