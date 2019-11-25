
using Foundation;
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

        public void Include(UISearchBar sb)
        {
            sb.Text = sb.Text + "";
            sb.Placeholder = sb.Placeholder + "";
        }
    }
}
