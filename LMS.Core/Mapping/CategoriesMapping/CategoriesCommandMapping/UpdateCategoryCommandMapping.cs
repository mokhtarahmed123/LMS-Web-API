using LMS.Core.Feature.Categories.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CategoriesMapping
{
    public partial class CategoriesProfile
    {
        public void UpdateCategoryCommandMapping()
        {
            CreateMap<UpdateCategoryCommand, Categories>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
