namespace PrankChat.Mobile.Core.Data.Models
{
    public sealed class InviteFriendResponse
    {
        private InviteFriendResponse(string message, bool isSuccessful)
        {
            Message = message;
            IsSuccessful = isSuccessful;
        }

        public string Message { get; }

        public bool IsSuccessful { get; }

        public static InviteFriendResponse Successful() =>
            new InviteFriendResponse(null, true);

        public static InviteFriendResponse Unsuccessful(string message) =>
            new InviteFriendResponse(message, false);
    }
}
