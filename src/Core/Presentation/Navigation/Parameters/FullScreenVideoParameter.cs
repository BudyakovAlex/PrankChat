using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class FullScreenVideoParameter
    {
        public FullScreenVideoParameter(FullScreenVideoDataModel video) : this(new List<FullScreenVideoDataModel> { video }, 0)
        {
        }

        public FullScreenVideoParameter(List<FullScreenVideoDataModel> videos, int index)
        {
            Videos = videos;
            Index = index;
        }

        public List<FullScreenVideoDataModel> Videos { get; }

        public int Index { get; }
    }
}