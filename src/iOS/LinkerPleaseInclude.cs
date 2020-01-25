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

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + "";
            textField.EditingChanged += (sender, args) => { textField.Text = ""; };
            textField.EditingDidEnd += (sender, args) => { textField.Text = ""; };
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + "";
            textView.TextStorage.DidProcessEditing += (sender, e) => textView.Text = "";
            textView.Changed += (sender, args) => { textView.Text = ""; };
        }
    }
}
