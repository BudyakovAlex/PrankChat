using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class UserRegistrationDataModel
    {
        public UserRegistrationDataModel(string name,
                                         string email,
                                         string login,
                                         DateTime? birthday,
                                         GenderType? sex,
                                         string password,
                                         string passwordConfirmation)
        {
            Name = name;
            Email = email;
            Login = login;
            Birthday = birthday;
            Sex = sex;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }

        public string Name { get; }

        public string Email { get; }

        public string Login { get; }

        public DateTime? Birthday { get; }

        public GenderType? Sex { get; }

        public string Password { get; }

        public string PasswordConfirmation { get; }
    }
}
