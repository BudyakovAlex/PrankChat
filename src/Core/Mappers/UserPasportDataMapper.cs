using PrankChat.Mobile.Core.Data.Dtos.Users;
using PrankChat.Mobile.Core.Data.Models.User;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class UserPasportDataMapper
    {
        public static UserPasportDataDto Map(this UserPasportData model) =>
            new UserPasportDataDto
            {
                FirstName = model.FirstName,
                Nationality = model.Nationality,
                Pasport = model.Pasport,
                PasportDate = model.PasportDate,
                Patronymic = model.Patronymic,
                Phone = model.Phone,
                Surname = model.Surname
            };
    }
}
