using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.User.Request
{
    public sealed record RegisterFidoUserRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public long UserId { get; set; }
    }
}
