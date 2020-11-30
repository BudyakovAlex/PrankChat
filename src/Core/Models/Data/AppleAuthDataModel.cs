namespace PrankChat.Mobile.Core.Models.Data
{
    public class AppleAuthDataModel
    {
        public AppleAuthDataModel(string username,
                                  string email,
                                  string identityToken,
                                  string token,
                                  string password)
        {
            UserName = username;
            Email = email;
            IdentityToken = identityToken;
            Token = token;
            Password = password;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string IdentityToken { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}