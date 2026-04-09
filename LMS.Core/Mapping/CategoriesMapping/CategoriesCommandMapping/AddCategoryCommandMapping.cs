using LMS.Core.Feature.Categories.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CategoriesMapping
{
    public partial class CategoriesProfile
    {
        public void AddCategoryCommandMapping()
        {
            CreateMap<AddCategoryCommand, Categories>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));


        }
    }
}
