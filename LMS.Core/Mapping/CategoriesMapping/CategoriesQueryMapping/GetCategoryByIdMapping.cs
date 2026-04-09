using LMS.Core.Feature.Categories.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CategoriesMapping
{
    public partial class CategoriesProfile
    {
        public void GetCategoryByIdQueryMapping()
        {
            CreateMap<Categories, GetCategoryByIdQueryResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
