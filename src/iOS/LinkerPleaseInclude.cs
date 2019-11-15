using MvvmCross;
using UIKit;

namespace PrankChat.Mobile.iOS
{
    [Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(UIBarButtonItem button)
        {
            button.Clicked += (s, e) => button.Title = button.Title;
        }
    }
}
