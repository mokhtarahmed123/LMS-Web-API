using MediatR;

namespace LMS.Core.Feature.Coupons.Command.Models
{
    public class DeleteCouponCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCouponCommand(int Id)
        {
            this.Id = Id;
        }
    }
}
