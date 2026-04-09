using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {

        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

    }
}
