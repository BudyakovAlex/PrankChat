using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Providers.UserSession;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    public class OrderedVideoItemViewModel : BaseVideoItemViewModel
    {
        public OrderedVideoItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video) : base(videoManager, userSessionProvider, video)
        {
        }

        public override bool CanPlayVideo => true;

        public override bool CanVoteVideo => true;

        protected override User User => Video.Customer;
    }
}