using System.ComponentModel.DataAnnotations;


namespace ASP.NET_Core_UI.Models.GeneralModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public bool AreCredentialsInvalid { get; set; }
    }
}
