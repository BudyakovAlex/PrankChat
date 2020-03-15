namespace PrankChat.Mobile.Core.Infrastructure
{
    public static class Constants
    {
        public static class Formats
        {
            public const string DateWithSpace = "dd' : 'hh' : 'mm";
            public const string DateMoreSevenDays = "dd MMMM yyyy";
            public const string DateLessSevenDays = "dd MMMM";
        }

        public static class Delays
        {
            public const int ViewedFactRegistrationDelayInMilliseconds = 3000;
            public const int RepeatDelayInSeconds = 10;
        }

        public static class Age
        {
            public const int AdultAge = 18;
        }

        public static class Keys
        {
            public const string MuteStateKey = nameof(MuteStateKey);
        }

        public static class Pagination
        {
            public const int DefaultPaginationSize = 100;
        }
    }
}