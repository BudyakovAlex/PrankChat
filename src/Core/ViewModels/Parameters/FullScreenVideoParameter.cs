using PrankChat.Mobile.Core.ViewModels.Common.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Parameters
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