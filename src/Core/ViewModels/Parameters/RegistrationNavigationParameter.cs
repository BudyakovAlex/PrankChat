namespace PrankChat.Mobile.Core.ViewModels.Parameters
{
    public class RegistrationNavigationParameter
    {
        public RegistrationNavigationParameter(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
