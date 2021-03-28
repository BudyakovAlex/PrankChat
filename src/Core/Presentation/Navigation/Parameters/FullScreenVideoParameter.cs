using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class FullScreenVideoParameter
    {
        public FullScreenVideoParameter(BaseVideoItemViewModel video) : this(new [] { video }, 0)
        {
        }

        public FullScreenVideoParameter(BaseVideoItemViewModel[] videos, int index)
        {
            Videos = videos;
            StartIndex = index;
        }

        public BaseVideoItemViewModel[] Videos { get; }

        public int StartIndex { get; }
    }
}