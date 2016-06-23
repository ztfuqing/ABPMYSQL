using System.ComponentModel.DataAnnotations;

namespace Fq.Web.Models.Account
{
    public class ResetPasswordFormViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ResetCode { get; set; }

        [Required]
        public string Password { get; set; }
    }
}