using PrankChat.Mobile.Core.Localization;

namespace PrankChat.Mobile.Core.Common
{
    public static class Constants
    {
        public static class Configuration
        {
            public const string AppConfigFileName = "AppConfig.json";
        }

        public static class Profile
        {
            public const int DescriptionMaxLength = 120;
            public const int CheckCanSendEmailInterval = 60000;
            public const int UnlockResendMinutes = 3;
        }

        public static class Orders
        {
            public const int DescriptionMaxLength = 1000;
        }

        public static class Cashbox
        {
            public const double PaymentServceCommissionMultiplier = 0.028;
        }

        public static class Formats
        {
            public const string DateWithSpace = "dd' : 'hh' : 'mm";
            public const string DateMoreSevenDays = "dd MMMM yyyy";
            public const string DateLessSevenDays = "dd MMMM";
            public const string DateTimeFormat = "dd.MM.yyyy";

            public const string MoneyFormat = "{0:### ### ### ###} ₽";
            public const string NumberFormat = @"{0:\#000#}";
            public const string DownloadVideoDateTimeFormat = "MM_dd_yyyy";
        }

        public static class File
        {
            public const string DownloadVideoFormat = ".mp4";
            public const long MaximumFileSizeInMegabytes = 1024 * 1024 * 150;
        }

        public static class Delays
        {
            public const int VideoPartiallyPlayedDelay = 3;
            public const int MillisecondsDelayBeforeMarkAsReaded = 3000;
            public const int RepeatDelayInSeconds = 10;
            public const int DebounceDelay = 500;
        }

        public static class Age
        {
            public const int AdultAge = 18;
        }

        public static class Keys
        {
            public const string MuteStateKey = nameof(MuteStateKey);
            public const string IsOnBoardingShown = nameof(IsOnBoardingShown);
            public const string User = nameof(User);
        }

        public static class Notification
        {
            public const string ChannelId = "classtag_notification_channel";
        }
        
        public static class Pagination
        {
            public const int DefaultPaginationSize = 20;
        }

        public static class ErrorCodes
        {
            public const string LowBalance = "XA0001";
            public const string Unauthorized = "unauthorized";
        }

        public static class ComplaintConstants
        {
            public static readonly string[] ProfileCompetitionAims =
            {
                Resources.UsesSomeoneElseName,
                Resources.InappropriateContent,
                Resources.InappropriateProfileInformation,
                Resources.Other
            };

            public static readonly string[] CommonCompetitionAims =
            {
                Resources.ThisIsSpam,
                Resources.PornographyAndNudity,
                Resources.InsultingOrBullying,
                Resources.ViolenceOrDangerousOrganizations,
                Resources.CopyrighInfringement,
                Resources.FraudOrDeception,
                Resources.Other
            };
        }
    }
}