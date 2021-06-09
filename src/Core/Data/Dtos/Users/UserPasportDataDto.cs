using Newtonsoft.Json;
using System;

namespace PrankChat.Mobile.Core.Data.Dtos.Users
{
    public class UserPasportDataDto
    {
        public string Phone { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Pasport { get; set; }

        [JsonProperty("pasport_date")]
        public DateTime PasportDate { get; set; }

        public string Nationality { get; set; }
    }
}
