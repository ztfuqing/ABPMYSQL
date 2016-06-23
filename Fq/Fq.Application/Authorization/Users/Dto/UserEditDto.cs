using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;

namespace Fq.Authorization.Users.Dto
{
    public class UserEditDto : IValidate, IPassivable
    {
        public long? Id { get; set; }

        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        public long? OrgId { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        [StringLength(15)]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }
    }
}