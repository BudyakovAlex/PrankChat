using System;
namespace PrankChat.Mobile.Core.Data.Models.User
{
    public class UserPasportData
    {
        public UserPasportData(
            string phone,
            string surname,
            string firstName,
            string patronymic,
            string pasport,
            DateTime pasportDate,
            string nationality)
        {
            Phone = phone;
            Surname = surname;
            FirstName = firstName;
            Patronymic = patronymic;
            Pasport = pasport;
            PasportDate = pasportDate;
            Nationality = nationality;
        }

        public string Phone { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Pasport { get; set; }

        public DateTime PasportDate { get; set; }

        public string Nationality { get; set; }
    }
}
