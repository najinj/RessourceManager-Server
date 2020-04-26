namespace RessourceManager.Core.ViewModels.Authentication
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword {get;set;}
        public string ResetToken { get; set; }
    }
}
