namespace PrankChat.Mobile.Core.Models.Data
{
    public class AppVersion
    {
        public AppVersion(string link)
        {
            Link = link;
        }

        public string Link { get; set; }
    }
}
