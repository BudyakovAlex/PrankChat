namespace PrankChat.Mobile.Core.Models.Data
{
    public class Period
    {
        public Period(int hours, string title)
        {
            Hours = hours;
            Title = title;
        }

        public int Hours { get; set; }

        public string Title { get; set; }
    }
}
