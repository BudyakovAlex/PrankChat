using System;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class RegistrationNavigationParameter : INavigationParameter
    {
        public string Email { get; }

        public RegistrationNavigationParameter(string email)
        {
            Email = email;
        }
    }
}
