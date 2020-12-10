using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class UserUpdateProfileDataModel
    {
        public UserUpdateProfileDataModel(string name,
                                          string email,
                                          string login,
                                          GenderType sex,
                                          string birthday,
                                          string description)
        {
            Name = name;
            Email = email;
            Login = login;
            Sex = sex;
            Birthday = birthday;
            Description = description;
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public GenderType Sex { get; set; }

        public string Birthday { get; set; }

        public string Description { get; set; }
    }
}
