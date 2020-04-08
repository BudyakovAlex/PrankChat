using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure
{
    public static class Constants
    {
        public static class Formats
        {
            public const string DateWithSpace = "dd' : 'hh' : 'mm";
            public const string DateMoreSevenDays = "dd MMMM yyyy";
            public const string DateLessSevenDays = "dd MMMM";
            public const string DateTimeFormat = "dd.MM.yyyy";

            public const string MoneyFormat = "{0:### ### ### ###} ₽";
            public const string NumberFormat = @"{0:\#000#}";
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
            public const int DefaultPaginationSize = 20;
        }

        public static class CompetitionStatuses
        {
            public const string New = "new";
            public const string UploadVideos = "competition_upload_videos";
            public const string Voting = "competition_voting";
            public const string Finished = "finished";
        }

        public static class ComplaintConstants
        {
            public static readonly string[] ProfileCompetitionAims =
            {
                Resources.Complaint_Wrong_Name,
                Resources.Complaint_Unacceptable_Content,
                Resources.Complaint_Unacceptable_Information_In_Profile,
                Resources.Complaint_Other
            };

            public static readonly string[] CommonCompetitionAims =
            {
                Resources.Complaint_Spam,
                Resources.Complaint_Naked_Content,
                Resources.Complaint_Hostile_Utterances,
                Resources.Complaint_Dangerous_Organizations,
                Resources.Complaint_Rights_Violation,
                Resources.Complaint_Cheating,
                Resources.Complaint_Other
            };
        }
    }
}