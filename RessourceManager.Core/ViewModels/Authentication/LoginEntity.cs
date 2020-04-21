using System.ComponentModel.DataAnnotations;




namespace RessourceManager.Core.ViewModels.Authentication
{
    public class LoginEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public enum LoginResult { WrongCredentials, NotActivated , Success};
}
