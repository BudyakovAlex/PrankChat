using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class UserUpdateProfileDataModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public GenderType Sex { get; set; }

        public string Birthday { get; set; }

        public string Description { get; set; }
    }
}
