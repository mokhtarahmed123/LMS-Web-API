using MediatR;

namespace LMS.Core.Feature.Categories.Command.Models
{
    public class DeleteCategoryCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
