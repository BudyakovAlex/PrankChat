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

        public static class CompetitionStatuses
        {
            public const string New = "new";
            public const string UploadVideos = "competition_upload_videos";
            public const string Voting = "competition_voting";
            public const string Finished = "finished";
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
                Resources.ComplaintWrongName,
                Resources.ComplaintUnacceptableContent,
                Resources.ComplaintUnacceptableInformationInProfile,
                Resources.ComplaintOther
            };

            public static readonly string[] CommonCompetitionAims =
            {
                Resources.ComplaintSpam,
                Resources.ComplaintNakedContent,
                Resources.ComplaintHostileUtterances,
                Resources.ComplaintDangerousOrganizations,
                Resources.ComplaintRightsViolation,
                Resources.ComplaintCheating,
                Resources.ComplaintOther
            };
        }
    }
}