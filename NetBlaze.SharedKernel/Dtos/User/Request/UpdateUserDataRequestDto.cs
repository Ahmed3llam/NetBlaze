using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.User.Request
{
    public sealed record UpdateUserDataRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Phone, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InValidPhone))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        [RegularExpression(RegexTemplate.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InValidEmail))]
        public string Email { get; set; }

        public long? ManagerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public long DepartmentId { get; set; }
    }
}
