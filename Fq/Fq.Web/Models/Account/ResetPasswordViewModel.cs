using System.ComponentModel.DataAnnotations;

namespace Fq.Web.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}