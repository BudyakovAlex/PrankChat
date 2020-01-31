using System;
namespace PrankChat.Mobile.Core.Models.Data
{
    public class PeriodDataModel
    {
        public int Hours { get; set; }

        public string Title { get; set; }

        public PeriodDataModel(int hours, string title)
        {
            Hours = hours;
            Title = title;
        }
    }
}
