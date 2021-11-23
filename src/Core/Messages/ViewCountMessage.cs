using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Messages
{
    public class ViewCountMessage : MvxMessage
    {
        public ViewCountMessage(
            object sender,
            int videoId,
            long viewsCount) : base(sender)
        {
            VideoId = videoId;
            ViewsCount = viewsCount;
        }

        public int VideoId { get; }

        public long ViewsCount { get; }
    }
}
