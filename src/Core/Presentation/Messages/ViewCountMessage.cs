using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class ViewCountMessage : MvxMessage
    {
        public int VideoId { get; }
        public long ViewsCount { get; }

        public ViewCountMessage(object sender, int videoId, long viewsCount) : base(sender)
        {
            VideoId = videoId;
            ViewsCount = viewsCount;
        }
    }
}
