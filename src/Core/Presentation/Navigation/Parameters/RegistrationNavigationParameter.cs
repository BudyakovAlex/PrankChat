namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
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
