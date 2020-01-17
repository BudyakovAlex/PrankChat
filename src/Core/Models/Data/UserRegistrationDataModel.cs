using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class UserRegistrationDataModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public DateTime Birthday { get; set; }

        public GenderType Sex { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }
    }
}
