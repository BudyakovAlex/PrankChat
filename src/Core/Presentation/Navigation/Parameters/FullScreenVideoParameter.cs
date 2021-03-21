using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class FullScreenVideoParameter
    {
        public FullScreenVideoParameter(FullScreenVideo video) : this(new List<FullScreenVideo> { video }, 0)
        {
        }

        public FullScreenVideoParameter(List<FullScreenVideo> videos, int index)
        {
            Videos = videos;
            Index = index;
        }

        public List<FullScreenVideo> Videos { get; }

        public int Index { get; }
    }
}