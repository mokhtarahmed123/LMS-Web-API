using AutoMapper;

namespace LMS.Core.Mapping.CategoriesMapping
{
    public partial class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            AddCategoryCommandMapping();
            GetAllCategoriesQueryMapping();
            GetCategoryByIdQueryMapping();
            UpdateCategoryCommandMapping();
            GetAllByFilter();
        }
    }
}
