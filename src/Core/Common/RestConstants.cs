namespace PrankChat.Mobile.Core.Common
{
    public class RestConstants
    {
        public const string AllOrders = "newsline/orders/all";
        public const string NewOrders = "newsline/orders/new";
        public const string InWork = "newsline/orders/in_work";
        public const string InWaiting = "newsline/orders/in_waiting";
        public const string NewVideos = "newsline/videos/new";
        public const string ProfileOwnOrders = "profile/orders/own";
        public const string ProfileOwnOrdersInExecute = "profile/orders/execute";
        public const string PolicyEndpoint = "https://onplaysite.com/policy";
        public const string StorePassportDataResource = "me/requisites";

        public const string YoomoneyResourceUrl = "https://yoomoney.ru/";
        public const string ComplaintUserResourceTemplate = "users/{0}/complaint";

        public const string MyDevice = "me/device";
        public const string Notifications = "notifications";
        public const string NotificationsRead = "notifications/read";
        public const string NotificationsUnreaded = "notifications/undelivered";

        public const string MyExecuteCompetitonsResource = "profile/competitions/execute";
        public const string MyOrderedCompetitionsResource = "profile/competitions/own";

        public const string AuthorizationCookieKey = "Authorization";
        public const string AuthorizationCookieValueTemplate = "Bearer {0}";
        public const string AcceptLanguageCookieKey = "Accept-Language";
        public const string UrlStringTemplate = "{0}/api/v{1}/videos";
        public const string ContentTypeCookieValueTemplate = "multipart/form-data; boundary=\"{0}\"";
        public const string ContentTypeKey = "Content-Type";
        public const string DefaultBoundary = "----PrankChatBoundary7MA4YWxkTrZu0gW";
        public const string MultipartSuffixTemplate = "form-data; name=\"{0}\"";
        public const string ContentTypeAppJson = "application/json";
        public const string ContentDespositionKey = "Content-Disposition";
        public const string DispositionTypeFormData = "form-data";
        public const string NewLine = "\r\n";
        public readonly static string ContentDispositionDefaultTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; name=""{{0}}""{NewLine}";
        public readonly static string ContentDispositionFileTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; filename={{0}}; name={{1}}{NewLine}";
    }
}
